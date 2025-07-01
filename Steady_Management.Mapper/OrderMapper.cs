using Steady_Management.Api.Dtos;
using Steady_Management.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Steady_Management.Mapper
{
    public static class OrderMapper
    {
        public static Order ToOrder(OrderDTO dto)
        {
            return new Order(
                0,
                dto.ClientId,
                dto.EmployeeId,
                dto.CityId,
                dto.OrderDate
            );
        }

        public static List<OrderDetail> ToOrderDetails(OrderDTO dto)
        {
            return dto.OrderDetails.Select(d => new OrderDetail(
                0,
                d.ProductId,
                d.Quantity,
                d.UnitPrice
            )).ToList();
        }

        public static Payment ToPayment(OrderDTO dto)
        {
            return new Payment(
                0,
                0, // Se setea después en Business
                dto.Payment.PaymentMethodId,
                dto.Payment.CreditCardNumber,
                0, // Se setea después en Business
                DateTime.MinValue // Se setea después en Business
            );
        }
    }
}


