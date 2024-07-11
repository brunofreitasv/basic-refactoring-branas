using basic_refactoring_branas.Application.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace basic_refactoring_branas.test
{
    [TestFixture]
    public class ApiTest
    {
        private readonly string _ApiBaseUrl = "http://localhost:59689";
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [TearDown]
        public void TearDown() 
        {
            _client.Dispose();
        }

        [Test]
        public async Task SignupAsync_InvalidCpf_ReturnsInvalidCpf()
        {
            var account = new Account { Name = "Valid Name", Email = $"test{Guid.NewGuid()}@example.com", Cpf = "123456789" };
            var content = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{_ApiBaseUrl}/signup", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.UnprocessableEntity));

            var result = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

            Assert.That(result, Is.EqualTo(SignupResult.InvalidCpf.Message));
        }

        [Test]
        public async Task SignupAsync_Passenger_ReturnsSuccess()
        {
            var account = new Account
            {
                Name = "Valid Name",
                Email = $"test{Guid.NewGuid()}@example.com",
                Cpf = "97456321558",
                IsPassenger = true
            };

            var content = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");

            var signupResponse = await _client.PostAsync($"{_ApiBaseUrl}/signup", content);

            Assert.That(signupResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var accountId = JsonConvert.DeserializeObject<string>(await signupResponse.Content.ReadAsStringAsync());

            Assert.IsNotNull(accountId);

            var getAccountResponse = await _client.GetAsync($"{_ApiBaseUrl}/accounts/{accountId}");

            Assert.That(getAccountResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var outputAccout = JsonConvert.DeserializeObject<Account>(await getAccountResponse.Content.ReadAsStringAsync());

            Assert.IsNotNull(outputAccout);
            Assert.That(outputAccout.Name, Is.EqualTo(account.Name));
            Assert.That(outputAccout.Email, Is.EqualTo(account.Email));
            Assert.That(outputAccout.Cpf, Is.EqualTo(account.Cpf));
        }
    }
}