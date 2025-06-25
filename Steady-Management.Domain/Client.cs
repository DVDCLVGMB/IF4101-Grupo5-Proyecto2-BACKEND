using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteadyManagement.Domain
{
    public class Client
    {
        public int ClientId { get; set; }
        public int CityId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string ContactSurname { get; set; } = string.Empty;
        public string ContactRank { get; set; } = string.Empty;
        public string ClientAddress { get; set; } = string.Empty;
        public string ClientPhoneNumber { get; set; } = string.Empty;
        public string ClientFaxNumber { get; set; } = string.Empty;
        public string ClientPostalCode { get; set; } = string.Empty;
    }
}
