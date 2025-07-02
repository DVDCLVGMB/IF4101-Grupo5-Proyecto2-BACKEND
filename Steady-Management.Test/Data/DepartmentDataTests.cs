using NUnit.Framework;
using Steady_Management.DataAccess;
using Steady_Management.Domain;
using System;
using System.Collections.Generic;

namespace Steady_Management.Tests
{
    [TestFixture]
    public class DepartmentDataTests
    {
        private string _conn;
        private DepartmentData _dataAccess;

        [SetUp]
        public void Setup()
        {
            // Usa aquí una cadena de conexión válida a tu base de datos de pruebas
            _conn = "Data Source=163.178.173.130;Initial Catalog=PedidosSteadyManagement;Persist Security Info=True;User ID=Lenguajes;Password=lenguajesparaiso2025;Trust Server Certificate=True;";
            _dataAccess = new DepartmentData(_conn);
        }

        [Test]
        public void Create_ShouldAssignId_WhenExecutedSuccessfully()
        {
            // Arrange
            var department = new Department { DeptName = "Test Department" };

            // Act
            _dataAccess.Create(department);

            // Assert
            Assert.That(department.DeptId, Is.GreaterThan(0));
        }

        [Test]
        public void GetAll_ShouldReturnListOfDepartments()
        {
            // Act
            List<Department> departments = _dataAccess.GetAll();

            // Assert
            Assert.That(departments, Is.Not.Null);
            Assert.That(departments.Count, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void GetById_ShouldReturnCorrectDepartment()
        {
            // Arrange
            var department = new Department { DeptName = "Temp Department" };
            _dataAccess.Create(department);

            // Act
            var found = _dataAccess.GetById(department.DeptId);

            // Assert
            Assert.That(found, Is.Not.Null);
            Assert.That(found!.DeptId, Is.EqualTo(department.DeptId));
            Assert.That(found.DeptName, Is.EqualTo("Temp Department"));
        }

        [Test]
        public void Update_ShouldReturnTrue_WhenDepartmentExists()
        {
            // Arrange
            var department = new Department { DeptName = "Original Name" };
            _dataAccess.Create(department);

            // Act
            department.DeptName = "Updated Name";
            var updated = _dataAccess.Update(department);

            // Assert
            Assert.That(updated, Is.True);

            var refreshed = _dataAccess.GetById(department.DeptId);
            Assert.That(refreshed!.DeptName, Is.EqualTo("Updated Name"));
        }

        [Test]
        public void Delete_ShouldReturnTrue_WhenDepartmentExists()
        {
            // Arrange
            var department = new Department { DeptName = "To Delete" };
            _dataAccess.Create(department);

            // Act
            var deleted = _dataAccess.Delete(department.DeptId);

            // Assert
            Assert.That(deleted, Is.True);
            var result = _dataAccess.GetById(department.DeptId);
            Assert.That(result, Is.Null);
        }
    }
}
