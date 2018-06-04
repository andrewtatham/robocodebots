using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using Ionic.Zip;

namespace AndrewTatham.BattleTests.Helpers
{
    public static class ZipHelper
    {
        private const string Basedir = @"C:\robocode\robots";
        private const string ZipName = @"\participants-latest.zip";
        private const string Url = @"http://robocode-archive.strangeautomata.com/participants-latest.zip";
        private const string Server = "robocode-archive.strangeautomata.com";
        private const string Localzip = Basedir + ZipName;

        public static void DownloadLatestEnemiesZip()
        {
            bool zipExists = File.Exists(Localzip);
            bool zipIsInvalid = !IsZipValid();
            bool zipIsOld = DateTime.Now - File.GetLastWriteTime(Localzip) > new TimeSpan(3, 0, 0, 0, 0);

            if (!zipExists || zipIsInvalid || zipIsOld)
            {
                bool online = false;
                try
                {
                    using (Ping ping = new Ping())
                    {
                        Console.WriteLine("Pinging {0}", Server);
                        var reply = ping.Send(Server, 30000);
                        Console.WriteLine(reply.Status);
                        online = reply.Status == IPStatus.Success;
                    }
                }
                catch (PingException ex)
                {
                    Console.WriteLine(ex);
                }

                if (online)
                {
                    if (File.Exists(Localzip)) File.Delete(Localzip);

                    using (var wc = new WebClient())
                    {
                        float progress = 0;
                        Console.WriteLine("Downloading from {0}", Url);

                        wc.DownloadProgressChanged += (s, e) => { progress = e.ProgressPercentage / 100f; };
                        wc.DownloadFileCompleted += (s, e) =>
                        {
                            if (e.Error != null)
                            {
                                throw new Exception(e.Error.Message);
                            }
                            Console.WriteLine("Done");
                        };
                        wc.DownloadFileAsync(new Uri(Url), Localzip);

                        do
                        {
                            Console.WriteLine("{0:P0}", progress);
                            Thread.Sleep(15000);
                        } while (wc.IsBusy);
                    }
                }
            }
        }

        private static bool IsZipValid()
        {
            try
            {
                // ReSharper disable UnusedVariable
                using (ZipFile z = ZipFile.Read(Localzip))

                // ReSharper restore UnusedVariable
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static List<string> GetAllRobots()
        {
            // get all robots from the zip
            using (ZipFile z = ZipFile.Read(Localzip))
            {
                IEnumerable<string> jars = z.Select(x => x.FileName);

                return jars.Select(jar =>
                    {
                        var f = new FileInfo(jar);
                        return f.Name
                                .Replace(f.Extension, string.Empty)
                                .Replace("_", " ");
                    }).ToList();
            }
        }
    }
}