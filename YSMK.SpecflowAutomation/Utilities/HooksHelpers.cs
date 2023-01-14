using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using YSMK.SpecflowAutomation.Configuration;

namespace YSMK.SpecflowAutomation.Utilities
{
    public static class HooksHelpers
    {

        public static void KillProcesses()
        {
            string driverName = Settings.BrowserName.Equals("CHROME") ? "chromedriver" : Settings.BrowserName.Equals("EDGE") ? "msedgedriver" : "geckodriver";
            try
            { 

                Process[] processes = Process.GetProcessesByName(driverName);
                Console.WriteLine("====================== Total open "+driverName+"'s" + processes.Length + " ======================");
                foreach (var process in processes)
                {
                    process.Kill();
                }
                Console.WriteLine("====================== Closed all the " + driverName + "'s ======================");
            }
            catch(Exception e)
            {
                Console.WriteLine("====================== Failed to all the " + driverName + "'s ======================");
                throw new Exception(" Exception in Process Killing : "+e);
            }
        }

    }
}
