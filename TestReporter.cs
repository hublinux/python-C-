//报告生成器 TestReporter.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutomatedTestTool
{
    public interface ITestReporter
    {
        string GenerateReport(TestSuite suite, string outputPath = null);
    }

    public class HtmlReporter : ITestReporter
    {
        public string GenerateReport(TestSuite suite, string outputPath = null)
        {
            var stats = suite.GetStats();
            var totalTests = suite.Results.Count;
            var passRate = (double)stats.GetValueOrDefault(TestStatus.Passed, 0) / totalTests * 100;

            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<title>Test Report</title>");
            html.AppendLine("<style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine(".summary { background: #f5f5f5; padding: 15px; border-radius: 5px; margin-bottom: 20px; }");
            html.AppendLine(".passed { color: green; }");
            html.AppendLine(".failed { color: red; }");
            html.AppendLine(".error { color: orange; }");
            html.AppendLine(".skipped { color: gray; }");
            html.AppendLine("table { width: 100%; border-collapse: collapse; }");
            html.AppendLine("th, td { padding: 8px; text-align: left; border-bottom: 1px solid #ddd; }");
            html.AppendLine("tr:hover { background-color: #f5f5f5; }");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendLine($"<h1>Test Report: {suite.Name}</h1>");
            
            // 摘要部分
            html.AppendLine("<div class='summary'>");
            html.AppendLine("<h2>Summary</h2>");
            html.AppendLine($"<p>Total Tests: {totalTests}</p>");
            html.AppendLine($"<p class='passed'>Passed: {stats.GetValueOrDefault(TestStatus.Passed, 0)}</p>");
            html.AppendLine($"<p class='failed'>Failed: {stats.GetValueOrDefault(TestStatus.Failed, 0)}</p>");
            html.AppendLine($"<p class='error'>Errors: {stats.GetValueOrDefault(TestStatus.Error, 0)}</p>");
            html.AppendLine($"<p class='skipped'>Skipped: {stats.GetValueOrDefault(TestStatus.Skipped, 0)}</p>");
            html.AppendLine($"<p>Pass Rate: {passRate:F2}%</p>");
            html.AppendLine("</div>");
            
            // 详细结果
            html.AppendLine("<h2>Test Results</h2>");
            html.AppendLine("<table>");
            html.AppendLine("<tr><th>Test Name</th><th>Status</th><th>Duration</th><th>Message</th></tr>");
            
            foreach (var result in suite.Results)
            {
                html.AppendLine($"<tr class='{result.Status.ToString().ToLower()}'>");
                html.AppendLine($"<td>{result.Name}</td>");
                html.AppendLine($"<td>{result.Status}</td>");
                html.AppendLine($"<td>{result.Duration.TotalSeconds:F3}s</td>");
                html.AppendLine($"<td>{result.Message ?? "-"}</td>");
                html.AppendLine("</tr>");
            }
            
            html.AppendLine("</table>");
            html.AppendLine("<footer style='margin-top: 30px; text-align: center; color: #666;'>");
            html.AppendLine($"Generated on {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            html.AppendLine("</footer>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");

            if (string.IsNullOrEmpty(outputPath))
            {
                outputPath = Path.Combine("Reports", $"TestReport_{DateTime.Now:yyyyMMdd_HHmmss}.html");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            File.WriteAllText(outputPath, html.ToString());

            return outputPath;
        }
    }
}
