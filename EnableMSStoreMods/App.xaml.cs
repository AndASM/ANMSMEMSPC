using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Threading;

namespace AndASM_NMS.EnableMSStoreMods
{
	/// <summary>
	///     Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			var errorLogFile = new FileInfo("ErrorLog.txt");
			if (errorLogFile.Exists) errorLogFile.Delete();
			using var errorOutput = errorLogFile.CreateText();

			errorOutput.WriteLine("Error!");
			errorOutput.WriteLine(
				$"Please check {Assembly.GetEntryAssembly().GetCustomAttributes<AssemblyMetadataAttribute>().FirstOrDefault(attr => attr.Key == "PackageProjectUrl")?.Value}/issues for this issue.");
			errorOutput.WriteLine("If it hasn't been reported, please click the New Issue button and report it.");
			errorOutput.WriteLine("Please include the error log available here:");
			errorOutput.WriteLine($"{errorLogFile.FullName}");
			errorOutput.WriteLine();

			errorOutput.WriteLine("Important technical details:");
			errorOutput.WriteLine($"{e.Exception}");

			errorOutput.Flush();
			throw e.Exception;
		}
	}
}