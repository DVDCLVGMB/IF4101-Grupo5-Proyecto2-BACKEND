using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management.Domain
{
    public class SteadyManagementInfo
    {
        private int setupId;
        private int cityId;
        private decimal salesTax;
        private string setupName;
        private string setupAddress;
        private string setupFaxNumber;
        private string setupPostalCode;
        private string setupPhoneNumber;

        public SteadyManagementInfo(int setupId, int cityId, decimal salesTax, string setupName, string setupAddress, string setupFaxNumber, string setupPostalCode, string setupPhoneNumber)
        {
            this.setupId = setupId;
            this.cityId = cityId;
            this.salesTax = salesTax;
            this.setupName = setupName;
            this.setupAddress = setupAddress;
            this.setupFaxNumber = setupFaxNumber;
            this.setupPostalCode = setupPostalCode;
            this.setupPhoneNumber = setupPhoneNumber;
        }

        public int SetupId { get => setupId; set => setupId = value; }
        public int CityId { get => cityId; set => cityId = value; }
        public decimal SalesTax { get => salesTax; set => salesTax = value; }
        public string SetupName { get => setupName; set => setupName = value; }
        public string SetupAddress { get => setupAddress; set => setupAddress = value; }
        public string SetupFaxNumber { get => setupFaxNumber; set => setupFaxNumber = value; }
        public string SetupPostalCode { get => setupPostalCode; set => setupPostalCode = value; }
        public string SetupPhoneNumber { get => setupPhoneNumber; set => setupPhoneNumber = value; }
    }
}
