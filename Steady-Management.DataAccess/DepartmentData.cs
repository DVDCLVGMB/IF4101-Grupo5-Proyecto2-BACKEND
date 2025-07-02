using Microsoft.Data.SqlClient;
using Steady_Management.Domain;
using System;
using System.Collections.Generic;
using System.Data;

namespace Steady_Management.DataAccess
{
    /// <summary>
    /// Acceso a datos para la tabla Department
    /// </summary>
    public class DepartmentData
    {
        private readonly string _conn;

        public DepartmentData(string connectionString) => _conn = connectionString;

        // -------------------  CREATE  -------------------
        public void Create(Department d)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("dbo.usp_CreateDepartment", cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Name", d.DeptName);
            var pId = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(pId);

            cn.Open();
            cmd.ExecuteNonQuery();

            d.DeptId = (int)pId.Value;
        }

        public List<Department> GetAll()
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("dbo.usp_GetAllDepartments", cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cn.Open();
            using var rd = cmd.ExecuteReader();
            var list = new List<Department>();
            while (rd.Read())
                list.Add(Map(rd));
            return list;
        }

        public Department? GetById(int id)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("dbo.usp_GetDepartmentById", cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", id);
            cn.Open();
            using var rd = cmd.ExecuteReader();
            return rd.Read() ? Map(rd) : null;
        }

        public bool Update(Department d)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("dbo.usp_UpdateDepartment", cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", d.DeptId);
            cmd.Parameters.AddWithValue("@Name", d.DeptName);
            var pRows = new SqlParameter("@RowsAffected", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(pRows);

            cn.Open();
            cmd.ExecuteNonQuery();
            return (int)pRows.Value > 0;
        }

        public bool Delete(int id)
        {
            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("dbo.usp_DeleteDepartment", cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", id);
            var pRows = new SqlParameter("@RowsAffected", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(pRows);

            cn.Open();
            cmd.ExecuteNonQuery();
            return (int)pRows.Value > 0;
        }


        // -------------------  MAPPER  -------------------
        private static Department Map(SqlDataReader r) =>
            new(
                deptId: (int)r["dept_id"],
                deptName: (string)r["dept_name"]
            );
    }
}
