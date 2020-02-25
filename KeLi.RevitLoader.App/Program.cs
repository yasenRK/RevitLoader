﻿/*
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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

using KeLi.RevitLoader.App.Utils;

using static System.Configuration.ConfigurationManager;

namespace KeLi.RevitLoader.App
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                GetAddins().Run(args.Length > 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static AddinManager GetAddins()
        {
            var currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (currentFolder == null)
                throw  new NullReferenceException(nameof(currentFolder));

            var addins = new AddinManager();

            var versionDlls = new Dictionary<int, string>();

            var filePaths = Directory.GetFiles(currentFolder, AppSettings["AddinPattern"]);

            foreach (var filePath in filePaths)
            {
                var match = Regex.Match(filePath, @"(\d\d\d\d)\.dll$");

                if (!match.Success)
                    continue;

                versionDlls.Add(int.Parse(match.Groups[1].Value), filePath);
            }

            addins.AddinEntries = versionDlls;

            var addinFile = Path.Combine(currentFolder, AppSettings["AddinFileName"]);

            if (File.Exists(addinFile))
                addins.AddinFilePath = addinFile;

            return addins;
        }
    }
}