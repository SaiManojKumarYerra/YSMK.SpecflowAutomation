using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using YSMK.SpecflowAutomation.Base;

namespace YSMK.SpecflowAutomation.Pages
{
    public class HomePage : BasePage
    {
        public HomePage(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }
        private IWebElement adminMenu => DriverContext.Driver.FindElement(By.XPath("//span[text()='Admin']"));
        private IWebElement adminPage => DriverContext.Driver.FindElement(By.XPath("//h6[text()='Admin']"));
    
        public void NavigateToAdminModule()
        {
            adminMenu.Click();
        }

        public void VerifyAdminModule()
        {
            Assert.IsTrue(adminPage.Displayed, " Admin Page is not displayed");
        }
    }
}
