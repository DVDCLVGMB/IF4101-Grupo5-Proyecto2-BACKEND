using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Steady_Management.Domain;
using Microsoft.Extensions.Configuration;


namespace Steady_Management.DataAccess
{
    public class EmployeeData
    {
        private readonly string _connectionString;

        public EmployeeData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            var employees = new List<Employee>();

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            SqlCommand cmd = new("usp_Employee_Read", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var employee = new Employee(
                    employeeId: reader.GetInt32(reader.GetOrdinal("employee_id")),
                    deptId: reader.GetInt32(reader.GetOrdinal("dept_id")),
                    roleId: reader.GetInt32(reader.GetOrdinal("role_id")),
                    extension: reader.GetString(reader.GetOrdinal("extension")),
                    employeeName: reader.GetString(reader.GetOrdinal("employee_name")),
                    employeeSurname: reader.GetString(reader.GetOrdinal("employee_surname")),
                    workPhoneNumber: reader.GetString(reader.GetOrdinal("work_phone_number"))
                );

                employees.Add(employee);
            }

            return employees;
        }


        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            SqlCommand cmd = new("usp_Employee_Read", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@employee_id", employeeId);

            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Employee(
                    employeeId: reader.GetInt32(reader.GetOrdinal("employee_id")),
                    deptId: reader.GetInt32(reader.GetOrdinal("dept_id")),
                    roleId: reader.GetInt32(reader.GetOrdinal("role_id")),
                    extension: reader.GetString(reader.GetOrdinal("extension")),
                    employeeName: reader.GetString(reader.GetOrdinal("employee_name")),
                    employeeSurname: reader.GetString(reader.GetOrdinal("employee_surname")),
                    workPhoneNumber: reader.GetString(reader.GetOrdinal("work_phone_number"))
                );
            }

            return null;
        }


        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            SqlCommand cmd = new("usp_Employee_Create", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@dept_id", employee.DeptId);
            cmd.Parameters.AddWithValue("@role_id", employee.RoleId);
            cmd.Parameters.AddWithValue("@extension", employee.Extension);
            cmd.Parameters.AddWithValue("@employee_name", employee.EmployeeName);
            cmd.Parameters.AddWithValue("@employee_surname", employee.EmployeeSurname);
            cmd.Parameters.AddWithValue("@work_phone_number", employee.WorkPhoneNumber);

            object result = await cmd.ExecuteScalarAsync();
            return result != null ? Convert.ToInt32(result) : -1;
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            SqlCommand cmd = new("usp_Employee_Update", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@employee_id", employee.EmployeeId);
            cmd.Parameters.AddWithValue("@dept_id", employee.DeptId);
            cmd.Parameters.AddWithValue("@role_id", employee.RoleId);
            cmd.Parameters.AddWithValue("@extension", employee.Extension);
            cmd.Parameters.AddWithValue("@employee_name", employee.EmployeeName);
            cmd.Parameters.AddWithValue("@employee_surname", employee.EmployeeSurname);
            cmd.Parameters.AddWithValue("@work_phone_number", employee.WorkPhoneNumber);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            SqlCommand cmd = new("usp_Employee_Delete", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@employee_id", employeeId);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}

