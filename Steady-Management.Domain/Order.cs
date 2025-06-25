using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management.Domain
{
    public class Order
    {
        private int orderId;
        private int clientId;
        private int employeeId;
        private int cityId;
        private DateTime orderDate;

        public Order(int orderId, int clientId, int employeeId, int cityId, DateTime orderDate)
        {
            this.orderId = orderId;
            this.clientId = clientId;
            this.employeeId = employeeId;
            this.cityId = cityId;
            this.orderDate = orderDate;
        }

        public int OrderId { get => orderId; set => orderId = value; }
        public int ClientId { get => clientId; set => clientId = value; }
        public int EmployeeId { get => employeeId; set => employeeId = value; }
        public int CityId { get => cityId; set => cityId = value; }
        public DateTime OrderDate { get => orderDate; set => orderDate = value; }
    }
}
