using Steady_Management.Api.Dtos;
using Steady_Management.DataAccess;
using Steady_Management.Domain;
using Steady_Management.WebAPI.DTOs;

namespace Steady_Management.WebAPI.Mappers
{
    public static class OrderMapper
    {
        // Conversión a entidad Order
        public static Order ToOrder(OrderDTO dto)
        {
            return new Order(
                orderId: 0, // se genera en base de datos
                clientId: dto.ClientId,
                employeeId: dto.EmployeeId,
                cityId: dto.CityId,
                orderDate: dto.OrderDate
            );
        }

        // Conversión a lista de detalles (OrderDetail)
        public static List<OrderDetail> ToOrderDetails(OrderDTO dto)
        {
            return dto.OrderDetails.Select(detail => new OrderDetail(
                orderId: 0, // será asignado posteriormente en Business
                productId: detail.ProductId,
                quantity: detail.Quantity,
                unitPrice: detail.UnitPrice
            )).ToList();
        }

        // Conversión a entidad Payment
        public static Payment ToPayment(OrderDTO dto)
        {
            return new Payment(
                paymentId: 0, // se genera en base de datos
                paymentMethodId: dto.PaymentMethodId,
                orderId: 0, // se asigna en Business al momento de insertar
                creditCardNumber: dto.CreditCardNumber,
                paymentQuantity: dto.PaymentQuantity,
                paymentDate: dto.PaymentDate
            );
        }

        //Convertir de entidades → DTO para lectura
        public static OrderDTO ToOrderDto(Order order, List<OrderDetail> details, Payment payment, string paymentMethodName)
        {
            return new OrderDTO
            {
                ClientId = order.ClientId,
                EmployeeId = order.EmployeeId,
                CityId = order.CityId,
                OrderDate = order.OrderDate,
                OrderDetails = details.Select(d => new OrderDetailDTO
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList(),
                PaymentMethodId = payment.PaymentMethodId,
                PaymentDate = payment.PaymentDate,
                CreditCardNumber = payment.CreditCardNumber,
                PaymentQuantity = payment.PaymentQuantity,
                PaymentMethodName = paymentMethodName
            };
        }
    }
}

