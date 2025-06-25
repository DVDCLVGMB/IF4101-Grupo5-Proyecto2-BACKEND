using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management.Domain
{
    public class PaymentMethod
    {
        private int paymentMethodId;
        private string paymentMethodName;

        public PaymentMethod(int paymentMethodId, string paymentMethodName)
        {
            this.paymentMethodId = paymentMethodId;
            this.paymentMethodName = paymentMethodName;
        }

        public int PaymentMethodId { get => paymentMethodId; set => paymentMethodId = value; }
        public string PaymentMethodName { get => paymentMethodName; set => paymentMethodName = value; }
    }
}
