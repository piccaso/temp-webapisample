using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AwaitAndGetResult;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.CSharp;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using ApiClient = Client.Client;
using NUnit.Framework;
using WebApiSample.Infrastructure;

namespace IntegrationTests
{
    [TestFixture]
    public class WebApiTests
    {
        private HttpClient _client;
        private ApiClient _api;

        [SetUp]
        public void SetUp()
        {
            var sw = Stopwatch.StartNew();
            var host = new WebHostBuilder().UseStartup<Startup>();
            var server = new TestServer(host);
            _client = server.CreateClient();
            _api = new ApiClient(server.BaseAddress.ToString(), _client);
            sw.Stop();
            TestContext.WriteLine($"SetUp Time: {sw.Elapsed}");
        }

        [Test]
        public async Task HasSwaggerJson()
        {
            var response = await _client.GetAsync("/swagger/v1/swagger.json");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            
            await File.WriteAllTextAsync("swagger.json", json, Encoding.UTF8);
            TestContext.AddTestAttachment("swagger.json", "Swagger Spec");
        }

        [Test]
        public async Task CanBuildClient() {
            var response = await _client.GetAsync("/swagger/v1/swagger.json");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            var document = await SwaggerDocument.FromJsonAsync(json);
            var settings = new SwaggerToCSharpClientGeneratorSettings {
                InjectHttpClient = true,
                GenerateSyncMethods = true,
                CSharpGeneratorSettings = {
                    Namespace = "Client",
                }
            };
            var generator = new SwaggerToCSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();

            Directory.CreateDirectory("cbc");

            File.WriteAllText("cbc/Client.cs", code);
            TestContext.WriteLine(code);
            TestContext.AddTestAttachment("cbc/Client.cs","generated rest api client");

            // TODO: maybe compile? 
            // https://msdn.microsoft.com/en-us/magazine/mt808499.aspx
            // https://github.com/RSuter/NSwag/wiki/SwaggerToCSharpClientGenerator
        }

        [Test]
        public async Task Increment()
        {
            var id = Guid.NewGuid();
            const long val = 222;
            var sw = new Stopwatch();

            sw.Start();
            var setResp = await _api.ApiCounterByGuidSetByValuePostAsync(id, val);
            var incResp = await _api.ApiCounterByGuidIncrementByByPostAsync(id, 15);
            var getResp = await _api.ApiCounterByGuidGetGetAsync(id);
            sw.Stop();

            Assert.AreEqual(val, setResp);
            Assert.AreEqual(val + 15, incResp);
            Assert.AreEqual(incResp, getResp);
            TestContext.WriteLine($"Api Operations: {sw.Elapsed}");
        }

        [Test]
        public void Threads()
        {
            var sync = new object();
            Parallel.For(0, 100, new ParallelOptions {MaxDegreeOfParallelism = Environment.ProcessorCount}, (i, state) => 
            {
                // Arrange
                var uuid = Guid.NewGuid();
                var value = i + 100L;
                var sw = new Stopwatch();

                // Act
                sw.Start();
                var setResult = _api.ApiCounterByGuidSetByValuePostAsync(uuid, value).AwaitAndGetResult();
                var addResult = _api.ApiCounterByGuidIncrementByByPostAsync(uuid, 500).AwaitAndGetResult();
                var getResult = _api.ApiCounterByGuidGetGetAsync(uuid).AwaitAndGetResult();
                sw.Stop();


                // Assert
                Assert.AreEqual(value, setResult);
                Assert.AreEqual(value + 500, getResult);
                Assert.AreEqual(addResult, getResult);
                lock (sync)
                {
                    TestContext.WriteLine($"{i,10}: {sw.Elapsed}"); 
                }
            });
        }
    }
}
