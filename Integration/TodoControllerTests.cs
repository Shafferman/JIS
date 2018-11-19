using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JISTesting;
using JISTesting.Models;
using Newtonsoft.Json;
using Xunit;

namespace Integration
{
    public class TodoControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;

        public TodoControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            client = factory.CreateClient();
        }

        [Fact]
        public async Task WhenGettingAllItemsThenSuccessIsReturned()
        {
            var response = await client.GetAsync("api/todo");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GivenIdWhenIdIsFoundThenSuccessIsReturned()
        {
            var response = await client.GetAsync("api/todo/1");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GivenIdWhenIdIsNotFoundThen404IsReturned()
        {
            var response = await client.GetAsync("api/todo/4");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GivenItemWhenItemIsAlreadyCreatedThenBadRequestIsReturned()
        {
            var item = new TodoItem { Id = 1, Name = "Dummy" };
            string json = JsonConvert.SerializeObject(item);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/todo", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GivenItemWhenItemIsAddedThenSuccessIsReturned()
        {
            var item = new TodoItem { Id = 5, Name = "Dummy" };
            string json = JsonConvert.SerializeObject(item);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/todo", content);

            response.EnsureSuccessStatusCode();

            var actual = await GetById(5);

            Assert.Equal(5, actual.Id);
            Assert.Equal("Dummy", actual.Name);
        }

        [Fact]
        public async Task WhenItemIsUpdatedThenSuccessResponseIsReturned()
        {

            var item = new TodoItem { Name = "Jack", IsComplete = true };
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("api/todo/2", content);

            response.EnsureSuccessStatusCode();

            var actual = await GetById(2);

            Assert.Equal("Jack", actual.Name);
            Assert.True(actual.IsComplete);
        }

        [Fact]
        public async Task WhenItemIsDeletedThenSuccessResponseIsReturned()
        {
            var response = await client.DeleteAsync("api/todo/3");

            response.EnsureSuccessStatusCode();

            var actual = await IsNotFoundStatus(3);

            Assert.True(actual);
        }

        private async Task<TodoItem> GetById(long id)
        {
            var response = await client.GetAsync("api/todo/" + id);
            var value = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TodoItem>(value);
        }

        private async Task<bool> IsNotFoundStatus(long id)
        {
            var response = await client.GetAsync("api/todo/" + id);

            return response.StatusCode == HttpStatusCode.NotFound;
        }
    }
}
