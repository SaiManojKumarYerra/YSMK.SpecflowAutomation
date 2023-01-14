using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TechTalk.SpecFlow;
using YSMK.SpecflowAutomation.Configuration;

namespace YSMK.SpecflowAutomation.Utilities
{
    public static class ReportHelpers
    {

        public static ExtentTest featureTest { get; set; }
        public static ExtentTest scenarioTest { get; set; }
        public static ExtentTest nodeTest { get; set; }
        public static ExtentReports extent { get; set; }
        public static ExtentHtmlReporter htmlReporter { get; set; }

        public static bool IsReportToBeGenerated = Settings.IsReport;
        public static string ApplicationName = "ActiTime";
        public static string FileName = null;


        public static void GetExtent()
        {
            try
            {
                if (IsReportToBeGenerated)
                {
                    FileName = ApplicationName + " - " + " AutomationReport -" + " " + Settings.Environment + " " + "- " + GenericHelpers.GetDateTimeWithoutSpace() + ".html";

                    Settings.ReportFullPath = GenericHelpers.GetProjectPathOfFolder("TestReports\\ExtentReports") + FileName;

                    extent = new ExtentReports();
                    htmlReporter = new ExtentHtmlReporter(Settings.ReportFullPath);
                    htmlReporter.LoadConfig(GenericHelpers.GetExecutingAssemblyProjectPathOfFolder("") + "\\extent-config.xml");

                    //htmlReporter.Config.CSS = "css-string";
                    //htmlReporter.Config.DocumentTitle = "Automation Extent Report";
                    //htmlReporter.Config.EnableTimeline = true;
                    //htmlReporter.Config.Encoding = "utf-8";
                    //htmlReporter.Config.JS = "js-string";
                    //htmlReporter.Config.ReportName = "Build 20231001";
                    //htmlReporter.Config.Theme = Theme.Standard;

                    extent.AttachReporter(htmlReporter);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception in GetExtent Method : " + e);
            }
        }

        public static ExtentTest CreateFeatureTest(string featureTitle)
        {
            try
            {
                featureTest= extent.CreateTest<Feature>(featureTitle);
                return featureTest;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in CreateFeatureTest Method : " + e);
                return null;
            }
        }

        public static ExtentTest CreateScenarioTest(string scenarioTitle)
        {
            try
            {
                scenarioTest = featureTest.CreateNode<Scenario>(scenarioTitle);
                return scenarioTest;
            }catch(Exception e)
            {
                Console.WriteLine("Exception in CreateScenarioTest Method : " + e);
                return null;
            }
        }

        

        public static void CreateStepTest(ScenarioContext scenarioContext)
        {
            try
            {
                var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();

                //PropertyInfo pInfo = typeof(ScenarioContext).GetProperty("TestStatus", BindingFlags.Instance | BindingFlags.NonPublic);
                //MethodInfo getter = pInfo.GetGetMethod(nonPublic: true);
                //object TestResult = getter.Invoke(scenarioContext, null);

                if (scenarioContext.TestError == null)
                {
                    if (stepType == "Given") nodeTest = scenarioTest.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "When") nodeTest = scenarioTest.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "And") nodeTest = scenarioTest.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "Then") nodeTest = scenarioTest.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text);
                }else if(scenarioContext.TestError != null)
                {
                    Exception ex = scenarioContext.TestError;
                    if (stepType == "Given") nodeTest = scenarioTest.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "When") nodeTest =  scenarioTest.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "And") nodeTest = scenarioTest.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "Then") nodeTest = scenarioTest.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text);
                }

                //if (TestResult.ToString() == "StepDefinitionPending")
                //{
                //    if (stepType == "Given")
                //        scenarioTest.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                //    else if (stepType == "When")
                //        scenarioTest.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                //    else if (stepType == "Then")
                //        scenarioTest.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");

                //}

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in CreateNode Method : " + e);
            }
        }


        public static void Log(LogStatus status, string message)
        {

            switch (status)
            {
                case LogStatus.INFO:
                    nodeTest.Info(message);
                    break;
                case LogStatus.PASS:
                    nodeTest.Pass(message);
                    //nodeTest.Log(Status.Pass, message);
                    break;
                case LogStatus.BOLD:
                    nodeTest.Pass(message);
                    //nodeTest.Log(Status.Pass, message);
                    break;
                case LogStatus.FAIL:
                    nodeTest.Fail(message);
                    //nodeTest.Log(Status.Fail, message);
                    break;
                case LogStatus.EXCEPTION:
                    nodeTest.Fatal(message);
                    //nodeTest.Log(Status.Fatal, message);
                    break;
                default:
                    break;

            }
        }
        public enum LogStatus
        {
            PASS,
            FAIL,
            EXCEPTION,
            BOLD,
            INFO

        }

        public static void CloseReport(IWebDriver driver)
        {
            try
            {
                if (IsReportToBeGenerated)

                    //extent.AddSystemInfo("Browser Version", ((RemoteWebDriver)driver).Capabilities.GetCapability("version").ToString());
                    extent.AddSystemInfo("Environment", Settings.Environment);
                    extent.AddSystemInfo("Computer Name", System.Environment.MachineName);
                    extent.Flush();
                    //Send_Report_In_Mail();

                
            }
            catch (Exception e)
            {
                LogHelpers.Write("ERROR :: " + e.Message, e.InnerException);
            }
        }
    }
}
