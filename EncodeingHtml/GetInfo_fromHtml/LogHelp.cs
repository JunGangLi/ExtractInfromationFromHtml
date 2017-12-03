using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Add_in_PlacSetAttributeTool
{
    class LogHelp
    {
        private static string LogPath = @"C:\Windows\Temp";

        public static void WriteLog(Exception err, string prefix)
        {
            if (!Directory.Exists(LogPath + "\\" + prefix))
            {
                Directory.CreateDirectory(LogPath + "\\" + prefix);               
            }
            StreamWriter sw = new StreamWriter(LogPath + "\\" + prefix + "\\"+prefix+"_Log.txt", true);
            sw.WriteLine();
            sw.WriteLine("********" + DateTime.Now.ToString() + "********");
            sw.WriteLine(err.Message);
            sw.WriteLine(err.StackTrace);            
            sw.WriteLine(err.Source);
            sw.WriteLine(err.Data);           
            sw.Close();
        }

        private static void CreatePath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                LogPath = path;
            }
        }
    }
}
