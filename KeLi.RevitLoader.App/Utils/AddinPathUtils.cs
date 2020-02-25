/*
 * MIT License
 *
 * Copyright(c) 2019 KeLi
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

/*
             ,---------------------------------------------------,              ,---------,
        ,----------------------------------------------------------,          ,"        ,"|
      ,"                                                         ,"|        ,"        ,"  |
     +----------------------------------------------------------+  |      ,"        ,"    |
     |  .----------------------------------------------------.  |  |     +---------+      |
     |  | C:\>FILE -INFO                                     |  |  |     | -==----'|      |
     |  |                                                    |  |  |     |         |      |
     |  |                                                    |  |  |/----|`---=    |      |
     |  |              Author: kelicto                       |  |  |     |         |      |
     |  |              Email: kelistudy@163.com              |  |  |     |         |      |
     |  |              Creation Time: 11/22/2019 12:01:34 AM |  |  |     |         |      |
     |  | C:\>_                                              |  |  |     | -==----'|      |
     |  |                                                    |  |  |   ,/|==== ooo |      ;
     |  |                                                    |  |  |  // |(((( [66]|    ,"
     |  `----------------------------------------------------'  |," .;'| |((((     |  ,"
     +----------------------------------------------------------+  ;;  | |         |,"
        /_)_________________________________________________(_/  //'   | +---------+
           ___________________________/___  `,
          /  oooooooooooooooo  .o.  oooo /,   \,"-----------
         / ==ooooooooooooooo==.o.  ooo= //   ,`\--{)B     ,"
        /_==__==========__==_ooo__ooo=_/'   /___________,"
*/

using System;
using System.IO;
using System.Security.Principal;

using static System.Configuration.ConfigurationManager;

namespace KeLi.RevitLoader.App.Utils
{
    public class AddinPathUtils
    {
        private static string AlluserPath { get; set; }

        private static string CurrentUserPath { get; set; }

        public static string GetAllUserAppDataPath()
        {
            return AlluserPath ?? (AlluserPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
        }

        public static string GetAllUserPath(string revitVersion)
        {
            if (revitVersion == null)
                throw new ArgumentNullException(nameof(revitVersion));

            return Path.Combine(GetAllUserAppDataPath(), AppSettings["AddinsPath"], revitVersion);
        }

        public static string GetCurrentUserAppDataPath()
        {
            return CurrentUserPath ?? (CurrentUserPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        }

        public static string GetCurrentUserPath(string revitVersion)
        {
            if (revitVersion == null)
                throw new ArgumentNullException(nameof(revitVersion));

            var appData = GetCurrentUserAppDataPath();

            var dir = Path.Combine(appData, AppSettings["AddinsPath"], revitVersion);

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
            if (version == null)
                throw new ArgumentNullException(nameof(version));

            if (addinFilename == null)
                throw new ArgumentNullException(nameof(addinFilename));

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