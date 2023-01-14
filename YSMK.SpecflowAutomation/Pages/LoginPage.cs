using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;
using YSMK.SpecflowAutomation.AppHooks;
using YSMK.SpecflowAutomation.Base;
using YSMK.SpecflowAutomation.Configuration;

namespace YSMK.SpecflowAutomation.Pages
{
    public class LoginPage : BasePage
    {

        public LoginPage(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        #region Locators
        private IWebElement userName => DriverContext.Driver.FindElement(By.XPath("//input[@placeholder='Username']"));
        private IWebElement passWord => DriverContext.Driver.FindElement(By.XPath("//input[@placeholder='Password']"));
        private IWebElement loginButton => DriverContext.Driver.FindElement(By.XPath("//button[normalize-space()='Login']"));
        private IWebElement profileMenu => DriverContext.Driver.FindElement(By.XPath("//i[@class='oxd-icon bi-caret-down-fill oxd-userdropdown-icon']"));
        private IWebElement logoutButton => DriverContext.Driver.FindElement(By.XPath("//a[normalize-space()='Logout']"));
        #endregion


        public void LoginToApplication()
        {
            DriverContext.Driver.Navigate().GoToUrl(Settings.ActitimeURL);
            userName.SendKeys(Settings.ValidUserName);
            passWord.SendKeys(Settings.ValidPassword);
            loginButton.Click();
        }

        public void LogOutFromApplication()
        {
            profileMenu.Click();
            Thread.Sleep(5000);
            logoutButton.Click();
            Thread.Sleep(5000);
        }
    }
}
