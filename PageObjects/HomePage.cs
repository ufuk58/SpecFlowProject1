
using OpenQA.Selenium;
using SpecFlowProject1.Support;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlowProject1.PageObjects;

public class HomePage
{
    public IWebDriver driver;
    private IWebDriver Driver
    {
        get { return Hooks1.driver; }
    }

    private IWebElement AcceptAll => Driver.FindElement(By.XPath("//div[text()='Accept all']"));
    private IWebElement SearchInput => Driver.FindElement(By.XPath("//textarea[@title='Search']"));

    public void NavigateToLandingPage()
    {
        //string url = ConfigurationManager.AppSettings["URL"];
        string url2 = Hooks1.configSettings.Env.url;
        Driver.Navigate().GoToUrl(url2);

    }

    public void ClickAcceptAll()
    {
        AcceptAll.Click();

    }

    public void EnterTextToSearchInput()
    {
        SearchInput.Clear();
        SearchInput.SendKeys("What is the best test automation tool");
        SearchInput.SendKeys(Keys.Enter);
    }


}
