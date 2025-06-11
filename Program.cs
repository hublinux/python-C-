//使用示例 Program.cs
using System;

namespace AutomatedTestTool
{
    class Program
    {
        static void Main(string[] args)
        {
            // 创建测试用例
            var test1 = new SampleTest1();
            var test2 = new SampleTest2();
            
            // 创建测试套件
            var runner = new TestRunner();
            var suite = runner.CreateSuite("Sample Test Suite");
            suite.AddTestCase(test1);
            suite.AddTestCase(test2);
            
            // 运行测试
            suite.Run();
            
            // 生成报告
            var reporter = new HtmlReporter();
            var reportPath = reporter.GenerateReport(suite);
            
            Console.WriteLine($"Test completed! Report generated at: {reportPath}");
        }
    }

    // 示例测试用例1
    public class SampleTest1 : TestCase
    {
        public override void Test()
        {
            Assert.IsTrue(1 + 1 == 2, "Math is broken!");
        }
    }

    // 示例测试用例2
    public class SampleTest2 : TestCase
    {
        public override void Test()
        {
            Assert.IsTrue(1 + 1 == 3, "This test is supposed to fail");
        }
    }
}
