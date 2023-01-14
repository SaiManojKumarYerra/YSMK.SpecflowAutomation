using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YSMK.SpecflowAutomation.Base;
using YSMK.SpecflowAutomation.Configuration;
using YSMK.SpecflowAutomation.Extensions;
using YSMK.SpecflowAutomation.Utilities;

namespace YSMK.SpecflowAutomation.Extensions
{
    public static class WebElementExtensions
    {
        public static string GetSelectedDropDown(this IWebElement element)
        {
            SelectElement ddl = new SelectElement(element);
            return ddl.AllSelectedOptions.First().ToString();
        }
        public static IList<IWebElement> GetSelectedListOptions(this IWebElement element)
        {
            SelectElement ddl = new SelectElement(element);
            return ddl.AllSelectedOptions;
        }
        public static string GetLinkText(this IWebElement element)
        {
            return element.Text;
        }
        public static void SelectDropDownList(this IWebElement element, string value)
        {
            SelectElement ddl = new SelectElement(element);
            ddl.SelectByText(value);
        }
        public static void Hover(this IWebElement element)
        {
            Actions actions = new Actions(DriverContext.Driver);
            actions.MoveToElement(element).Perform();
        }
        public static bool IsElementDisplayed(this IWebElement element)
        {
            try
            {
                bool ele = element.Displayed;
                return true;
            }
            catch (Exception ex)
            {
                LogHelpers.Write(ex.Message, ex.InnerException);
                return false;
            }
        }
        public static void WaitForElementtoExist<T>(this T obj, Func<T, IWebElement> condition, int timeOut)
        {
            IWebElement execute(T arg)
            {
                try
                {
                    return condition(arg);
                }
                catch (Exception e)
                {
                    LogHelpers.Write(e.Message, e.InnerException);
                    return null;
                }
            }

            var stopWatch = Stopwatch.StartNew();
            while (stopWatch.ElapsedMilliseconds < timeOut)
            {
                if (execute(obj) != null)
                {
                    break;
                }
            }
        }
        public static void WaitForElementToBecomeVisibleWithinTimeout(this IWebElement element, int timeout)
        {
            new WebDriverWait(DriverContext.Driver, TimeSpan.FromSeconds(timeout)).Until(ElementIsVisible(element));
        }
        private static Func<IWebDriver, bool> ElementIsVisible(IWebElement element)
        {
            return driver =>
            {
                try
                {
                    return element.Displayed;
                }
                catch (Exception)
                {
                    // If element is null, stale or if it cannot be located
                    return false;
                }
            };
        }
        public static void WaitForElementtobeDisplayed(this IWebElement element)
        {
            WebDriverWait wait = new WebDriverWait(DriverContext.Driver, TimeSpan.FromMinutes(Settings.ImplicitTimeOut));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until<bool>(driver =>
            {
                try
                {
                    return element.Displayed;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
        public static void ClickUsingJavaScript(this IWebElement _element)
        {
            try
            {
                IJavaScriptExecutor jsc = (IJavaScriptExecutor)DriverContext.Driver;
                DriverContext.Driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(1);
                jsc.ExecuteAsyncScript("arguments[0].click()", _element);
                DriverContext.Driver.WaitForPageLoaded();
            }
            catch (Exception e)
            {
                LogHelpers.Write(e.Message, e.InnerException);
            }
        }
        public static void Scrolldown()
        {
            IJavaScriptExecutor jsx = (IJavaScriptExecutor)DriverContext.Driver;
            jsx.ExecuteScript("window.scrollBy(0,650)", "");
        }
        public static T ClickUsingAction<T>(this T webElement) where T : IWebElement
        {
            new Actions(DriverContext.Driver)
                   .MoveToElement(webElement)
                   .Click()
                   .Build()
                   .Perform();
            return webElement;
        }
        public static T ClickUsingJavascript<T>(this T webElement) where T : IWebElement
        {
            ((IJavaScriptExecutor)DriverContext.Driver).ExecuteScript("document.elementFromPoint(" + webElement.Location.X + "," + webElement.Location.Y + ").click();");
            return webElement;
        }
        public static IWebElement DoubleClick(this IWebElement webElement)
        {
            new Actions(DriverContext.Driver)
                .DoubleClick(webElement)
                .Build()
                .Perform();
            return webElement;
        }
        public static DateTime GetDateValue(string dateValue)
        {
            return DateTime.Parse(dateValue.Substring(dateValue.Length - 10, 10));
        }
        public static DateTime GetDateValueFromDisplayField(this IWebElement webElement)
        {
            string dateString = webElement.HTMLContent();
            return GetDateValue(dateString);
        }
        public static DateTime GetDateValueFromTextField(this IWebElement webElement)
        {
            string dateString = webElement.GetAttribute("value");
            return GetDateValue(dateString);
        }

        /// <summary>
        /// Gets the value attribute from the webelement
        /// </summary>
        /// <param name="webElement">The web element.</param>
        /// <returns></returns>
        public static string GetValue(this IWebElement webElement)
        {
            if (webElement.GetAttribute("aria-valuenow") != null)
            {
                return webElement.GetAttribute("aria-valuenow");
            }
            return webElement.GetAttribute("value");
        }
        public static bool HasClass(this IWebElement webElement, string className)
        {
            return webElement.GetAttribute("class")
                .Contains(className);
        }
        public static bool HasHtmlContent(this IWebElement webElement)
        {
            var contents = webElement.GetAttribute("innerHTML");
            return contents != null && contents.Trim().Length > 0;
        }
        public static string HTMLContent(this IWebElement webElement)
        {
            return webElement.GetAttribute("innerHTML");
        }
        public static bool IsElementPresent(this IWebElement webElement, By findElementBy)
        {
            try
            {
                webElement.FindElement(findElementBy);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public static bool IsEnabled(this IWebElement webElement)
        {
            var classAttribute = webElement.GetAttribute("class");
            var readOnlyAttribute = webElement.GetAttribute("readonly");
            var disabledAttribute = webElement.GetAttribute("disabled");
            return !classAttribute
                .Contains("x-item-disabled")
                && readOnlyAttribute == null
                && disabledAttribute == null;
        }
        public static bool IsRequired(this IWebElement webElement)
        {
            var inputOfElement = webElement;
            if (webElement.TagName != "input")
            {
                inputOfElement = webElement.FindElement(By.XPath(".//input"));
            }
            return webElement.GetAttribute("class")
                .Split(' ')
                .ToList()
                .Contains("x-form-required-field") ||
                inputOfElement.GetAttribute("class")
                .Split(' ')
                .ToList()
                .Contains("x-form-required-field");
        }
        public static IWebElement MoveTo(this IWebElement webElement)
        {
            new Actions(DriverContext.Driver)
                 .MoveToElement(webElement)
                 .Build()
                 .Perform();
            return webElement;
        }
        public static T ScrollIntoView<T>(this T webElement) where T : IWebElement
        {
            var id = webElement.GetAttribute("id");
            DriverContext.Driver.ExecuteJs("document.getElementById('" + id + "').scrollIntoView(true);");
            webElement.Wait(30).Until(x => webElement.Displayed);
            return webElement;
        }
        public static T SetFieldValue<T>(this T webElement, string fieldName, string text) where T : IWebElement
        {
            var field = webElement.FindElement(By.Name(fieldName));
            field.Clear();
            field.SendKeys(text);
            field.SendKeys(Keys.Tab);
            return webElement;
        }
        public static IWebElement Type(this IWebElement webElement, string text)
        {
            webElement.Clear();
            webElement.SendKeys(text);
            webElement.SendKeys(Keys.Tab);
            return webElement;
        }
        public static void UntilNotDisplayed(this DefaultWait<IWebDriver> webDriverWait, By selector)
        {
            webDriverWait.Until(driver =>
            {
                var elements = driver.FindElements(selector);
                if (!elements.Any())
                    return true;
                return elements.All(NotDisplayedOrAlreadyGoneFromDOMTree);
            });
        }
        public static WebDriverWait Wait(this IWebDriver driver, int seconds)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        }
        public static WebDriverWait Wait(this IWebElement element, int timeout)
        {
            return DriverContext.Driver.Wait(timeout);
        }
        private static bool NotDisplayedOrAlreadyGoneFromDOMTree(IWebElement el)
        {
            try
            {
                return !el.Displayed;
            }
            catch (StaleElementReferenceException notExistingAnymoreEx)
            {
                LogHelpers.Write(notExistingAnymoreEx.Message, notExistingAnymoreEx.InnerException);
                return true;
            }
        }
    }
}
