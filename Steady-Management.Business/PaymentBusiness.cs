using Microsoft.Data.SqlClient;
using Steady_Management.DataAccess;
using Steady_Management.Domain;

namespace Steady_Management.Business
{
    public class PaymentBusiness
    {
        private readonly PaymentData _paymentData;

        public PaymentBusiness(string connectionString)
        {
            _paymentData = new PaymentData(connectionString);
        }

        public Payment? GetByOrderId(int orderId)
        {
            return _paymentData.GetByOrderId(orderId);
        }

        public string GetPaymentMethodName(int paymentMethodId)
        {
            return _paymentData.GetPaymentMethodName(paymentMethodId);
        }

        public void Insert(Payment payment, SqlConnection connection, SqlTransaction transaction)
        {
            _paymentData.Insert(payment, connection, transaction);
        }

        public void DeleteByOrderId(int orderId, SqlConnection connection, SqlTransaction transaction)
        {
            _paymentData.DeleteByOrderId(orderId, connection, transaction);
        }
    }
}

