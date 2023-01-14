using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using YSMK.SpecflowAutomation.Base;

namespace YSMK.SpecflowAutomation.Configuration
{
    public static class Settings
    {
        public static string BrowserName;
        public static string Environment;
        public static int ImplicitTimeOut;
        public static int PageLoadTimeOut;
        public static string ActitimeURL;
        public static string ValidUserName;
        public static string ValidPassword;
        public static bool IsLog;
        public static bool IsReport;
        public static string TestType;


        public static string LogPath;
        public static string LogFileName;
        public static string ReportPath;
        public static string ReportName;
        public static string ReportFullPath;

        public static BrowserType BrowserType;
        public static SqlConnection ApplicationCon { get; set; }

        private static bool _fileCreated = false;

        public static bool FileCreated
        {
            get
            {
                return _fileCreated;
            }
            set
            {
                _fileCreated = value;
            }
        }
    }
}
