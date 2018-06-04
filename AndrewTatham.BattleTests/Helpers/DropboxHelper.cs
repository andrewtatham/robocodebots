using System;
using System.IO;
using System.Text;

namespace AndrewTatham.BattleTests.Helpers
{
    public static class DropboxHelper
    {
        public static string GetDropboxPath()
        {
            var dbPath = Path.Combine(
                     Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Dropbox\\host.db");

            string[] lines = File.ReadAllLines(dbPath);
            byte[] dbBase64Text = Convert.FromBase64String(lines[1]);
            string folderPath = Encoding.ASCII.GetString(dbBase64Text);
            Console.WriteLine(folderPath);
            return folderPath;
        }
    }
}