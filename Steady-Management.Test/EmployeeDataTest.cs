using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Steady_Management.DataAccess;
using Steady_Management.Domain;

namespace Steady_Management.Test.Integration
{
    [TestFixture]
    public class EmployeeDataRealTests
    {
        private EmployeeData _employeeData;
        private readonly List<int> _createdEmployeeIds = new();
        private const string ConnectionString = "Data Source=163.178.173.130;Initial Catalog=PedidosSteadyManagement;Persist Security Info=True;User ID=Lenguajes;Password=lenguajesparaiso2025;Trust Server Certificate=True;";

        [OneTimeSetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", ConnectionString)
                })
                .Build();

            _employeeData = new EmployeeData(configuration);
        }

        [TearDown]
        public async Task Cleanup()
        {
            foreach (var id in _createdEmployeeIds)
            {
                await DeleteEmployeeReal(id);
            }
            _createdEmployeeIds.Clear();
        }

        private async Task DeleteEmployeeReal(int employeeId)
        {
            using var conn = new SqlConnection(ConnectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand("DELETE FROM Employee WHERE employee_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", employeeId);
            await cmd.ExecuteNonQueryAsync();
        }

        private async Task<(int deptId, int roleId)> GetValidDepartmentAndRoleIds()
        {
            using var conn = new SqlConnection(ConnectionString);
            await conn.OpenAsync();

            using var deptCmd = new SqlCommand("SELECT TOP 1 dept_id FROM Department", conn);
            var deptId = (int)await deptCmd.ExecuteScalarAsync();

            using var roleCmd = new SqlCommand("SELECT TOP 1 role_id FROM Role", conn);
            var roleId = (int)await roleCmd.ExecuteScalarAsync();

            return (deptId, roleId);
        }

        private async Task<(int deptId, int roleId)> GetDifferentValidIds(int currentDeptId, int currentRoleId)
        {
            using var conn = new SqlConnection(ConnectionString);
            await conn.OpenAsync();

            using var deptCmd = new SqlCommand("SELECT TOP 1 dept_id FROM Department WHERE dept_id <> @currentId", conn);
            deptCmd.Parameters.AddWithValue("@currentId", currentDeptId);
            var newDeptId = (int)await deptCmd.ExecuteScalarAsync();

            using var roleCmd = new SqlCommand("SELECT TOP 1 role_id FROM Role WHERE role_id <> @currentId", conn);
            roleCmd.Parameters.AddWithValue("@currentId", currentRoleId);
            var newRoleId = (int)await roleCmd.ExecuteScalarAsync();

            return (newDeptId, newRoleId);
        }

        [Test]
        public async Task CreateEmployee_SavesCorrectly()
        {
            var (deptId, roleId) = await GetValidDepartmentAndRoleIds();

            var employee = new Employee(0, deptId, roleId, "Ext101", "EmpleadoCreado", "Apellido", "1234-5678");

            int newId = await _employeeData.CreateEmployeeAsync(employee);
            _createdEmployeeIds.Add(newId);

            var created = await _employeeData.GetEmployeeByIdAsync(newId);

            Assert.That(created, Is.Not.Null);
            Assert.That(created.EmployeeName, Is.EqualTo("EmpleadoCreado"));
            Assert.That(created.DeptId, Is.EqualTo(deptId));
            Assert.That(created.RoleId, Is.EqualTo(roleId));
        }

        [Test]
        public async Task UpdateEmployee_ChangesData()
        {
            var (deptId, roleId) = await GetValidDepartmentAndRoleIds();

            var employee = new Employee(0, deptId, roleId, "Ext102", "EmpleadoActualizar", "Apellido", "2222-2222");

            int newId = await _employeeData.CreateEmployeeAsync(employee);
            _createdEmployeeIds.Add(newId);

            var toUpdate = await _employeeData.GetEmployeeByIdAsync(newId);

            var (newDeptId, newRoleId) = await GetDifferentValidIds(deptId, roleId);

            toUpdate.EmployeeName = "Actualizado";
            toUpdate.DeptId = newDeptId;
            toUpdate.RoleId = newRoleId;

            await _employeeData.UpdateEmployeeAsync(toUpdate);

            var updated = await _employeeData.GetEmployeeByIdAsync(newId);

            Assert.That(updated.EmployeeName, Is.EqualTo("Actualizado"));
            Assert.That(updated.DeptId, Is.EqualTo(newDeptId));
            Assert.That(updated.RoleId, Is.EqualTo(newRoleId));
        }

        [Test]
        public async Task DeleteEmployee_RemovesData()
        {
            var (deptId, roleId) = await GetValidDepartmentAndRoleIds();

            var employee = new Employee(0, deptId, roleId, "Ext103", "EmpleadoEliminar", "Apellido", "9999-9999");

            int newId = await _employeeData.CreateEmployeeAsync(employee);

            await _employeeData.DeleteEmployeeAsync(newId);

            var deleted = await _employeeData.GetEmployeeByIdAsync(newId);

            Assert.That(deleted, Is.Null);
        }

        [Test]
        public async Task GetAllEmployees_ReturnsData_FromRealDatabase()
        {
            var result = await _employeeData.GetAllEmployeesAsync();

            // Si no hay empleados, inserta uno para validar el test
            if (!result.Any())
            {
                var (deptId, roleId) = await GetValidDepartmentAndRoleIds();
                var employee = new Employee(0, deptId, roleId, "Ext104", "EmpleadoDummy", "Apellido", "0000-0000");

                int newId = await _employeeData.CreateEmployeeAsync(employee);
                _createdEmployeeIds.Add(newId);

                result = await _employeeData.GetAllEmployeesAsync();
            }

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Any(), Is.True);
        }

        [Test]
        public void CreateEmployee_WithInvalidDepartment_ThrowsRealException()
        {
            var invalidEmployee = new Employee(
                0,
                999999, // Departamento inexistente
                1,      // Rol válido (asumiendo que existe)
                "Ext999",
                "Invalido",
                "Departamento",
                "0000-0000");

            Assert.ThrowsAsync<SqlException>(async () =>
                await _employeeData.CreateEmployeeAsync(invalidEmployee));
        }
    }
}
