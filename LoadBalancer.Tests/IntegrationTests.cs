using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json;
using System.Text;
using Sc = System.Net.HttpStatusCode;

namespace LoadBalancer.Tests
{

    public class IntegrationTests
    {

        private readonly HttpClient _client;

        public IntegrationTests()
        {
            _client = new HttpClient();
        }

        private const string Url = "http://localhost:3333/api/increment/";

        private readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
     
        [Fact]
        public async Task GetIsSuccess()
        {
            var response = await Get("foo");

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task PostIsSuccess()
        {
            var data = new Lib.KeyValuePair("buzz", 2);

            var response = await Post(data);

            Assert.True(response.IsSuccessStatusCode);

            await Task.Delay(1000);

        }

        [Fact]
        public async Task PostStoresDataWithin10Seconds()
        {
            //get current value 
            var oldResponse = await Get("foo");

            var oldData = await DeserializeToKvp(oldResponse);

            //post increment 10
            _ = await Post(new Lib.KeyValuePair("foo", 20));

            //wait 10 seconds
            await Task.Delay(10000);

            var newResponse = await Get("foo");

            var newData = await DeserializeToKvp(newResponse);

            var shouldBe = oldData.Value + 20;

            Assert.Equal(newData.Value, shouldBe);
        }

        [Fact]
        public async Task PostStoresDataWithin10SecondsAtAllowedLoad()
        {
            //get current value 
            var oldResponse = await Get("foo");

            var oldData = await DeserializeToKvp(oldResponse);

            //loop and capture amount
            int acc = 0, val = 20;

            for (int i = 0; i < 10; i++)
            {
                _ = await Post(new Lib.KeyValuePair("foo", val));

                acc += val;

                await Task.Delay(1000);


            }

            //get new value
            var newResponse = await Get("foo");

            var newData = await DeserializeToKvp(newResponse);

            var shouldBe = oldData.Value + acc;


            Assert.Equal(newData.Value, shouldBe);
        }



        [Fact]
        public async Task RateLimiterLimitsPostsToOnePerSecond()
        {
            _ = await Post(new Lib.KeyValuePair("foo", 3));
            var second = await Post(new Lib.KeyValuePair("foo", 4));

            await Task.Delay(1500);

            Assert.Equal(Sc.TooManyRequests, second.StatusCode);

            await Task.Delay(1000);
        }

        [Fact]
        public async Task RateLimiterAllowsPostsAtLimit()
        {
            _ = await Post(new Lib.KeyValuePair("foo", 3));

            await Task.Delay(1000);

            var second = await Post(new Lib.KeyValuePair("foo", 4));

            Assert.True(second.IsSuccessStatusCode);

            await Task.Delay(1000);
        }


        private async Task<HttpResponseMessage> Get(string key)
            => await _client.GetAsync(Path.Combine(Url, key));

        private async Task<HttpResponseMessage> Post(Lib.KeyValuePair kvp)
        {
            var content = new StringContent(JsonSerializer.Serialize(kvp), Encoding.UTF8, "application/json");
            
            return await _client.PostAsync(Url, content);
        }

        private async Task<Lib.KeyValuePair> DeserializeToKvp(HttpResponseMessage msg) =>
            JsonSerializer.Deserialize<Lib.KeyValuePair>(await msg.Content.ReadAsStringAsync(), JsonOptions);


    }
            
}
