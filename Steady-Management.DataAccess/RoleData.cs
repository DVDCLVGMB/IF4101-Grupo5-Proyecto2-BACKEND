using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Steady_Management.Domain;

namespace Steady_Management.DataAccess
{
    public class RoleData
    {
        private readonly string _connectionString;

        public RoleData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            var roles = new List<Role>();

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            SqlCommand cmd = new("usp_Role_Read", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                roles.Add(new Role(
                    reader.GetInt32(0),
                    reader.GetString(1)
                ));
            }

            return roles;
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            SqlCommand cmd = new("usp_Role_Read", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@role_id", roleId);

            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Role(
                    reader.GetInt32(0),
                    reader.GetString(1)
                );
            }

            return null;
        }

        public async Task<int> CreateRoleAsync(Role role)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            SqlCommand cmd = new("usp_Role_Create", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@role_name", role.RoleName);

            object result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : -1;
        }

        public async Task UpdateRoleAsync(Role role)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            SqlCommand cmd = new("usp_Role_Update", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@role_id", role.RoleId);
            cmd.Parameters.AddWithValue("@role_name", role.RoleName);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            SqlCommand cmd = new("usp_Role_Delete", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@role_id", roleId);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
