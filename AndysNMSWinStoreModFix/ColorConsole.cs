using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

// https://gist.github.com/tomzorz/6142d69852f831fb5393654c90a1f22e

namespace AndysNMSWinStoreModFix
{
    static class ColorConsole
    {
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;


        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        public static readonly string NewLine = System.Environment.NewLine;

        public const char Esc = '\u001b';
        public static readonly string Normal = $"{Esc}[0m";
        public static readonly string Underline = $"{Esc}[4m";
        public static readonly string NoUnderline = $"{Esc}[24m";
        public static readonly string FgRed = $"{Esc}[31m";
        public static readonly string FgYellow = $"{Esc}[33m";
        public static readonly string FgBrightRed = $"{Esc}[91m";
        public static readonly string FgBrightGreen = $"{Esc}[92m";
        public static readonly string FgBrightYellow = $"{Esc}[93m";
        public static readonly string FgBrightMagenta = $"{Esc}[95m";
        public static readonly string FgBrightCyan = $"{Esc}[96m";
        public static readonly string FgBrightWhite = $"{Esc}[97m";

        private static StringBuilder ErrorLogBuilder;

        static ColorConsole()
        {
            // Enable VT100
            var stdOutHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            GetConsoleMode(stdOutHandle, out uint ConsoleMode);
            SetConsoleMode(stdOutHandle, ConsoleMode | ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN);
            ErrorLogBuilder = new StringBuilder();
        }

        private static void Write(string text)
        {
            ErrorLogBuilder.Append(text);
            Console.Write(text);
        }

        private static void WriteLine(string text) =>
            Write($"{text}{NewLine}");

        public static void WriteIntroduction()
        {
            WriteLine($"{FgBrightYellow}ANMSMEMSPC ({Normal}v{Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}{FgBrightYellow}): " +
                      $"{FgBrightCyan}Andy's NMS Mod Enabler for the Microsoft Store / Xbox GamePass PC edition.");
            WriteLine($"{Normal}Nexus Mods: {FgBrightWhite}{Underline}https://www.nexusmods.com/nomanssky/mods/1751{NoUnderline} ");
            WriteLine($"{Normal}Source Code: {FgBrightWhite}{Underline}https://github.com/AndASM/ANMSMEMSPC{NoUnderline}{Normal} ");
            WriteLine("License: GPL-3.0-only");
            WriteBlankLine();
        }

        public static void WriteExplanation()
        {
            WriteInfo($"{Normal}You shouldn't have to run this again unless you reinstall No Man's Sky.");
            WriteBlankLine();
            WriteInfo("There is a hidden system file named disablemods.txt in the PCBANKS folder.");
            WriteInfo("It is a reparse point tombstone marker. It's what hides/deletes DISABLEDMODS.TXT.");
            WriteNameValue("You may need to first run this console command before you can delete that file",
                "fsutil reparsepoint delete disablemods.txt");
            WriteBlankLine();
            WriteInfo("Of course... why would you want to do that? That would disable mods again.");
            WriteBlankLine();
        }

        public static void WriteInfo(string info) => WriteLine($"{Normal}{info}");

        public static void WriteNameValue(string name, string value) =>
            WriteLine($"{Normal}{name}:{NewLine}" +
                              $"{FgBrightWhite}\t{value}{Normal}");

        public static void WriteBlankLine() => WriteLine("");

        public static void WriteTryExcept(string actionText, string success, Action actionCode)
        {
            try
            {
                WriteInfo(actionText);
                actionCode();
                WriteLine($"{FgBrightGreen}{success}{Normal}");
            }
            catch (Exception e)
            {
                WriteException(e.ToString());
            }
        }

        public static void WriteException(string errorText)
        {
            var errorLogFile = new FileInfo("ErrorLog.txt");

            WriteLine($"{FgBrightRed}Error!");
            WriteLine($"Please check {Normal}{Underline}https://github.com/AndASM/ANMSMEMSPC/issues{NoUnderline}{FgBrightRed} for this issue.");
            WriteLine("If it hasn't been reported, please click the New Issue button and report it.");
            WriteLine("Please include the error log available here:");
            WriteLine($"{FgBrightWhite}{errorLogFile.FullName}{FgBrightRed}");
            WriteBlankLine();

            WriteLine("Important technical details:");
            WriteLine($"{FgRed}{errorText}{Normal}");

            if (errorLogFile.Exists) errorLogFile.Delete();
            using var errorOutput = errorLogFile.CreateText();
            errorOutput.WriteLine(
                Regex.Replace(
                    ErrorLogBuilder.ToString(),
                    @"\u001b\[\d+m",
                    ""
                ));
            errorOutput.Flush();

            ConfirmQuit();
            Environment.Exit(1);
        }

        public static void ConfirmQuit()
        {
            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
