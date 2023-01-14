using OpenQA.Selenium;
using YSMK.SpecflowAutomation.Extensions;

namespace YSMK.SpecflowAutomation.Base
{
    public class Browser
    {
        private readonly IWebDriver _driver;

        public Browser(IWebDriver driver)
        {
            _driver = driver;
        }

        public BrowserType Type { get; set; }

        public void GoToUrl(string url)
        {
            DriverContext.Driver.Url = url;
            DriverContext.Driver.WaitForPageLoaded();
        }

    }

    public enum BrowserType
    {
        InternetExplorer,
        FireFox,
        Chrome,
        WebService,
        SSIS
    }
}
