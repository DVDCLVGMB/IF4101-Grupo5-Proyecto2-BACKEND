using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Steady_Management.Domain;

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
            const string sql = @"
                INSERT INTO Department (dept_name)
                VALUES (@Name);
                SELECT SCOPE_IDENTITY();";

            using var cn = new SqlConnection(_conn);
            cn.Open();

            using var tx = cn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(sql, cn, tx);
                cmd.Parameters.AddWithValue("@Name", d.DeptName);

                // Recupera el ID generado
                d.DeptId = Convert.ToInt32(cmd.ExecuteScalar());

                tx.Commit();
            }
            catch (SqlException ex)
            {
                tx.Rollback();
                // Aquí podrías registrar el error en un logger antes de relanzar
                throw new Exception("Error al crear el departamento.", ex);
            }
        }

        // -------------------  READ ALL  -----------------
        public List<Department> GetAll()
        {
            const string sql = "SELECT * FROM Department";
            var list = new List<Department>();

            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(sql, cn);
            cn.Open();
            using var rd = cmd.ExecuteReader();

            while (rd.Read())
                list.Add(Map(rd));

            return list;
        }

        // -------------------  READ BY ID  ---------------
        public Department? GetById(int id)
        {
            const string sql = "SELECT * FROM Department WHERE dept_id = @Id";

            using var cn = new SqlConnection(_conn);
            using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@Id", id);
            cn.Open();

            using var rd = cmd.ExecuteReader();
            return rd.Read() ? Map(rd) : null;
        }

        // -------------------  UPDATE  -------------------
        public bool Update(Department d)
        {
            const string sql = @"
                UPDATE Department
                   SET dept_name = @Name
                 WHERE dept_id   = @Id";

            using var cn = new SqlConnection(_conn);
            cn.Open();

            using var tx = cn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(sql, cn, tx);
                cmd.Parameters.AddWithValue("@Name", d.DeptName);
                cmd.Parameters.AddWithValue("@Id", d.DeptId);

                var rows = cmd.ExecuteNonQuery();
                tx.Commit();
                return rows > 0;
            }
            catch (SqlException ex)
            {
                tx.Rollback();
                throw new Exception("Error al actualizar el departamento.", ex);
            }
        }

        // -------------------  DELETE  -------------------
        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Department WHERE dept_id = @Id";

            using var cn = new SqlConnection(_conn);
            cn.Open();

            using var tx = cn.BeginTransaction();
            try
            {
                using var cmd = new SqlCommand(sql, cn, tx);
                cmd.Parameters.AddWithValue("@Id", id);

                var rows = cmd.ExecuteNonQuery();
                tx.Commit();
                return rows > 0;
            }
            catch (SqlException ex)
            {
                tx.Rollback();
                throw new Exception("Error al eliminar el departamento.", ex);
            }
        }

        // -------------------  MAPPER  -------------------
        private static Department Map(SqlDataReader r) =>
            new(
                deptId: (int)r["dept_id"],
                deptName: (string)r["dept_name"]
            );
    }
}
