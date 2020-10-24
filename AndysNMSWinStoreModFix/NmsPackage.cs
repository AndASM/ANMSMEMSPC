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
    class NmsPackage
    {
        public IReadOnlyCollection<String> Capabilities
        {
            get { return _capabilities; }
        }
        public string FamilyName
        {
            get { return _uwpPackage.Id.FamilyName; }
        }
        public StorageFolder PcBanksFolder
        {
            get
            {
                return _appData.LocalCacheFolder.GetFolderAsync(
                    @"Local\Microsoft\WritablePackageRoot\GAMEDATA\PCBANKS").GetAwaiter().GetResult();
            }
        }

        private readonly Package _uwpPackage;
        private readonly HashSet<string> _capabilities;
        private readonly ApplicationData _appData;
        public NmsPackage()
        {
            var packageManager = new PackageManager();
            _uwpPackage = packageManager.FindPackagesForUser("").First(package => package.Id.Name.Equals("HelloGames.NoMansSky", StringComparison.CurrentCultureIgnoreCase));
            XElement gameManifest;
            using var manifestFileStream = _uwpPackage.InstalledLocation.GetFileAsync(@"AppxManifest.xml").GetAwaiter()
                .GetResult().OpenStreamForReadAsync().GetAwaiter().GetResult();
                gameManifest = XElement.Load(manifestFileStream);

            _capabilities = gameManifest.DescendantsAndSelf().Where(element => element.Name.LocalName == "Capability").Select(element => element.Attribute("Name")?.Value).ToHashSet();
            _appData = ApplicationDataManager.CreateForPackageFamily(FamilyName);
        }
    }
}
