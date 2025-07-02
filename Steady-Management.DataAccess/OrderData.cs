using Steady_Management.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Steady_Management.DataAccess
{
    public class OrderData
    {
        private readonly string connectionString;

        public OrderData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private static Order Map(SqlDataReader reader) => new()
        {
            OrderId = (int)reader["order_id"],
            ClientId = (int)reader["client_id"],
            EmployeeId = (int)reader["employee_id"],
            CityId = (int)reader["city_id"],
            OrderDate = reader.GetDateTime(reader.GetOrdinal("order_date"))
        };

        public List<Order> GetAll()
        {
            var orders = new List<Order>();
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("usp_Order_Read_ALL", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                orders.Add(Map(reader));
            }
            return orders;
        }

        public List<Order> GetByClientId(int clientId)
        {
            var orders = new List<Order>();
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("usp_Client_Filtraded", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@client_id", clientId);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                orders.Add(Map(reader));
            }
            return orders;
        }

        public List<Order> GetByCityId(int cityId)
        {
            var orders = new List<Order>();
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("usp_City_Filtraded", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@city_id", cityId);
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                orders.Add(Map(reader));
            }
            return orders;
        }

        public List<Order> GetByDate(DateOnly orderDate)
        {
            var orders = new List<Order>();
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("usp_PedidosPorFecha", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@order_date", orderDate.ToDateTime(TimeOnly.MinValue));
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                orders.Add(Map(reader));
            }
            return orders;
        }


        public SqlConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }

        public int InsertOrder(Order order, SqlConnection connection, SqlTransaction transaction)
        {
            using (var command = new SqlCommand("usp_Order_Create", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@client_id", order.ClientId);
                command.Parameters.AddWithValue("@employee_id", order.EmployeeId);
                command.Parameters.AddWithValue("@city_id", order.CityId);
                command.Parameters.AddWithValue("@order_date", order.OrderDate);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void InsertOrderDetail(OrderDetail detail, SqlConnection connection, SqlTransaction transaction)
        {
            using var command = new SqlCommand("usp_OrderDetail_Insert", connection, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@order_id", detail.OrderId);
            command.Parameters.AddWithValue("@product_id", detail.ProductId);
            command.Parameters.AddWithValue("@quantity", detail.Quantity);
            command.Parameters.Add("@unit_price", SqlDbType.Decimal).Value = detail.UnitPrice;

            command.ExecuteNonQuery();
        }

        public void InsertPayment(Payment payment, SqlConnection connection, SqlTransaction transaction)
        {
            using (var command = new SqlCommand("usp_Payment_Insert", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@payment_method_id", payment.PaymentMethodId);
                command.Parameters.AddWithValue("@order_id", payment.OrderId);
                command.Parameters.AddWithValue("@credit_card_number", payment.CreditCardNumber);
                command.Parameters.AddWithValue("@payment_quantity", payment.PaymentQuantity);
                command.Parameters.AddWithValue("@payment_date", payment.PaymentDate);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateInventoryAfterSale(int productId, int quantitySold, SqlConnection connection, SqlTransaction transaction)
        {
            using (var command = new SqlCommand("usp_Inventory_UpdateAfterSale", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@product_id", productId);
                command.Parameters.AddWithValue("@quantity_sold", quantitySold);

                command.ExecuteNonQuery();
            }
        }

        //Método para Create, read
        public Inventory? GetInventoryByProductId(int productId)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("SELECT * FROM Inventory WHERE product_id = @product_id", connection);
            command.Parameters.AddWithValue("@product_id", productId);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Inventory(
                    reader.GetInt32(reader.GetOrdinal("product_id")),
                    reader.IsDBNull(reader.GetOrdinal("size")) ? string.Empty : reader.GetString(reader.GetOrdinal("size")),
                    reader.GetInt32(reader.GetOrdinal("item_quantity")),
                    reader.GetInt32(reader.GetOrdinal("limit_until_restock"))
                );
            }

            return null;
        }

        //Método para update
        public Inventory? GetInventoryByProductId(int productId, SqlConnection connection, SqlTransaction transaction)
        {
            string sql = @"
        SELECT product_id, size, item_quantity, limit_until_restock
        FROM Inventory
        WHERE product_id = @product_id";

            using var command = new SqlCommand(sql, connection, transaction);
            command.Parameters.AddWithValue("@product_id", productId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Inventory(
                    reader.GetInt32(reader.GetOrdinal("product_id")),
                    reader.IsDBNull(reader.GetOrdinal("size")) ? string.Empty : reader.GetString(reader.GetOrdinal("size")),
                    reader.GetInt32(reader.GetOrdinal("item_quantity")),
                    reader.GetInt32(reader.GetOrdinal("limit_until_restock"))
                );
            }

            return null;
        }


        public decimal GetSalesTaxPercentage()
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("SELECT sales_tax FROM Parameter", connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return reader.GetDecimal(0);
            }

            throw new InvalidOperationException("No se encontró el parámetro 'sales_tax'.");
        }

        //Read orders

        public List<Order> GetAllOrders()
        {
            var orders = new List<Order>();
            string sql = @"
                SELECT order_id, client_id, employee_id, city_id, order_date
                FROM [Order]";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                orders.Add(new Order(
                    orderId: reader.GetInt32(reader.GetOrdinal("order_id")),
                    clientId: reader.GetInt32(reader.GetOrdinal("client_id")),
                    employeeId: reader.GetInt32(reader.GetOrdinal("employee_id")),
                    cityId: reader.GetInt32(reader.GetOrdinal("city_id")),
                    orderDate: reader.GetDateTime(reader.GetOrdinal("order_date"))
                ));
            }

            return orders;
        }

        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            var details = new List<OrderDetail>();
            string sql = @"
                SELECT order_id, product_id, quantity, unit_price
                FROM OrderDetail
                WHERE order_id = @order_id";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@order_id", orderId);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                details.Add(new OrderDetail(
                    orderId: reader.GetInt32(reader.GetOrdinal("order_id")),
                    productId: reader.GetInt32(reader.GetOrdinal("product_id")),
                    quantity: reader.GetInt32(reader.GetOrdinal("quantity")),
                    unitPrice: reader.GetDecimal(reader.GetOrdinal("unit_price"))
                ));
            }

            return details;
        }

        public Payment? GetPaymentByOrderId(int orderId)
        {
            string sql = @"
                SELECT payment_id, payment_method_id, order_id, credit_card_number, 
                       payment_quantity, payment_date
                FROM Payment
                WHERE order_id = @order_id";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@order_id", orderId);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Payment(
                    paymentId: reader.GetInt32(reader.GetOrdinal("payment_id")),
                    paymentMethodId: reader.GetInt32(reader.GetOrdinal("payment_method_id")),
                    orderId: reader.GetInt32(reader.GetOrdinal("order_id")),
                    creditCardNumber: reader.GetString(reader.GetOrdinal("credit_card_number")),
                    paymentQuantity: reader.GetDecimal(reader.GetOrdinal("payment_quantity")),
                    paymentDate: reader.GetDateTime(reader.GetOrdinal("payment_date"))
                );
            }

            return null;
        }

        public string GetPaymentMethodName(int paymentMethodId)
        {
            string sql = @"
                SELECT payment_method_name
                FROM PaymentMethod
                WHERE payment_method_id = @id";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", paymentMethodId);

            connection.Open();
            return command.ExecuteScalar()?.ToString() ?? string.Empty;
        }

        public void DeleteOrderDetails(int orderId, SqlConnection connection, SqlTransaction transaction)
        {
            using var command = new SqlCommand("usp_OrderDetail_DeleteByOrderId", connection, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@order_id", orderId);
            command.ExecuteNonQuery();
        }

        public void DeletePayment(int orderId, SqlConnection connection, SqlTransaction transaction)
        {
            using var command = new SqlCommand("usp_Payment_Delete", connection, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@order_id", orderId);
            command.ExecuteNonQuery();
        }

        public void RestoreInventoryFromCancelledOrder(int productId, int quantity, SqlConnection connection, SqlTransaction transaction)
        {
            using var command = new SqlCommand("usp_Inventory_RestoreFromCancelledOrder", connection, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@product_id", productId);
            command.Parameters.AddWithValue("@quantity", quantity);
            command.ExecuteNonQuery();
        }

        //Delete methods

        public void DeleteOrder(int orderId, SqlConnection connection, SqlTransaction transaction)
        {
            string sql = @"DELETE FROM [Order] WHERE order_id = @order_id";
            using var command = new SqlCommand(sql, connection, transaction);
            command.Parameters.AddWithValue("@order_id", orderId);
            command.ExecuteNonQuery();
        }



    }
}




