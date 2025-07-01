using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management.Domain
{
    public class OrderDetail
    {
        private int orderId;
        private int productId;
        private int quantity;
        private decimal unitPrice;

        public OrderDetail(int orderId, int productId, int quantity, decimal unitPrice)
        {
            this.orderId = orderId;
            this.productId = productId;
            this.quantity = quantity;
            this.unitPrice = unitPrice;
        }

        public int OrderId { get => orderId; set => orderId = value; }
        public int ProductId { get => productId; set => productId = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public decimal UnitPrice { get => unitPrice; set => unitPrice = value; }
    }
}
