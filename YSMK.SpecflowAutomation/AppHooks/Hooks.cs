using BoDi;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;
using YSMK.SpecflowAutomation.Base;
using YSMK.SpecflowAutomation.Configuration;
using YSMK.SpecflowAutomation.Utilities;

namespace YSMK.SpecflowAutomation.AppHooks
{
    [Binding]
    public class Hooks : TestInitializeHook
    {
        private readonly FeatureContext _featureContext;
        private readonly ScenarioContext _scenarioContext;
        private static ISpecFlowOutputHelper _specFlowOutputHelper;
        private IObjectContainer _objectContainer;


        public static string resultsPath;
        public static string ScreenshotPath;

        public Hooks(IObjectContainer objectContainer, FeatureContext featureContext, ScenarioContext scenarioContext, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            _featureContext = featureContext;
            _scenarioContext = scenarioContext;
            _specFlowOutputHelper = specFlowOutputHelper;
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            ConfigReader.InitializeConfigurationSettings();
            ReportHelpers.GetExtent();
            


            resultsPath = GenericHelpers.GetProjectPathOfFolder("TestReports");
            Settings.LogPath = resultsPath + "\\LogFolder\\";
            Settings.ReportPath = resultsPath + "\\ExtentReports\\";
            ScreenshotPath = Settings.ReportPath + "\\Screenshots\\";

            if (!Directory.Exists(Settings.LogPath))  Directory.CreateDirectory(Settings.LogPath);
            if (!Directory.Exists(Settings.ReportPath))  Directory.CreateDirectory(Settings.ReportPath);
            if (!Directory.Exists(ScreenshotPath))  Directory.CreateDirectory(ScreenshotPath);

            LogHelpers.CreateLogFile();
            LogHelpers.Write("Framework Initialized");

        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContexts)
        {
            var featureTile = featureContexts.FeatureInfo.Title;
            ReportHelpers.CreateFeatureTest(featureTile);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            LogHelpers.Write("****************Started Exceution******************");
            LogHelpers.Write(_scenarioContext.ScenarioInfo.Title);
            ReportHelpers.CreateScenarioTest(_scenarioContext.ScenarioInfo.Title);
            _specFlowOutputHelper.WriteLine("Environment : " + Settings.Environment);
            _specFlowOutputHelper.WriteLine("Browser : " + Enum.GetName(typeof(BrowserType), Settings.BrowserType));
            _specFlowOutputHelper.WriteLine("Scenario : " + _scenarioContext.ScenarioInfo.Title);
            LogHelpers.Write("***************************************************");
            OpenBrowser(Settings.BrowserType);
            if (DriverContext.Driver != null)
                _objectContainer.RegisterInstanceAs(DriverContext.Driver);

        }

        [BeforeStep]
        public void BeforeStep()
        {
            _specFlowOutputHelper.WriteLine("-----------------------------------");
            string stepName = _scenarioContext.StepContext.ToString();
            ReportHelpers.CreateStepTest(_scenarioContext);
        }


        [AfterStep]
        public void AfterStep()
        {
            _specFlowOutputHelper.WriteLine("-----------------------------------");
            if (_scenarioContext.TestError != null)
            {
                #region Error Message 
                string Failmsg = "Message: " + _scenarioContext.TestError.Message +
                            "Inner Exception " + _scenarioContext.TestError.InnerException +
                             "Stack Trace" + _scenarioContext.TestError.StackTrace;
                #endregion

                if (Settings.BrowserType != BrowserType.WebService)
                {
                    #region ScreenShot
                    Random rad = new Random();
                    string Imgname = "image" + rad.Next(0, 10000) + ".png";
                    ((ITakesScreenshot)DriverContext.Driver).GetScreenshot().SaveAsFile(ScreenshotPath + Imgname);
                    string f = @"./Screenshots/" + Imgname;
                    _specFlowOutputHelper.WriteLine(Failmsg);
                    _specFlowOutputHelper.AddAttachment(f);
                    TestContext.AddTestAttachment(Path.GetFullPath(ScreenshotPath + Imgname));
                    #endregion
                }
                else
                _specFlowOutputHelper.WriteLine(Failmsg);
            }
        }
        
        [AfterScenario]
        public void AfterScenario()
        {
            if (DriverContext.Driver != null)
            {
                DriverContext.Driver.Close();
                DriverContext.Driver.Quit();
                DriverContext.Driver.Dispose();
            }
            //CleanupResources();
            //if (Settings.CloseBrowserInstances.Equals("y", StringComparison.OrdinalIgnoreCase))
            //    CleanupOpenBrowser();

            LogHelpers.Write("***************************************************");
            LogHelpers.Write(_scenarioContext.ScenarioInfo.Title);
            LogHelpers.Write("****************Completed Exceution****************");

        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            ReportHelpers.CloseReport(DriverContext.Driver);
            //HooksHelpers.KillProcesses();
            LogHelpers.Write("***************************************************");
            LogHelpers.Write("Adding attachment to Specflow and Test Result");
            _specFlowOutputHelper.AddAttachment(Settings.LogPath + Settings.LogFileName);
            TestContext.AddTestAttachment(Settings.LogPath + Settings.LogFileName);
        }

    }
}
