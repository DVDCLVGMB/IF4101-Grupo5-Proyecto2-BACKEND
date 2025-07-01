using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management.Domain
{
    public class Payment
    {
        private int paymentId;
        private int paymentMethodId;
        private int orderId;
        private string creditCardNumber;
        private decimal paymentQuantity;
        private DateTime paymentDate;

        public Payment(int paymentId, int paymentMethodId, int orderId, string creditCardNumber, decimal paymentQuantity, DateTime paymentDate)
        {
            this.paymentId = paymentId;
            this.paymentMethodId = paymentMethodId;
            this.orderId = orderId;
            this.creditCardNumber = creditCardNumber;
            this.paymentQuantity = paymentQuantity;
            this.paymentDate = paymentDate;
        }

        public int PaymentId { get => paymentId; set => paymentId = value; }
        public int PaymentMethodId { get => paymentMethodId; set => paymentMethodId = value; }
        public int OrderId { get => orderId; set => orderId = value; }
        public string CreditCardNumber { get => creditCardNumber; set => creditCardNumber = value; }
        public decimal PaymentQuantity { get => paymentQuantity; set => paymentQuantity = value; }
        public DateTime PaymentDate { get => paymentDate; set => paymentDate = value; }
    }
}
