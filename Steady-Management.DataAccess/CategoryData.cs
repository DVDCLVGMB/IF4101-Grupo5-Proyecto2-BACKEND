using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Steady_Management.Domain;

namespace Steady_Management.Data
{
    public class CategoryData
    {
        private readonly string _connectionString;

        public CategoryData(string connectionString)
        {
            _connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public int Create(Category category)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Category_Create", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@description", category.Description);

            conn.Open();
            var id = cmd.ExecuteScalar();
            return Convert.ToInt32(id);
        }

        public IEnumerable<Category> GetAll()
        {
            var list = new List<Category>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Category_Read", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new Category(
                    rdr.GetInt32(rdr.GetOrdinal("category_id")),
                    rdr.GetString(rdr.GetOrdinal("description"))
                ));
            }

            return list;
        }

        public Category? GetById(int categoryId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Category_Read", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@category_id", categoryId);

            conn.Open();
            using var rdr = cmd.ExecuteReader();
            if (!rdr.Read()) return null;

            return new Category(
                rdr.GetInt32(rdr.GetOrdinal("category_id")),
                rdr.GetString(rdr.GetOrdinal("description"))
            );
        }

        public bool Update(Category category)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Category_Update", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@category_id", category.CategoryId);
            cmd.Parameters.AddWithValue("@description", category.Description);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int categoryId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("usp_Category_Delete", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@category_id", categoryId);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
