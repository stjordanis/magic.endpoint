/*
 * Magic, Copyright(c) Thomas Hansen 2019, thomas@gaiasoul.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json.Linq;
using magic.endpoint.contracts;

namespace magic.endpoint.tests
{
    public class AuthTests
    {
        [Fact]
        public async Task ExecuteSimpleGet()
        {
            var svc = Common.Initialize();
            var executor = svc.GetService(typeof(IExecutorAsync)) as IExecutorAsync;
            var result = await executor.ExecuteGetAsync("/foo-1", null);
            Assert.Equal(200, result.Result);
            Assert.Empty(result.Headers);
            var j = result.Content as JObject;
            Assert.NotNull(j);
            Assert.Equal("hello world", j["result"].Value<string>());
        }

        [Fact]
        public async Task GetEcho()
        {
            var svc = Common.Initialize();
            var executor = svc.GetService(typeof(IExecutorAsync)) as IExecutorAsync;

            // Notice, GET will convert its arguments.
            var input = new JObject
            {
                ["input1"] = "foo",
                ["input2"] = "5",
                ["input3"] = "true",
            };
            var result = await executor.ExecuteGetAsync("/echo", input);
            Assert.Equal(200, result.Result);
            Assert.Empty(result.Headers);
            var j = result.Content as JObject;
            Assert.NotNull(j);
            Assert.Equal("foo", j["input1"].Value<string>());
            Assert.Equal(5, j["input2"].Value<int>());
            Assert.True(j["input3"].Value<bool>());
        }

        [Fact]
        public async Task GetStatusResponse()
        {
            var svc = Common.Initialize();
            var executor = svc.GetService(typeof(IExecutorAsync)) as IExecutorAsync;
            var result = await executor.ExecuteGetAsync("/status", null);
            Assert.Equal(201, result.Result);
        }

        [Fact]
        public async Task GetHttpHeader()
        {
            var svc = Common.Initialize();
            var executor = svc.GetService(typeof(IExecutorAsync)) as IExecutorAsync;
            var result = await executor.ExecuteGetAsync("/header", null);
            Assert.Single(result.Headers);
            Assert.Equal("bar", result.Headers["foo"]);
        }

        [Fact]
        public async Task PostEcho()
        {
            var svc = Common.Initialize();
            var executor = svc.GetService(typeof(IExecutorAsync)) as IExecutorAsync;
            var input = new JObject
            {
                ["input1"] = "foo",
                ["input2"] = 5,
                ["input3"] = true,
            };
            var result = await executor.ExecutePostAsync("/echo", input);
            Assert.Equal(200, result.Result);
            Assert.Empty(result.Headers);
            var j = result.Content as JObject;
            Assert.NotNull(j);
            Assert.Equal("foo", j["input1"].Value<string>());
            Assert.Equal(5, j["input2"].Value<int>());
            Assert.True(j["input3"].Value<bool>());
        }
    }
}
