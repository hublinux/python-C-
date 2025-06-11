//扩展功能实现 API测试扩展 ApiTestCase.cs
using System.Net.Http;
using System.Threading.Tasks;

namespace AutomatedTestTool
{
    public abstract class ApiTestCase : TestCase
    {
        protected HttpClient Client { get; private set; }

        public override void Setup()
        {
            Client = new HttpClient();
            // 可以配置默认请求头等
        }

        public override void Teardown()
        {
            Client?.Dispose();
        }

        protected async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await Client.GetAsync(url);
        }

        // 可以添加更多HTTP方法...
    }

    public class SampleApiTest : ApiTestCase
    {
        public override async void Test()
        {
            var response = await GetAsync("https://jsonplaceholder.typicode.com/posts/1");
            Assert.IsTrue(response.IsSuccessStatusCode, "API request failed");
            
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(content.Contains("userId"), "Response format incorrect");
        }
    }
}
