using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Text;
using YSMK.SpecflowAutomation.Base;
using YSMK.SpecflowAutomation.Utilities;

namespace YSMK.SpecflowAutomation.Configuration
{
    public static class ConfigReader
    {
        static IConfigurationRoot configurationRoot;
       public static void InitializeConfigurationSettings()
        {
            try
            {
                var builder = new ConfigurationBuilder()          //MS.Ext.Configuration package
                    .SetBasePath(Directory.GetCurrentDirectory()) //MS.Ext.Config.FileExtensions package
                    .AddJsonFile("appsettings.json");             //MS.Ext.Config.Json package

                configurationRoot = builder.Build();


                Settings.Environment        = GetConfigValue<string>("testSettings:environment");
                Settings.BrowserType = GetConfigValue<BrowserType>("testSettings:browser");
                Settings.ImplicitTimeOut    = GetConfigValue<int>("testSettings:implicitTimeOut");
                Settings.PageLoadTimeOut    = GetConfigValue<int>("testSettings:pageLoadTImeOut");
                Settings.ActitimeURL        = GetConfigValue<string>("testSettings:Environments:" + Settings.Environment + ":actiTimeURL");
                Settings.ValidUserName      = GetConfigValue<string>("testSettings:Environments:" + Settings.Environment + ":validUserName");
                Settings.ValidPassword      = GetConfigValue<string>("testSettings:Environments:" + Settings.Environment + ":validPassword");

                Settings.IsLog              = GetConfigValue<bool>("testSettings:isLog");
                Settings.IsReport           = GetConfigValue<bool>("testSettings:isReport");
                Settings.LogFileName        = string.Format("{0:yyyymmddhhmmss}", DateTime.Now) + ".log";

                
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw new Exception();
            }
           

        }


        private static T GetConfigValue<T>(string key)
        {
            return configurationRoot.GetValue<T>(key); //MS.Ext.Config.Binder package
        }
    }
}
