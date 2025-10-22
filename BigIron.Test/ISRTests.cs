using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BigIron.Test
{
    public class ISRTests
    {
        private readonly HttpClient _client;

        public ISRTests(IntegrationTestWebApplicationFactory<Program> factory) 
        {
            _client = factory.CreateClient();
        }



        [Fact]
        public void Test_Process()
        {
            
        }
    }
}
