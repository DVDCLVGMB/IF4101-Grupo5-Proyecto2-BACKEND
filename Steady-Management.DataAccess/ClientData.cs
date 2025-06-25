using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SteadyManagement.Domain;

namespace Steady_Management.DataAccess
{
    public class ClientData
    {
        private readonly string _conn;

        public ClientData(string connectionString) => _conn = connectionString;

        // CREATE --------------------------------------------------
        public void Create(Client c)
        {
            const string sql = @"
                INSERT INTO Client
                      (city_id, company_name, contact_name, contact_surname,
                       contact_rank, client_address, client_phone_number,
                       client_fax_number, client_postal_code)
                VALUES (@CityId,@Company,@Name,@Surname,@Rank,
                        @Address,@Phone,@Fax,@Postal);
                SELECT SCOPE_IDENTITY();";

            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@CityId", c.CityId);
            cmd.Parameters.AddWithValue("@Company", c.CompanyName);
            cmd.Parameters.AddWithValue("@Name", c.ContactName);
            cmd.Parameters.AddWithValue("@Surname", c.ContactSurname);
            cmd.Parameters.AddWithValue("@Rank", c.ContactRank);
            cmd.Parameters.AddWithValue("@Address", c.ClientAddress);
            cmd.Parameters.AddWithValue("@Phone", c.ClientPhoneNumber);
            cmd.Parameters.AddWithValue("@Fax", c.ClientFaxNumber);
            cmd.Parameters.AddWithValue("@Postal", c.ClientPostalCode);
            cn.Open();
            c.ClientId = Convert.ToInt32(cmd.ExecuteScalar());
        }

        // READ ALL -----------------------------------------------
        public List<Client> GetAll()
        {
            const string sql = "SELECT * FROM Client";
            var list = new List<Client>();

            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(sql, cn);
            cn.Open();
            using var rd = cmd.ExecuteReader();
            while (rd.Read()) list.Add(Map(rd));
            return list;
        }

        // READ BY ID ---------------------------------------------
        public Client? GetById(int id)
        {
            const string sql = "SELECT * FROM Client WHERE client_id=@Id";
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@Id", id);
            cn.Open();
            using var rd = cmd.ExecuteReader();
            return rd.Read() ? Map(rd) : null;
        }

        // UPDATE --------------------------------------------------
        public bool Update(Client c)
        {
            const string sql = @"
              UPDATE Client SET
                  city_id=@CityId, company_name=@Company,
                  contact_name=@Name, contact_surname=@Surname,
                  contact_rank=@Rank, client_address=@Address,
                  client_phone_number=@Phone, client_fax_number=@Fax,
                  client_postal_code=@Postal
              WHERE client_id=@Id";
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@CityId", c.CityId);
            cmd.Parameters.AddWithValue("@Company", c.CompanyName);
            cmd.Parameters.AddWithValue("@Name", c.ContactName);
            cmd.Parameters.AddWithValue("@Surname", c.ContactSurname);
            cmd.Parameters.AddWithValue("@Rank", c.ContactRank);
            cmd.Parameters.AddWithValue("@Address", c.ClientAddress);
            cmd.Parameters.AddWithValue("@Phone", c.ClientPhoneNumber);
            cmd.Parameters.AddWithValue("@Fax", c.ClientFaxNumber);
            cmd.Parameters.AddWithValue("@Postal", c.ClientPostalCode);
            cmd.Parameters.AddWithValue("@Id", c.ClientId);
            cn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // DELETE --------------------------------------------------
        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Client WHERE client_id=@Id";
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@Id", id);
            cn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        // MAPPER --------------------------------------------------
        private static Client Map(SqlDataReader r) => new()
        {
            ClientId = (int)r["client_id"],
            CityId = (int)r["city_id"],
            CompanyName = (string)r["company_name"],
            ContactName = (string)r["contact_name"],
            ContactSurname = (string)r["contact_surname"],
            ContactRank = (string)r["contact_rank"],
            ClientAddress = (string)r["client_address"],
            ClientPhoneNumber = (string)r["client_phone_number"],
            ClientFaxNumber = (string)r["client_fax_number"],
            ClientPostalCode = (string)r["client_postal_code"]
        };
    }
}
