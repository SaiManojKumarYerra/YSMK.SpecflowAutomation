using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using TechTalk.SpecFlow;
using YSMK.SpecflowAutomation.Utilities;

namespace YSMK.SpecflowAutomation.Base
{
    public abstract class TestInitializeHook : Steps
    {
        public static void OpenBrowser(BrowserType browserType = BrowserType.FireFox)
        {
            switch (browserType)
            {
                case BrowserType.InternetExplorer:
                    DriverContext.Driver = new InternetExplorerDriver();
                    DriverContext.Browser = new Browser(DriverContext.Driver);
                    break;
                case BrowserType.FireFox:
                    //var binary = new FirefoxBinary(@"C:\Program Files (x86)\Mozilla Firefox\firefox.exe");
                    //var profile = new FirefoxProfile();
                    //DriverContext.Driver = new FirefoxDriver(binary, profile);
                    var fireFoxOptions = new FirefoxOptions
                    {
                        AcceptInsecureCertificates = true
                    };
                    DriverContext.Driver = new FirefoxDriver(fireFoxOptions);
                    DriverContext.Browser = new Browser(DriverContext.Driver);
                    break;
                case BrowserType.Chrome:
                    ChromeOptions chromeOptions = new ChromeOptions();
                    chromeOptions.AddExcludedArgument("enable-automation");
                    chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
                    chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
                    chromeOptions.AddUserProfilePreference("download.default_directory", GenericHelpers.GetExecutingAssemblyProjectPathOfFolder("Downloads"));
                    //chromeOptions.AddArguments("headless");
                    chromeOptions.AddArgument("--incognito");
                    DriverContext.Driver = new ChromeDriver(chromeOptions);
                    DriverContext.Browser = new Browser(DriverContext.Driver);
                    break;
                case BrowserType.WebService:
                    break;
            }
            if (DriverContext.Driver != null)
            {
                DriverContext.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                DriverContext.Driver.Manage().Window.Maximize();
            }
        }
    }
}
