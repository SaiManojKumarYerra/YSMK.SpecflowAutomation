using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Infrastructure;
using YSMK.SpecflowAutomation.Base;
using YSMK.SpecflowAutomation.Configuration;
using YSMK.SpecflowAutomation.Pages;
using YSMK.SpecflowAutomation.Utilities;
using static YSMK.SpecflowAutomation.Utilities.ReportHelpers;

namespace YSMK.SpecflowAutomation.StepDefinitions
{
    [Binding]
    public class ActiTimeLoginSteps : BaseStep
    {
        public LoginPage loginPage;
        public HomePage homePage;
        public ActiTimeLoginSteps(ScenarioContext scenarioContext, ISpecFlowOutputHelper specFlowOutputHelper):base(scenarioContext,specFlowOutputHelper)
        {
            loginPage = new LoginPage(_scenarioContext);
            homePage = new HomePage(_scenarioContext);
        }

        [Given(@"User is in Login Page")]
        public void GivenUserIsInLoginPage()
        {
           
            loginPage.LoginToApplication();
            ReportHelpers.Log(LogStatus.PASS, "Login to Application is successful");
            WriteToSpecFlowOutputHelper("jklfd");
        }
        
        [Given(@"User enters UserId and Password")]
        public void GivenUserEntersUserIdAndPassword()
        {
            Thread.Sleep(5000);
            ReportHelpers.Log(LogStatus.PASS, "Waiting in Application is successful");
        }
        [Then(@"User logout from the Application")]
        public void ThenUserLogoutFromTheApplication()
        {
            loginPage.LogOutFromApplication();
            ReportHelpers.Log(LogStatus.PASS, "Logout from Application is successful");
        }

        [Given(@"User Clicks on Admin Page")]
        public void GivenUserClicksOnAdminPage()
        {
            homePage.NavigateToAdminModule();
            ReportHelpers.Log(LogStatus.PASS, "Navigation to Admin Page is successful");
        }

        [Then(@"Verify user is in Admin Page")]
        public void ThenVerifyUserIsInAdminPage()
        {
            homePage.VerifyAdminModule();
            ReportHelpers.Log(LogStatus.PASS, "Verifying Admin Page is successful");
            Thread.Sleep(5000);
        }

    }
}
