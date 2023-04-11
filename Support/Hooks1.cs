using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SpecFlowProject1.Drivers;
using SpecFlowProject1.Utility;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace SpecFlowProject1.Support;

[Binding]
public sealed class Hooks1
{
    public static IWebDriver driver;

    private static ScenarioContext scenarioContext;
    private static FeatureContext featureContext;
    private static ExtentReports extentReports;
    private static ExtentHtmlReporter extentHtmlReporter;
    private static ExtentTest feature;
    private static ExtentTest scenario;
    private static ScreenCapture screenCapture;
    public static Screenshot screenshot;
    public static string workingDirectory = Environment.CurrentDirectory;
    public static string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
    public static string Report_Direct = projectDirectory + "/Report/";
    public static string logs = projectDirectory + "/Logs/";
    public static string screenshotString = System.IO.Directory.GetParent(@"../../../").FullName
        + Path.DirectorySeparatorChar + "/Screenshots/";
    public static ConfigSettings configSettings;
    public static string configSettingPath = System.IO.Directory.GetParent(@"../../../").FullName
        + Path.DirectorySeparatorChar + "/specflow.json";
    public static string downloads = GenericHelper.DirectoryPath("Downloads");

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        configSettings = new ConfigSettings();

        ConfigurationBuilder builder = new ConfigurationBuilder();
        builder.AddJsonFile(configSettingPath);
        IConfiguration configuration = builder.Build();
        configuration.Bind(configSettings);

        Directory.GetFiles(downloads).ToList().ForEach(File.Delete);
        Directory.GetFiles(screenshotString).ToList().ForEach(File.Delete);
        Directory.GetFiles(Report_Direct).ToList().ForEach(File.Delete);
        Directory.GetFiles(logs).ToList().ForEach(File.Delete);

        extentHtmlReporter = new ExtentHtmlReporter(Report_Direct);
        extentReports = new ExtentReports();
        extentReports.AddSystemInfo("Browser Version", "chrome");
        extentReports.AnalysisStrategy = AnalysisStrategy.BDD;
        extentHtmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
        extentReports.AttachReporter(extentHtmlReporter);

        LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch(LogEventLevel.Debug);
        Serilog.Log.Logger = new LoggerConfiguration().MinimumLevel
            .ControlledBy(levelSwitch).WriteTo.File(logs + "/Logs", outputTemplate:
            "{TimeStamp:yyyy-MM-dd HH:mm:ss.fff} | {Lev" +
            "el:u3} | {Message}{NewLine} ", rollingInterval: RollingInterval.Day).CreateLogger();
    }

    [BeforeFeature]
    public static void BeforeFeature(FeatureContext featurecontext)
    {
        if(featurecontext!= null)
        {
            featureContext = featurecontext;
            feature = extentReports.CreateTest<Feature>(featurecontext.FeatureInfo.Title,
                featurecontext.FeatureInfo.Description);
            Serilog.Log.Information("Selecting feature {0} file to run", featurecontext.FeatureInfo.Title);
        }
        
    }

    [BeforeScenario]
    public void BeforeScenario(ScenarioContext scenariocontext)
    {
        Directory.GetFiles(downloads).ToList().ForEach(File.Delete);
        driver =Driver.setUp();
        if (scenariocontext != null)
        {
            scenarioContext = scenariocontext;
            scenario = feature.CreateNode<Scenario>(scenariocontext.ScenarioInfo.Title,
                scenariocontext.ScenarioInfo.Description);
            Serilog.Log.Information("Selecting scenario {0} file to run", scenariocontext.ScenarioInfo.Title);
        }
        

    }

    [AfterStep]
    public void AfterStep()
    {
        ScenarioBlock scenarioBlock = scenarioContext.CurrentScenarioBlock;
        Console.WriteLine(scenarioBlock);
        string[] variableName = scenarioContext.ScenarioInfo.ScenarioAndFeatureTags;
        Console.WriteLine(variableName[0]);

        switch (scenarioBlock)
        {
            case ScenarioBlock.Given:
                CreateNode<Given>();
                break;

            case ScenarioBlock.When:
                CreateNode<When>();
                break;

            case ScenarioBlock.Then:
                CreateNode<Then>();
                break;

            default:
                CreateNode<And>();
                break;

        }

    }

    [AfterScenario]
    public void AfterScenario()
    {
        Driver.CloseDriver();
    }

    [AfterTestRun]
    public static  void AfterTestRun()
    {
        extentReports.Flush();
    }


    public static void CreateNode<T>() where T : IGherkinFormatterModel
    {
        string[] variableName = scenarioContext.ScenarioInfo.ScenarioAndFeatureTags;
        if (scenarioContext.TestError != null)
        {

            string name = scenarioContext.StepContext.StepInfo.Text;
            name = Regex.Replace(name, @"[^0-9a-zA-Z\._]", "");
            name = screenshotString + name + ".jpeg";
            screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(name, ScreenshotImageFormat.Jpeg);
            scenario.CreateNode<T>(scenarioContext.StepContext.StepInfo.Text)
                .Fail(scenarioContext.TestError.Message)
                .AddScreenCaptureFromPath(name);
            scenario.AssignCategory(variableName[0]);
        }
        else
        {
            string name = scenarioContext.StepContext.StepInfo.Text;
            name = Regex.Replace(name, @"[^0-9a-zA-Z\._]", "");
            name = screenshotString + name + ".jpeg";
            scenario.CreateNode<T>(scenarioContext.StepContext.StepInfo.Text.ToString().Replace("Given", ""))
                .Pass("Passed").AddScreenCaptureFromPath(name);
            scenario.AssignCategory(variableName[0]);
        }
    }

}