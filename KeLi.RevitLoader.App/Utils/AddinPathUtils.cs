using System;
using System.IO;
using System.Security.Principal;
using KeLi.RevitLoader.App.Properties;

namespace KeLi.RevitLoader.App.Utils
{
    public class AddinPathUtils
    {
        private static string RevitFragment { get; } = Resources.Revit_Addins_Path;

        private static string AlluserPath { get; set; }

        private static string CurrentUserPath { get; set; }

        public static string GetAllUserAppDataPath()
        {
            return AlluserPath ?? (AlluserPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
        }

        public static string GetAllUserPath(string revitVersion)
        {
            return Path.Combine(GetAllUserAppDataPath(), RevitFragment, revitVersion);
        }

        public static string GetCurrentUserAppDataPath()
        {
            return CurrentUserPath ?? (CurrentUserPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        }

        public static string GetCurrentUserPath(string revitVersion)
        {
            var appData = GetCurrentUserAppDataPath();
            var dir = Path.Combine(appData, RevitFragment, revitVersion);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
        }

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static bool RemovePlugin(string version, string addinFilename, bool allUser = false)
        {
            if (allUser && !IsAdministrator())
                return false;

            var addinPath = allUser ? GetAllUserPath(version) : GetCurrentUserPath(version);

            if (!Directory.Exists(addinPath))
                return false;

            var addinFile = Path.Combine(addinPath, addinFilename);

            if (!File.Exists(addinFile))
                return true;

            try
            {
                File.Delete(addinFile);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}