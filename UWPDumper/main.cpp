#include <cstddef>
#include <cstddef>
#include <fstream>
#include <iomanip>

#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include <ShlObj.h>

#include <windows.storage.h>
#include <windows.system.h>
#include <wrl.h>

#define WIDEIFYIMP(x) L##x
#define WIDEIFY(x) WIDEIFYIMP(x)

#include <queue>

#include <filesystem>
namespace fs = std::filesystem;

#include <UWP/UWP.hpp>

#include <UWP/DumperIPC.hpp>

std::uint32_t __stdcall DumperThread(void* DLLHandle)
{
	IPC::SetTargetThread(GetCurrentThreadId());

	IPC::PushMessage(L"NMSFix Build date(%ls : %ls)\n", WIDEIFY(__DATE__), WIDEIFY(__TIME__));
	IPC::PushMessage(L"\t-https://github.com/AndASM/AQDNMSMEMXGPPC\n");
	IPC::PushMessage(L"Package Path:\n\t%s\n", UWP::Current::GetPackagePath().c_str());

	std::vector<fs::directory_entry> FileList;

	for( auto& Entry : fs::recursive_directory_iterator(UWP::Current::GetPackagePath()) )
	{
		if( fs::is_regular_file(Entry.path()) )
		{
			FileList.push_back(Entry);
		}
	}

	IPC::PushMessage(L"Scanning %zu files\n", FileList.size());

	std::size_t i = 0;
	for( const auto& File : FileList )
	{
		try
		{

			if (_stricmp(File.path().filename().string().c_str(), "DISABLEMODS.TXT") == 0)
			{
				IPC::PushMessage(L"Deleting File:\n\t%s\n", File.path().filename().c_str());
				remove(File);
				
				IPC::PushMessage(L"Deletion \033[92mcomplete\033[0m!\n");
				IPC::ClearTargetThread();

				FreeLibraryAndExitThread(
					reinterpret_cast<HMODULE>(DLLHandle),
					EXIT_SUCCESS
				);
			}
		}
		
		catch( std::exception& Exception )
		{
			//std::wstring ExceptionMessage = std::wstring_convert<std::codecvt_utf8<wchar_t>, wchar_t>{}.from_bytes( Exception.what() );

			IPC::PushMessage(
				L"Exception {%s}\n",
				Exception.what()
			);
		}
	}

	IPC::PushMessage(L"File not found!\n\tMods already enabled?\n");
	IPC::ClearTargetThread();

	FreeLibraryAndExitThread(
		reinterpret_cast<HMODULE>(DLLHandle),
		EXIT_SUCCESS
	);
}

std::int32_t __stdcall DllMain(HINSTANCE hDLL, std::uint32_t Reason, void* Reserved)
{
	switch( Reason )
	{
	case DLL_PROCESS_ATTACH:
	{
		IPC::PushMessage(L"DLL Attached to process %u\n", GetCurrentProcessId());
		if( IPC::GetTargetProcess() == GetCurrentProcessId() )
		{
			IPC::PushMessage(L"Creating file deletion thread%u\n");
			CreateThread(
				nullptr,
				0,
				reinterpret_cast<unsigned long(__stdcall*)(void*)>(&DumperThread),
				hDLL,
				0,
				nullptr
			);
		}
	}
	case DLL_PROCESS_DETACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	default:
	{
		return true;
	}
	}

	return false;
}
