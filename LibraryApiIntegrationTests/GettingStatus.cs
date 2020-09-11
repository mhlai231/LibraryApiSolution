using LibraryApi.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApiIntegrationTests
{
    public class GettingStatus : IClassFixture<WebTestFixture>
    {

        private HttpClient Client;

        public GettingStatus(WebTestFixture factory)
        {
            Client = factory.CreateClient();
            // switch to individual test
        }

        [Fact]
        public async Task GetAnOkStatusCode()
        {
            var response = await Client.GetAsync("/status");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SerializesAsJson()
        {
            var response = await Client.GetAsync("/status");
            var mediaType = response.Content.Headers.ContentType.MediaType;
            Assert.Equal("application/json", mediaType);
        }

        [Fact]
        public async Task HasCorrectData()
        {
            var response = await Client.GetAsync("/status");
            var content = await response.Content.ReadAsAsync<StatusResponse>();
            Assert.Equal("Looks good on my end. Party On!", content.message);
            Assert.Equal("Joe Schmidt", content.checkedBy);
            Assert.Equal(new DateTime(1969, 4, 20, 23, 59, 00), content.whenChecked);
        }


    }

    public class StatusResponse
    {
        public string message { get; set; }
        public string checkedBy { get; set; }
        public DateTime whenChecked { get; set; }
    }

}
