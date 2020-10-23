using System;
using System.IO;
using Windows.Storage;

namespace AndysNMSWinStoreModFix
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Welcome to ANMSMEMSPC: Andy's NMS Mod Enabler for the Microsoft Store / Xbox GamePass PC edition.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("Finding No Man's Sky Installation...");
            var package = new NmsPackage();
            var tombstoneFile = new FileInfo(Path.Combine(package.PcBanksFolder.Path, @"DISABLEMODS.TXT"));
            var ModsFolder = package.PcBanksFolder.CreateFolderAsync("MODS", CreationCollisionOption.OpenIfExists).GetAwaiter().GetResult();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Created MODS folder:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\t{0}", ModsFolder.Path);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("Marking DISABLEMODS.TXT as deleted...");
            tombstoneFile.CreateWciTombstone();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Done!");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine("You shouldn't have to run this again unless you reinstall No Man's Sky.");
            Console.WriteLine();
            Console.WriteLine("There is a hidden system file named disablemods.txt in the PCBANKS folder.");
            Console.WriteLine("It is a reparse point tombstone marker. It's what hides/deletes DISABLEDMODS.TXT.");
            Console.WriteLine("You may need to first run this console command before you can delete that file:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\tfsutil reparsepoint delete disablemods.txt");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.WriteLine("Of course... why would you want to do that? That would disable mods again.");
            Console.WriteLine();
            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }
    }
}