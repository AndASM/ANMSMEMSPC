using System;
using System.IO;
using Windows.Storage;
using Console = AndysNMSWinStoreModFix.ColorConsole;

namespace AndysNMSWinStoreModFix
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                DoEverything();
                Console.ConfirmQuit();
            }
            catch (Exception e)
            {
                Console.WriteException(e.ToString());
            }
        }

        private static void DoEverything()
        {
            Console.WriteIntroduction();

            Console.WriteInfo("Finding No Man's Sky Installation...");
            var package = new NmsPackage();
            var tombstoneFile = new FileInfo(Path.Combine(package.PcBanksFolder.Path, @"DISABLEMODS.TXT"));
            var ModsFolder = package.PcBanksFolder.CreateFolderAsync("MODS", CreationCollisionOption.OpenIfExists)
                .GetAwaiter().GetResult();

            Console.WriteNameValue("Created MODS folder", ModsFolder.Path);
            Console.WriteBlankLine();

            Console.WriteTryExcept("Marking DISABLEMODS.TXT as deleted...",
                "File deleted, mods enabled!",
                () => tombstoneFile.CreateWciTombstone());
            Console.WriteBlankLine();

            Console.WriteExplanation();
        }
    }
}