using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using RazorEngine.Compilation.ImpromptuInterface.Dynamic;
using SpecFlowProject1.Support;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlowProject1.Utility;

public static class GenericHelper
{
    public static IWebDriver driver;

    public static IWebDriver Driver()
    {
        driver = Hooks1.driver;
        return driver;
    }

    public static IWebElement WaitForEnabled(this IWebElement element,int TimeSpan)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        while (watch.Elapsed.Milliseconds < TimeSpan)
        {
            if (element.Enabled)
            {
                return element;
            }
        }
        throw new ElementNotInteractableException();
    }

    public static IWebElement WaitForVisible(this IWebElement element, int TimeSpan)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        while (watch.Elapsed.Milliseconds < TimeSpan)
        {
            if (element.Displayed)
            {
                return element;
            }
        }
        throw new ElementNotVisibleException();
    }

    public static IWebElement WaitForText(this IWebElement element, int TimeSpan)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        while (watch.Elapsed.Milliseconds < TimeSpan)
        {
            if (element.Text.Length>0)
            {
                return element;
            }
        }
        throw new ElementNotVisibleException();
    }

    public static IWebElement WaitForText(this IWebElement element,string text, int TimeSpan)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        while (watch.Elapsed.Milliseconds < TimeSpan)
        {
            if (element.Text==text)
            {
                return element;
            }
        }
        throw new NoSuchElementException();
    }

    public static void SeriLogCreator(string text)
    {
        Serilog.Log.Debug(text);
    }

    public static void DoubleClick(IWebElement element)
    {
        Actions action = new Actions(Driver());
        action.DoubleClick(element).Build().Perform();
    }

    public static void ClickWithJavaScriptExecuter(IWebElement element)
    {
        IJavaScriptExecutor executer = (IJavaScriptExecutor)Driver();
        executer.ExecuteScript("arguments[0].click();", element);
    }

    public static string DirectoryPath(string directoryName)
    {
        string directoryPath = System.IO.Directory.GetParent(@"../../../").FullName
            + Path.DirectorySeparatorChar + directoryName + "\\";
        return directoryPath;
    }

    public static void VerifyPageTitle(string pageTitle)
    {
        Driver().Title.Should().Be(pageTitle);
    }

}
