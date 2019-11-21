using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using KeLi.RevitLoader.App.Properties;
using KeLi.RevitLoader.App.Utils;

namespace KeLi.RevitLoader.App
{
    public class Program
    {
        private static string AddinFilename { get; }

        private static string CurrentFolder { get; }
            = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        static Program()
        {
            AddinFilename = ConfigurationManager.AppSettings["addin"];
        }

        private static void Main(string[] args)
        {
            try
            {
                GetAddins(Resources.MyAddin_Pattern).Run(args.Length > 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static AddinManager GetAddins(string addinPattern)
        {
            var addins = new AddinManager();
            var versionDlls = new Dictionary<int, string>();
            var filePaths = Directory.GetFiles(CurrentFolder, addinPattern);

            foreach (var filePath in filePaths)
            {
                var match = Regex.Match(filePath, @"(\d\d\d\d)\.dll$");

                if (!match.Success)
                    continue;

                versionDlls.Add(int.Parse(match.Groups[1].Value), filePath);
            }

            addins.AddinEntries = versionDlls;

            var addinFile = Path.Combine(CurrentFolder, AddinFilename);

            if (File.Exists(addinFile))
                addins.AddinFilePath = addinFile;

            return addins;
        }
    }
}