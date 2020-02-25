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

                if (addinFileName == null)
                    continue;

                var newAddinFile = Path.Combine(addinFolder, addinFileName);

                File.Copy(AddinFilePath, newAddinFile, true);

                var addins = XmlUtil.Deserialize<RevitAddIns>(newAddinFile);

                addins.AddIn.Assembly = addinEntry.Value;
                XmlUtil.Serialize(newAddinFile, addins);
            }
        }
    }
}