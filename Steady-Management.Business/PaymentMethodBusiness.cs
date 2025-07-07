using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steady_Management.DataAccess;
using Steady_Management.Domain;

namespace Steady_Management.Business
{
    public class PaymentMethodBusiness
    {
        private readonly PaymentMethodData paymentMethodData;

        public PaymentMethodBusiness(string connectionString)
        {
            paymentMethodData = new PaymentMethodData(connectionString);

        }

        public List<PaymentMethod> GetAll()
        {
            return paymentMethodData.GetAll();
        }

    }
}
