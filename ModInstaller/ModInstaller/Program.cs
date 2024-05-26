using Microsoft.VisualBasic;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace Unix
{
    public class BepinexInstaller
    {
        public static string GetPathToInstall()
        {
            string result = "";
            if (Environment.SystemDirectory.Contains(@"C:\"))
            {
                if (Directory.Exists(@"C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag"))
                    result = @"C:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag";
                else
                    result = @"C:\Program Files\Oculus\Software\Software\another-axiom-gorilla-tag";
            }
            else
            {
                if (Directory.Exists(@"D:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag"))
                    result = @"D:\Program Files (x86)\Steam\steamapps\common\Gorilla Tag";
                else
                    result = @"D:\Program Files\Oculus\Software\Software\another-axiom-gorilla-tag";
            }
            return result;
        }

        static bool alreadyInstalledBepInEx = false;
        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("[1] Install BepInEx");
            Console.WriteLine("[2] Exit");
            Console.Write("> ");
            var opt = Console.ReadLine();
            switch (opt)
            {
                case "1":
                    #region Backing up BepInEx Files
                    if (Directory.Exists(GetPathToInstall() + @"\BepInEx"))
                    {
                        string src = $@"{GetPathToInstall()}\BepInEx\plugins";
                        string backup = AppDomain.CurrentDomain.BaseDirectory + @"\Backup";
                        Directory.CreateDirectory(backup);
                        string[] f = Directory.GetFiles(src);
                        foreach (string str in f)
                        {
                            string fn = Path.GetFileName(str);
                            string tg = Path.Combine(backup, fn);
                            File.Copy(str, tg, true);
                        }
                        Directory.Delete(GetPathToInstall() + @"\BepInEx", true);
                        File.Delete(GetPathToInstall() + @"\.doorstop_version");
                        File.Delete(GetPathToInstall() + @"\changelog.txt");
                        File.Delete(GetPathToInstall() + @"\doorstop_config.ini");
                        File.Delete(GetPathToInstall() + @"\winhttp.dll");
                        alreadyInstalledBepInEx = true;
                    }
                    #endregion
                    #region Downloading and Extracting BepInEx Files
                    string bepinex = "https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.1/BepInEx_win_x64_5.4.23.1.zip";
                    WebClient c = new WebClient();
                    c.DownloadFile(bepinex, "BepInEx_win_x64_5.4.23.1.zip");
                    ZipFile.ExtractToDirectory("BepInEx_win_x64_5.4.23.1.zip", GetPathToInstall());
                    File.Delete($@"{AppDomain.CurrentDomain.BaseDirectory}\BepInEx_win_x64_5.4.23.1.zip");
                    Directory.CreateDirectory($@"{GetPathToInstall()}\BepInEx\plugins");
                    #endregion
                    #region Restoring Files
                    if (alreadyInstalledBepInEx)
                    {
                        string src = $@"{GetPathToInstall()}\BepInEx\plugins";
                        string backup = AppDomain.CurrentDomain.BaseDirectory + @"\Backup";
                        string[] f = Directory.GetFiles(backup);
                        foreach (string str in f)
                        {
                            string fn = Path.GetFileName(str);
                            string tg = Path.Combine(src, fn);
                            File.Copy(str, tg, true);
                        }
                    }
                    #endregion
                    break;
                case "2":
                    Environment.Exit(-1);
                    break;
            }
        }
    }
}