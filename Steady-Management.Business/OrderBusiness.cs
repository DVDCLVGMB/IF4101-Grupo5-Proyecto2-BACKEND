﻿using Microsoft.Data.SqlClient;
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
        private readonly PaymentData _paymentData;

        public OrderBusiness(string connectionString)
        {
            _orderData = new OrderData(connectionString);
            _productData = new ProductData(connectionString);
            _paymentData = new PaymentData(connectionString);
        }

        public List<Order> GetAll()
        {
            return _orderData.GetAll();
        }

        public List<Order> GetByClientId(int id)
        {
            return _orderData.GetByClientId(id);
        }

        public List<Order> GetByCityId(int id)
        {
            return _orderData.GetByCityId(id);
        }

        public List<Order> GetByDate(DateOnly date)
        {
            return _orderData.GetByDate(date);
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
                    var product = _productData.GetById(detail.ProductId);
                    if (product == null)
                        throw new InvalidOperationException("Error en la inserción. Compruebe los datos ingresados.");

                    var inventory = _orderData.GetInventoryByProductId(detail.ProductId);
                    if (inventory == null || inventory.ItemQuantity < detail.Quantity)
                        throw new InvalidOperationException($"No hay suficiente inventario para el producto: {product.ProductName}.");

                    decimal subtotalProducto = detail.UnitPrice * detail.Quantity;
                    subtotal += subtotalProducto;

                    if (product.IsTaxable)
                        totalImpuestos += subtotalProducto * salesTaxPercentage;

                    detail.OrderId = orderId;
                    _orderData.InsertOrderDetail(detail, connection, transaction);
                    _orderData.UpdateInventoryAfterSale(detail.ProductId, detail.Quantity, connection, transaction);
                }

                decimal total = subtotal + totalImpuestos;
                payment.OrderId = orderId;
                payment.PaymentQuantity = total;
                payment.PaymentDate = DateTime.Now;

                _paymentData.Insert(payment, connection, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }



        public List<(Order Order, List<OrderDetail> Details, Payment Payment, string PaymentMethodName)> GetAllOrders()
        {
            var orders = _orderData.GetAllOrders();
            var result = new List<(Order, List<OrderDetail>, Payment, string)>();

            foreach (var order in orders)
            {
                var details = _orderData.GetOrderDetailsByOrderId(order.OrderId);
                var payment = _paymentData.GetByOrderId(order.OrderId)!;
                var methodName = _paymentData.GetPaymentMethodName(payment.PaymentMethodId);

                result.Add((order, details, payment, methodName));
            }

            return result;
        }


        public void UpdateOrder(int orderId, Order updatedOrder, List<OrderDetail> newDetails, Payment updatedPayment)
        {
            using var connection = _orderData.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Si no hay detalles nuevos, solo actualiza la cabecera y termina
                if (newDetails == null || newDetails.Count == 0)
                {
                    const string sql = @"
                UPDATE Orders
                   SET client_id   = @clientId,
                       employee_id = @employeeId,
                       city_id     = @cityId,
                       order_date  = @orderDate
                 WHERE order_id    = @orderId;
            ";

                    using var cmd = connection.CreateCommand();
                    cmd.Transaction = transaction;
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@clientId", updatedOrder.ClientId);
                    cmd.Parameters.AddWithValue("@employeeId", updatedOrder.EmployeeId);
                    cmd.Parameters.AddWithValue("@cityId", updatedOrder.CityId);
                    cmd.Parameters.AddWithValue("@orderDate", updatedOrder.OrderDate);
                    cmd.Parameters.AddWithValue("@orderId", orderId);

                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    return;
                }

                // 1) Restaurar inventario de los detalles originales
                var originalDetails = _orderData.GetOrderDetailsByOrderId(orderId);
                foreach (var detail in originalDetails)
                {
                    _orderData.RestoreInventoryFromCancelledOrder(
                        detail.ProductId,
                        detail.Quantity,
                        connection,
                        transaction
                    );
                }

                // 2) Eliminar detalles y pago previo
                _orderData.DeleteOrderDetails(orderId, connection, transaction);
                _paymentData.DeleteByOrderId(orderId, connection, transaction);

                // 3) Recalcular subtotal e impuestos con los detalles nuevos
                var salesTaxPercentage = _orderData.GetSalesTaxPercentage() / 100m;
                decimal subtotal = 0;
                decimal totalImpuestos = 0;

                foreach (var detail in newDetails)
                {
                    var product = _productData.GetById(detail.ProductId);
                    if (product == null)
                        throw new InvalidOperationException("Error en la inserción. Compruebe los datos ingresados.");

                    var inventory = _orderData.GetInventoryByProductId(
                        detail.ProductId, connection, transaction
                    );
                    if (inventory == null || inventory.ItemQuantity < detail.Quantity)
                        throw new InvalidOperationException(
                            $"No hay suficiente inventario para el producto: {product.ProductName}."
                        );

                    decimal subtotalProducto = detail.UnitPrice * detail.Quantity;
                    subtotal += subtotalProducto;

                    if (product.IsTaxable)
                        totalImpuestos += subtotalProducto * salesTaxPercentage;

                    detail.OrderId = orderId;
                    _orderData.InsertOrderDetail(detail, connection, transaction);
                    _orderData.UpdateInventoryAfterSale(
                        detail.ProductId, detail.Quantity, connection, transaction
                    );
                }

                // 4) Insertar nuevo pago
                decimal total = subtotal + totalImpuestos;
                updatedPayment.OrderId = orderId;
                updatedPayment.PaymentQuantity = total;
                updatedPayment.PaymentDate = DateTime.Now;
                _paymentData.Insert(updatedPayment, connection, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


        public void DeleteOrder(int orderId)
        {
            using var connection = _orderData.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Obtener detalles para restaurar inventario
                var details = _orderData.GetOrderDetailsByOrderId(orderId);

                // 2. Restaurar inventario para cada producto
                foreach (var detail in details)
                {
                    _orderData.RestoreInventoryFromCancelledOrder(detail.ProductId, detail.Quantity, connection, transaction);
                }

                // 3. Eliminar detalles de la orden
                _orderData.DeleteOrderDetails(orderId, connection, transaction);

                // 4. Eliminar pago
                _paymentData.DeleteByOrderId(orderId, connection, transaction);

                // 5. Eliminar orden principal
                _orderData.DeleteOrder(orderId, connection, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public decimal GetSalesTaxPercentage()
        {
            return _orderData.GetSalesTaxPercentage();
        }

    }
}
