// TestFramework.cs  基础框架结构
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutomatedTestTool
{
    public enum TestStatus { Passed, Failed, Error, Skipped }

    public class TestResult
    {
        public string Name { get; set; }
        public TestStatus Status { get; set; }
        public TimeSpan Duration { get; set; }
        public string Message { get; set; }
        public Dictionary<string, object> Details { get; set; } = new Dictionary<string, object>();
    }

    public abstract class TestCase
    {
        public virtual void Setup() { }
        public virtual void Teardown() { }
        public abstract void Test();

        public TestResult Run()
        {
            var result = new TestResult
            {
                Name = this.GetType().Name,
                Status = TestStatus.Passed
            };

            var startTime = DateTime.Now;
            
            try
            {
                Setup();
                Test();
            }
            catch (AssertionException ex)
            {
                result.Status = TestStatus.Failed;
                result.Message = ex.Message;
            }
            catch (Exception ex)
            {
                result.Status = TestStatus.Error;
                result.Message = $"Unexpected error: {ex.Message}";
            }
            finally
            {
                Teardown();
                result.Duration = DateTime.Now - startTime;
            }

            return result;
        }
    }

    public class AssertionException : Exception
    {
        public AssertionException(string message) : base(message) { }
    }

    public static class Assert
    {
        public static void IsTrue(bool condition, string message = "Condition is not true")
        {
            if (!condition) throw new AssertionException(message);
        }
        
        // 可以添加更多断言方法...
    }
}
