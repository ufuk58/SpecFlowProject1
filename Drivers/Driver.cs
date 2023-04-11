using OpenQA.Selenium;
using WebDriverManager.DriverConfigs.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using SpecFlowProject1.Support;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace SpecFlowProject1.Drivers;

public class Driver
{

    public static IWebDriver driver;

    public static IWebDriver setUp()
    {
        string browser = Environment.GetEnvironmentVariable("BROWSER") ?? "CHROME";

        switch (browser)
        {
            case "CHROME":
                new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
                ChromeOptions options = new ChromeOptions();
                options.AddUserProfilePreference("download.default_directory",Hooks1.downloads);
                options.AddUserProfilePreference("download.prompt_for_download", false);
                options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
                driver = new ChromeDriver(options);
                break;

            case "FIREFOX":
                new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
                driver = new FirefoxDriver();
                break;

            case "IE":
                new WebDriverManager.DriverManager().SetUpDriver(new InternetExplorerConfig());
                driver = new InternetExplorerDriver();
                break;

            default:
                throw new ArgumentException($"Browser not yet implemented:{browser}");
        }
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        driver.Manage().Window.Maximize();
        PageInitializer.Initialize();
        return driver;
    }
    
    public static void CloseDriver()
    {
        if (driver != null)
        {
            driver.Dispose();
        }
    }
}
