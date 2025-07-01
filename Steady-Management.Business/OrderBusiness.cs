using Microsoft.Data.SqlClient;
using Steady_Management.Data;
using Steady_Management.DataAccess;
using Steady_Management.Domain;
using System;
using System.Collections.Generic;

namespace Steady_Management.Business
{
    public class OrderBusiness
    {
        private readonly OrderData _orderData;
        private readonly ProductData _productData;

        public OrderBusiness(string connectionString)
        {
            _orderData = new OrderData(connectionString);
            _productData = new ProductData(connectionString);
        }

        public void CreateOrder(Order order, List<OrderDetail> details, Payment payment)
        {
            using var connection = _orderData.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var orderId = _orderData.InsertOrder(order, connection, transaction);
                var salesTaxPercentage = _orderData.GetSalesTaxPercentage() / 100m;

                decimal subtotal = 0;
                decimal totalImpuestos = 0;

                foreach (var detail in details)
                {
                    var inventory = _orderData.GetInventoryByProductId(detail.ProductId);
                    if (inventory == null || inventory.ItemQuantity < detail.Quantity)
                        throw new InvalidOperationException($"No hay suficiente inventario para el producto {detail.ProductId}.");

                    var product = _productData.GetById(detail.ProductId);
                    decimal subtotalProducto = detail.UnitPrice * detail.Quantity;
                    subtotal += subtotalProducto;

                    if (product != null && product.IsTaxable)
                        totalImpuestos += subtotalProducto * salesTaxPercentage;

                    detail.OrderId = orderId;
                    _orderData.InsertOrderDetail(detail, connection, transaction);
                    _orderData.UpdateInventoryAfterSale(detail.ProductId, detail.Quantity, connection, transaction);
                }

                decimal total = subtotal + totalImpuestos;
                payment.OrderId = orderId;
                payment.PaymentQuantity = total;
                payment.PaymentDate = DateTime.Now;

                _orderData.InsertPayment(payment, connection, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
