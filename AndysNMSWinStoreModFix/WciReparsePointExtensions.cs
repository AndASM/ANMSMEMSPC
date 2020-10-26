using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace AndysNMSWinStoreModFix
{
    public static class WciReparsePointExtensions
    {
        public enum IoControlCode : uint
        {
            FsctlSetReparsePoint = 0x000900A4,
            FsctlGetReparsePoint = 0x000900A8,
            FsctlDeleteReparsePoint = 0x000900AC
        }

        private const uint IoReparseTagWciTombstone = 0xA000001F;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeviceIoControl(
            SafeFileHandle hDevice,
            IoControlCode ioControlCode,
            ref ReparseDataBufferWciTombstone inBuffer,
            int inBufferSize,
            IntPtr outBuffer,
            int outBufferSize,
            out int pBytesReturned,
            IntPtr lpOverlapped
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern SafeFileHandle CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
        );

        public static void CreateWciTombstone(this FileInfo tombstoneFile)
        {
            if (tombstoneFile.Exists)
                tombstoneFile.Delete();

            using var handle = CreateFile(tombstoneFile.FullName, 0x40000000, 0x7, IntPtr.Zero, 2, 0x2200006,
                IntPtr.Zero);
            if (Marshal.GetLastWin32Error() != 0)
                throw new IOException("Cannot create deletion tombstone.",
                    Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()));

            var buffer = new ReparseDataBufferWciTombstone
                {ReparseTag = IoReparseTagWciTombstone, ReparseDataLength = 0};
            DeviceIoControl(handle, IoControlCode.FsctlSetReparsePoint, ref buffer, Marshal.SizeOf(buffer), IntPtr.Zero,
                0, out _, IntPtr.Zero);

            if (Marshal.GetLastWin32Error() != 0)
                throw new IOException("Cannot apply reparse point tag.",
                    Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()));
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct ReparseDataBufferWciTombstone
        {
            public uint ReparseTag;
            public ushort ReparseDataLength;
            public ushort Reserved;
        }
    }
}