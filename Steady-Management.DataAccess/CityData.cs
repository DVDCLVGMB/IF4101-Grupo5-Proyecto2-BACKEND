using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using SteadyManagement.Domain;

namespace Steady_Management.DataAccess
{
    public class CityData
    {
        private readonly string _cs;
        public CityData(string connectionString) => _cs = connectionString;

        public int Create(City city)
        {
            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand("usp_City_Create", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@city_name", city.CityName);
            cmd.Parameters.AddWithValue("@province", city.Province);
            cmd.Parameters.AddWithValue("@country", city.Country);

            conn.Open();
            // ExecuteScalar devuelve el nuevo ID
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<City> GetAll()
        {
            var list = new List<City>();
            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand("usp_City_Read", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@city_id", DBNull.Value);
            conn.Open();
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add(new City
                {
                    CityId = (int)r["city_id"],
                    CityName = (string)r["city_name"],
                    Province = (int)r["province"],
                    Country = (string)r["country"]
                });
            return list;
        }

        public City GetById(int id)
        {
            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand("usp_City_Read", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@city_id", id);
            conn.Open();
            using var r = cmd.ExecuteReader();
            return r.Read()
                ? new City
                {
                    CityId = (int)r["city_id"],
                    CityName = (string)r["city_name"],
                    Province = (int)r["province"],
                    Country = (string)r["country"]
                }
                : null;
        }

        public bool Update(City city)
        {
            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand("usp_City_Update", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@city_id", city.CityId);
            cmd.Parameters.AddWithValue("@city_name", city.CityName);
            cmd.Parameters.AddWithValue("@province", city.Province);
            cmd.Parameters.AddWithValue("@country", city.Country);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = new SqlConnection(_cs);
            using var cmd = new SqlCommand("usp_City_Delete", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@city_id", id);
            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
