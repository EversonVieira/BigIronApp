using BigIron.Api.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BigIron.Test
{
    public class ISRTests : IClassFixture<IntegrationTestWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly char _s = Path.DirectorySeparatorChar;
        private string rootPath => $"..{_s}..{_s}..{_s}..";
        public ISRTests(IntegrationTestWebApplicationFactory<Program> factory) 
        {
            _client = factory.CreateClient();
        }



        [Fact]
        public async Task Test_Process_Successfull()
        {
            //Arrange
            string csvContent = string.Empty;

            using (var fileReader = File.OpenRead($"{rootPath}{_s}data.csv"))
            {
                using (var streamReader = new StreamReader(fileReader, Encoding.UTF8))
                {
                    csvContent = streamReader.ReadToEnd();
                }
            }

            using (var content = new MultipartFormDataContent())
            {

                using var fileStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(csvContent));
                
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/csv");

                content.Add(fileContent, "File", "Test.csv");

                content.Add(new StringContent("12.3456"), "Latitude");
                content.Add(new StringContent("-45.6789"), "Longitude");

                // Act
                var response = await _client.PostAsync("api/isr", content);

                // Assert
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                Assert.NotEmpty(result);
            }

           

        }
    }
}
