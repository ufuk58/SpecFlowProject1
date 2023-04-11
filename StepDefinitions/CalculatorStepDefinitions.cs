using NUnit.Framework;
using SpecFlowProject1.Drivers;
using SpecFlowProject1.PageObjects;

namespace SpecFlowProject1.StepDefinitions;

[Binding]
public sealed class CalculatorStepDefinitions
{
    HomePage homePage = PageInitializer.homePage;

    [Author("Ufuk")]
    [Given(@"Navigate to landing page")]
    public void GivenNavigateToLandingPage()
    {
        homePage.NavigateToLandingPage();
    }

    [Author("Ufuk")]
    [Given(@"Click Accept All")]
    public void GivenClickAcceptAll()
    {
        homePage.ClickAcceptAll();
    }

    [Author("Ufuk")]
    [When(@"Enter text to search input and click search")]
    public void WhenEnterTextToSearchInputAndClickSearch()
    {
        homePage.EnterTextToSearchInput();
    }


}