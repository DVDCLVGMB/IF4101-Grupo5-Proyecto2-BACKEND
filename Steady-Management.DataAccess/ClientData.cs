using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using SteadyManagement.Domain;

namespace Steady_Management.DataAccess
{
    public class ClientData
    {
        private readonly string connectionString;

        public ClientData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // CREATE --------------------------------------------------
        public void Create(Client c)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("usp_Client_Create", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@city_id", c.CityId);
            command.Parameters.AddWithValue("@company_name", c.CompanyName);
            command.Parameters.AddWithValue("@contact_name", c.ContactName);
            command.Parameters.AddWithValue("@contact_surname", c.ContactSurname);
            command.Parameters.AddWithValue("@contact_rank", c.ContactRank);
            command.Parameters.AddWithValue("@client_address", c.ClientAddress);
            command.Parameters.AddWithValue("@client_phone_number", c.ClientPhoneNumber);
            command.Parameters.AddWithValue("@client_fax_number", c.ClientFaxNumber);
            command.Parameters.AddWithValue("@client_postal_code", c.ClientPostalCode);

            connection.Open();
            var result = command.ExecuteScalar();
            c.ClientId = Convert.ToInt32(result);
        }

        // READ ALL -----------------------------------------------
        public List<Client> GetAll()
        {
            var list = new List<Client>();
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("usp_Client_Read", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@client_id", DBNull.Value);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(Map(reader));
            }
            return list;
        }

        // READ BY ID ---------------------------------------------
        public Client? GetById(int id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("usp_Client_Read", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@client_id", id);

            connection.Open();
            using var reader = command.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        // UPDATE --------------------------------------------------
        public bool Update(Client c)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("usp_Client_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@client_id", c.ClientId);
            command.Parameters.AddWithValue("@city_id", c.CityId);
            command.Parameters.AddWithValue("@company_name", c.CompanyName);
            command.Parameters.AddWithValue("@contact_name", c.ContactName);
            command.Parameters.AddWithValue("@contact_surname", c.ContactSurname);
            command.Parameters.AddWithValue("@contact_rank", c.ContactRank);
            command.Parameters.AddWithValue("@client_address", c.ClientAddress);
            command.Parameters.AddWithValue("@client_phone_number", c.ClientPhoneNumber);
            command.Parameters.AddWithValue("@client_fax_number", c.ClientFaxNumber);
            command.Parameters.AddWithValue("@client_postal_code", c.ClientPostalCode);

            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        // DELETE --------------------------------------------------
        public bool Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("usp_Client_Delete", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@client_id", id);

            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        // MAPPER --------------------------------------------------
        private static Client Map(SqlDataReader reader) => new()
        {
            ClientId = (int)reader["client_id"],
            CityId = (int)reader["city_id"],
            CompanyName = (string)reader["company_name"],
            ContactName = (string)reader["contact_name"],
            ContactSurname = (string)reader["contact_surname"],
            ContactRank = (string)reader["contact_rank"],
            ClientAddress = (string)reader["client_address"],
            ClientPhoneNumber = (string)reader["client_phone_number"],
            ClientFaxNumber = (string)reader["client_fax_number"],
            ClientPostalCode = (string)reader["client_postal_code"]
        };
    }
}
