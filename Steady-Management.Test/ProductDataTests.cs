using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
using Steady_Management.Data;
using Steady_Management.Domain;

namespace Steady_Management.Test
{
    [TestFixture]
    public class ProductDataTests
    {
        private ProductData productData;
        private readonly List<int> _createdProductIds = new();
        private int _testCategoryId;
        private const string ConnectionString =
            "Data Source=163.178.173.130;Initial Catalog=PedidosSteadyManagement;Persist Security Info=True;User ID=Lenguajes;Password=lenguajesparaiso2025;Trust Server Certificate=True;";

        [SetUp]
        public void Setup()
        {
            productData = new ProductData(ConnectionString);

            // crear una categoría de prueba directamente por ADO.NET
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(
                @"INSERT INTO Category (description) 
                  OUTPUT INSERTED.category_id 
                  VALUES (@description);", conn);
            cmd.Parameters.AddWithValue("@description", "CAT_PRUEBA_TEST");
            conn.Open();
            _testCategoryId = Convert.ToInt32(cmd.ExecuteScalar());
        }

        [TearDown]
        public void TearDown()
        {
            // Borramos todos los productos que creamos
            foreach (var id in _createdProductIds)
                productData.Delete(id);
            _createdProductIds.Clear();

            // Borramos la categoría de prueba
            using var conn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(
                "DELETE FROM Category WHERE category_id = @id;", conn);
            cmd.Parameters.AddWithValue("@id", _testCategoryId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        [Test]
        public void CreateReadUpdateDelete_Product_Succeeds()
        {
            // Creamos el producto apuntando a la categoría que acabamos de insertar
            var product = new Product(0, _testCategoryId, "TestProduct", 9.99f);

            // CREATE
            int newId = productData.Create(product);
            Assert.That(newId, Is.GreaterThan(0), "El ID generado debe ser > 0");
            _createdProductIds.Add(newId);

            // READ
            var fromDb = productData.GetById(newId);
            Assert.That(fromDb, Is.Not.Null, "No se encontró el producto creado");
            Assert.That(fromDb!.ProductName, Is.EqualTo("TestProduct"));
            Assert.That(fromDb.Price, Is.EqualTo(9.99f));

            // UPDATE
            fromDb.ProductName = "UpdatedProduct";
            fromDb.Price = 19.95f;
            bool updated = productData.Update(fromDb);
            Assert.That(updated, Is.True, "La actualización debería afectar > 0 filas");

            var updatedDb = productData.GetById(newId)!;
            Assert.That(updatedDb.ProductName, Is.EqualTo("UpdatedProduct"));
            Assert.That(updatedDb.Price, Is.EqualTo(19.95f));

            // DELETE
            bool deleted = productData.Delete(newId);
            Assert.That(deleted, Is.True, "La eliminación debería afectar > 0 filas");
            _createdProductIds.Remove(newId);

            var deletedDb = productData.GetById(newId);
            Assert.That(deletedDb, Is.Null, "El producto debería haber sido eliminado");
        }

        [Test]
        public void GetAll_Returns_Collection_IncludesNewProduct()
        {
            var product = new Product(0, _testCategoryId, "ListTestProd", 5.50f);
            int newId = productData.Create(product);
            Assert.That(newId, Is.GreaterThan(0));
            _createdProductIds.Add(newId);

            bool found = false;
            foreach (var p in productData.GetAll())
            {
                if (p.ProductId == newId)
                {
                    found = true;
                    break;
                }
            }

            Assert.That(found, Is.True, "La lista debe incluir el producto recién creado");
        }
    }
}
