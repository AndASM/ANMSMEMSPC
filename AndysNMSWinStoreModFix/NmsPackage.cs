using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Management.Core;
using Windows.Management.Deployment;
using Windows.Storage;

namespace AndysNMSWinStoreModFix
{
    internal class NmsPackage
    {
        private readonly ApplicationData _appData;
        private readonly HashSet<string> _capabilities;

        private readonly Package _uwpPackage;

        public NmsPackage()
        {
            var packageManager = new PackageManager();
            _uwpPackage = packageManager.FindPackagesForUser("").First(package =>
                package.Id.Name.Equals("HelloGames.NoMansSky", StringComparison.CurrentCultureIgnoreCase));
            XElement gameManifest;
            using var manifestFileStream = _uwpPackage.InstalledLocation.GetFileAsync(@"AppxManifest.xml").GetAwaiter()
                .GetResult().OpenStreamForReadAsync().GetAwaiter().GetResult();
            gameManifest = XElement.Load(manifestFileStream);

            _capabilities = gameManifest.DescendantsAndSelf().Where(element => element.Name.LocalName == "Capability")
                .Select(element => element.Attribute("Name")?.Value).ToHashSet();
            _appData = ApplicationDataManager.CreateForPackageFamily(FamilyName);
        }

        public IReadOnlyCollection<string> Capabilities => _capabilities;

        public string FamilyName => _uwpPackage.Id.FamilyName;

        public StorageFolder PcBanksFolder =>
            _appData.LocalCacheFolder.GetFolderAsync(
                @"Local\Microsoft\WritablePackageRoot\GAMEDATA\PCBANKS").GetAwaiter().GetResult();
    }
}