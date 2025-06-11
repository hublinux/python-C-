//UI测试扩展 (使用Selenium) UiTestCase.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutomatedTestTool
{
    public abstract class UiTestCase : TestCase
    {
        protected IWebDriver Driver { get; private set; }

        public override void Setup()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
        }

        public override void Teardown()
        {
            Driver?.Quit();
        }

        protected void NavigateTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }
    }

    public class SampleUiTest : UiTestCase
    {
        public override void Test()
        {
            NavigateTo("https://www.example.com");
            Assert.IsTrue(Driver.Title.Contains("Example"), "Page title incorrect");
        }
    }
}
