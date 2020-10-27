using System;
using System.Collections.Generic;
using Windows.Storage;

namespace AndysNMSWinStoreModFix
{
    internal static class Shims
    {
        static readonly char[] DirectorySeparators = new char[] { System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar };
        internal static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        internal static StorageFolder CreateFolderPath(this StorageFolder parent, string path)
        {
            var directories = path.Split(DirectorySeparators, System.StringSplitOptions.RemoveEmptyEntries);
            var current = parent;
            foreach (var directory in directories)
            {
                current = current.CreateFolderAsync(directory, CreationCollisionOption.OpenIfExists).GetAwaiter().GetResult();
            }
            return current;
        }
    }
}