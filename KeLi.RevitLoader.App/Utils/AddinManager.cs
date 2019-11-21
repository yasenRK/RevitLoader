using System.Collections.Generic;
using System.IO;
using KeLi.RevitLoader.App.Entities;

namespace KeLi.RevitLoader.App.Utils
{
    public class AddinManager
    {
        public string AddinFilePath { get; set; }

        public Dictionary<int, string> AddinEntries { get; set; }

        public void Run(bool uninstall)
        {
            var addinFileName = Path.GetFileName(AddinFilePath);

            if (uninstall)
            {
                foreach (var addinEntry in AddinEntries)
                    AddinPathUtils.RemovePlugin(addinEntry.Key.ToString(), addinFileName);

                return;
            }

            foreach (var addinEntry in AddinEntries)
            {
                var addinFolder = AddinPathUtils.GetCurrentUserPath(addinEntry.Key.ToString());
                var newAddinFile = Path.Combine(addinFolder, addinFileName);

                File.Copy(AddinFilePath, newAddinFile, true);

                var addins = SerialUtils.GetObject<RevitAddIns>(newAddinFile);

                addins.AddIn.Assembly = addinEntry.Value;
                SerialUtils.ToFile(newAddinFile, addins);
            }
        }
    }
}