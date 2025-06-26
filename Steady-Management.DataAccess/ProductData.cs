using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Steady_Management.Domain;

namespace Steady_Management.Data
{
    public class ProductData
    {
        private readonly string _connectionString;

        public ProductData(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Crea un nuevo producto y devuelve el nuevo ProductId.
        /// </summary>
        public int Create(Product product)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Product_Create", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@category_id", product.CategoryId);
            cmd.Parameters.AddWithValue("@product_name", product.ProductName);
            cmd.Parameters.AddWithValue("@price", product.Price);

            conn.Open();
            var result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Obtiene todos los productos.
        /// </summary>
        public IEnumerable<Product> GetAll()
        {
            var lista = new List<Product>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Product_Read", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            // como no se añade @product_id el SP devuelve todos los registros

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                lista.Add(new Product(
                    rdr.GetInt32(rdr.GetOrdinal("product_id")),
                    rdr.GetInt32(rdr.GetOrdinal("category_id")),
                    rdr.GetString(rdr.GetOrdinal("product_name")),
                    (float)rdr.GetDouble(rdr.GetOrdinal("price"))
                ));
            }

            return lista;
        }

        /// <summary>
        /// Obtiene un solo producto por su ID, o null si no existe.
        /// </summary>
        public Product? GetById(int productId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Product_Read", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@product_id", productId);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (!rdr.Read()) return null;

            return new Product(
                rdr.GetInt32(rdr.GetOrdinal("product_id")),
                rdr.GetInt32(rdr.GetOrdinal("category_id")),
                rdr.GetString(rdr.GetOrdinal("product_name")),
                (float)rdr.GetDouble(rdr.GetOrdinal("price"))
            );
        }

        /// <summary>
        /// Actualiza un producto existente. Devuelve true si afectó al menos una fila.
        /// </summary>
        public bool Update(Product product)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Product_Update", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@product_id", product.ProductId);
            cmd.Parameters.AddWithValue("@category_id", product.CategoryId);
            cmd.Parameters.AddWithValue("@product_name", product.ProductName);
            cmd.Parameters.AddWithValue("@price", product.Price);

            conn.Open();
            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }

        /// <summary>
        /// Elimina un producto por su ID. Devuelve un true si afectó al menos una fila.
        /// </summary>
        public bool Delete(int productId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Product_Delete", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@product_id", productId);

            conn.Open();
            var rows = cmd.ExecuteNonQuery();
            return rows > 0;
        }
    }
}
