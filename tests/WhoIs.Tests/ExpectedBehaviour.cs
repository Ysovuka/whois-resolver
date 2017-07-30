using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WhoIs.IANA;
using Xunit;

namespace WhoIs.Tests
{
    public class ExpectedBehaviour
    {
        [Fact]
        public async Task GetWhoIsInformationForGoogle()
        {
            WHOISRequest request = new WHOISRequest();
            WHOISResponse response = await request.LookupAsync("google.com");

            Assert.Equal("COM", response.IANARecord.Domain);
            Assert.Equal("GOOGLE.COM", response.WHOISRecord.Domain);
        }
    }
}
