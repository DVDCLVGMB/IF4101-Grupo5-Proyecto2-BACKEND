using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Steady_Management.Domain;

namespace Steady_Management.DataAccess
{
    public class InventoryData
    {
        private readonly string _connectionString;

        public InventoryData(string connectionString)
        {
            _connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Inserta un nuevo registro de inventario.
        /// </summary>
        public void Create(Inventory inv)
        {
            const string sql = @"
                INSERT INTO Inventory (product_id, size, item_quantity, limit_until_restock)
                VALUES (@product_id, @size, @item_quantity, @limit_until_restock);
            ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@product_id", inv.ProductId);
            cmd.Parameters.AddWithValue("@size", (object?)inv.Size ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@item_quantity", inv.ItemQuantity);
            cmd.Parameters.AddWithValue("@limit_until_restock", inv.LimitUntilRestock);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Obtiene todos los registros de inventario.
        /// </summary>
        public IEnumerable<Inventory> GetAll()
        {
            const string sql = @"
        SELECT 
            inventory_id,    -- ← lo añadimos
            product_id, 
            size, 
            item_quantity, 
            limit_until_restock
          FROM Inventory;
    ";

            var list = new List<Inventory>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new Inventory(
                    rdr.GetInt32(rdr.GetOrdinal("inventory_id")),         
                    rdr.GetInt32(rdr.GetOrdinal("product_id")),
                    rdr.IsDBNull(rdr.GetOrdinal("size")) ? null : rdr.GetString(rdr.GetOrdinal("size")),
                    rdr.GetInt32(rdr.GetOrdinal("item_quantity")),
                    rdr.GetInt32(rdr.GetOrdinal("limit_until_restock"))
                ));
            }
            return list;
        }


        /// <summary>
        /// Obtiene el inventario de un producto por su ID, o null si no existe.
        /// </summary>
        public Inventory? GetByProductId(int productId)
        {
            const string sql = @"
                    SELECT
                      inventory_id,
                      product_id,
                      size,
                      item_quantity,
                      limit_until_restock
                    FROM Inventory
                    WHERE product_id = @product_id;
            ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@product_id", productId);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (!rdr.Read()) return null;

            return ReadInventory(rdr);
        }

        /// <summary>
        /// Actualiza un registro de inventario existente. Devuelve true si afectó una fila.
        /// </summary>
        public bool Update(Inventory inv)
        {
            const string sql = @"
                UPDATE Inventory
                   SET size                = @size,
                       item_quantity       = @item_quantity,
                       limit_until_restock = @limit_until_restock
                 WHERE product_id = @product_id;
            ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@product_id", inv.ProductId);
            cmd.Parameters.AddWithValue("@size", (object?)inv.Size ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@item_quantity", inv.ItemQuantity);
            cmd.Parameters.AddWithValue("@limit_until_restock", inv.LimitUntilRestock);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Elimina el inventario de un producto. Devuelve true si afectó una fila.
        /// </summary>
        public bool Delete(int productId)
        {
            const string sql = @"
                DELETE FROM Inventory
                 WHERE product_id = @product_id;
            ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@product_id", productId);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // Helper: mapea un SqlDataReader a Inventory
        private static Inventory ReadInventory(SqlDataReader rdr) =>
            new Inventory(
                rdr.GetInt32(rdr.GetOrdinal("inventory_id")),
                rdr.GetInt32(rdr.GetOrdinal("product_id")),
                rdr.IsDBNull(rdr.GetOrdinal("size"))
                    ? null
                    : rdr.GetString(rdr.GetOrdinal("size")),
                rdr.GetInt32(rdr.GetOrdinal("item_quantity")),
                rdr.GetInt32(rdr.GetOrdinal("limit_until_restock"))
            );
    }
}
