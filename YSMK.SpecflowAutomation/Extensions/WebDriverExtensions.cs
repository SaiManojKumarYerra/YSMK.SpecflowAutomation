using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Diagnostics;
using YSMK.SpecflowAutomation.Base;
using YSMK.SpecflowAutomation.Utilities;

namespace YSMK.SpecflowAutomation.Extensions
{
    public static class WebDriverExtensions
    {
        public static void WaitForPageLoaded(this IWebDriver driver)
        {
            driver.WaitForCondition(dri =>
            {
                string state = ((IJavaScriptExecutor)dri).ExecuteScript("return document.readyState").ToString();
                return state == "complete";
            }, 10);
        }

        public static void WaitForCondition<T>(this T obj, Func<T, bool> condition, int timeOut)
        {
            Func<T, bool> execute =
                (arg) =>
                {
                    try
                    {
                        return condition(arg);
                    }
                    catch (Exception e)
                    {
                        LogHelpers.Write(e.Message, e.InnerException);
                        return false;
                    }
                };

            var stopWatch = Stopwatch.StartNew();
            while (stopWatch.ElapsedMilliseconds < timeOut)
            {
                if (execute(obj))
                {
                    break;
                }
            }
        }
        public static IWebElement FindById(this RemoteWebDriver remoteWebDriver, string element)
        {
            try
            {
                if (remoteWebDriver.FindElementById(element).IsElementDisplayed())
                {
                    return remoteWebDriver.FindElementById(element);
                }
            }
            catch (Exception e)
            {
                LogHelpers.Write(e.Message, e.InnerException);
                throw new ElementNotVisibleException($"Element not found : {element}");
            }
            return null;
        }

        public static IWebElement FindByXpath(this RemoteWebDriver remoteWebDriver, string element)
        {
            try
            {
                if (remoteWebDriver.FindElementByXPath(element).IsElementDisplayed())
                {
                    return remoteWebDriver.FindElementByXPath(element);
                }
            }
            catch (Exception e)
            {
                LogHelpers.Write(e.Message, e.InnerException);
                throw new ElementNotVisibleException($"Element not found : {element}");
            }
            return null;
        }

        public static IWebElement FindByCss(this RemoteWebDriver remoteWebDriver, string element)
        {
            try
            {
                if (remoteWebDriver.FindElementByCssSelector(element).IsElementDisplayed())
                {
                    return remoteWebDriver.FindElementByCssSelector(element);
                }
            }
            catch (Exception e)
            {
                LogHelpers.Write(e.Message, e.InnerException);
                throw new ElementNotVisibleException($"Element not found : {element}");
            }
            return null;
        }

        public static IWebElement FindByLinkText(this RemoteWebDriver remoteWebDriver, string element)
        {
            try
            {
                if (remoteWebDriver.FindElementByLinkText(element).IsElementDisplayed())
                {
                    return remoteWebDriver.FindElementByLinkText(element);
                }
            }
            catch (Exception e)
            {
                LogHelpers.Write(e.Message, e.InnerException);
                throw new ElementNotVisibleException($"Element not found : {element}");
            }
            return null;
        }

        public static object ExecuteJs(this IWebDriver driver, string script)
        {
            return ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript(script);
        }

    }
}
