using System;
using System.IO;
using YSMK.SpecflowAutomation.Configuration;

namespace YSMK.SpecflowAutomation.Utilities
{
    public class LogHelpers
    {
        //Global Declaration
        private static StreamWriter _streamw = null;

        //Create a file which can store the log information
        public static void CreateLogFile()
        {
            string dir = Settings.LogPath;
            if (Settings.FileCreated == false)
                if (Directory.Exists(dir))
                {
                    Settings.FileCreated = true;
                    _streamw = File.AppendText(dir + Settings.LogFileName);
                }
                else
                {
                    Settings.FileCreated = true;
                    Directory.CreateDirectory(dir);
                    _streamw = File.AppendText(dir + Settings.LogFileName);
                }
        }



        //Create a method which can write the text in the log file
        public static void Write(string logMessage)
        {
            _streamw.Write("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            _streamw.WriteLine("    {0}", logMessage);
            _streamw.Flush();
        }

        public static void Write(string logMessage, Exception innerException)
        {
            _streamw.Write("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            _streamw.WriteLine("    {0}", logMessage);
            _streamw.WriteLine("    {0}", innerException);
            _streamw.Flush();
        }

    }
}
