using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace SteadyManagement.Domain
{
    public class Client
    {
        private int clientId;
        private int cityId;
        private string companyName;
        private string contactName;
        private string contactSurname;
        private string contactRank;
        private string clientAddress;
        private string clientPhoneNumber;
        private string clientFaxNumber;
        private string clientPostalCode;

        public Client()
        {
        }

        public Client(int clientId, int cityId, string companyName, string contactName, string contactSurname,
                      string contactRank, string clientAddress, string clientPhoneNumber,
                      string clientFaxNumber, string clientPostalCode)
        {
            this.clientId = clientId;
            this.cityId = cityId;
            this.companyName = companyName;
            this.contactName = contactName;
            this.contactSurname = contactSurname;
            this.contactRank = contactRank;
            this.clientAddress = clientAddress;
            this.clientPhoneNumber = clientPhoneNumber;
            this.clientFaxNumber = clientFaxNumber;
            this.clientPostalCode = clientPostalCode;
        }

        public int ClientId { get => clientId; set => clientId = value; }
        public int CityId { get => cityId; set => cityId = value; }
        public string CompanyName { get => companyName; set => companyName = value; }
        public string ContactName { get => contactName; set => contactName = value; }
        public string ContactSurname { get => contactSurname; set => contactSurname = value; }
        public string ContactRank { get => contactRank; set => contactRank = value; }
        public string ClientAddress { get => clientAddress; set => clientAddress = value; }
        public string ClientPhoneNumber { get => clientPhoneNumber; set => clientPhoneNumber = value; }
        public string ClientFaxNumber { get => clientFaxNumber; set => clientFaxNumber = value; }
        public string ClientPostalCode { get => clientPostalCode; set => clientPostalCode = value; }
    }
}
