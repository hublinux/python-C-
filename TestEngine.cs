//测试执行引擎 TestEngine.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AutomatedTestTool
{
    public class TestSuite
    {
        public string Name { get; }
        public List<TestCase> TestCases { get; } = new List<TestCase>();
        public List<TestResult> Results { get; private set; }

        public TestSuite(string name)
        {
            Name = name;
        }

        public void AddTestCase(TestCase testCase)
        {
            TestCases.Add(testCase);
        }

        public List<TestResult> Run()
        {
            Results = new List<TestResult>();
            foreach (var testCase in TestCases)
            {
                Results.Add(testCase.Run());
            }
            return Results;
        }

        public Dictionary<TestStatus, int> GetStats()
        {
            return Results
                .GroupBy(r => r.Status)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }

    public class TestRunner
    {
        public Dictionary<string, TestSuite> Suites { get; } = new Dictionary<string, TestSuite>();

        public TestSuite CreateSuite(string name)
        {
            if (Suites.ContainsKey(name))
                throw new ArgumentException($"Test suite '{name}' already exists");

            var suite = new TestSuite(name);
            Suites[name] = suite;
            return suite;
        }

        public TestSuite DiscoverTests(string assemblyPath)
        {
            var suite = new TestSuite("DiscoveredTests");
            var assembly = Assembly.LoadFrom(assemblyPath);

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(TestCase)) && !type.IsAbstract)
                {
                    var testCase = (TestCase)Activator.CreateInstance(type);
                    suite.AddTestCase(testCase);
                }
            }

            return suite;
        }
    }
}
