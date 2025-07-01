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
    }
}




