using System.Collections.Generic;
using NUnit.Framework;
using Steady_Management.DataAccess;   // InventoryData
using Steady_Management.Data;         // ProductData
using Steady_Management.Domain;       // Inventory, Product

namespace Steady_Management.Tests
{
    [TestFixture]
    public class InventoryDataTests
    {
        private string _conn;
        private InventoryData _dataAccess;
        private ProductData _productData;

        [SetUp]
        public void Setup()
        {
            // Cadena de conexión a la BD de pruebas
            _conn = "Data Source=163.178.173.130;" +
                    "Initial Catalog=PedidosSteadyManagement;" +
                    "Persist Security Info=True;" +
                    "User ID=Lenguajes;" +
                    "Password=lenguajesparaiso2025;" +
                    "TrustServerCertificate=True;";

            _dataAccess = new InventoryData(_conn);
            _productData = new ProductData(_conn);
        }

        [Test]
        public void Create_ShouldInsertInventory()
        {
            // Arrange: crear un producto para la FK
            var prodId = _productData.Create(new Product(0, 1, "TestProd", 9, true));

            var inv = new Inventory(
                1,
                productId: prodId,
                size: "M",
                itemQuantity: 5,
                limitUntilRestock: 2
            );

            // Act
            _dataAccess.Create(inv);

            // Assert
            var fetched = _dataAccess.GetByProductId(prodId);
            Assert.That(fetched, Is.Not.Null);
            Assert.That(fetched!.Size, Is.EqualTo("M"));
            Assert.That(fetched.ItemQuantity, Is.EqualTo(5));
            Assert.That(fetched.LimitUntilRestock, Is.EqualTo(2));
        }

        [Test]
        public void GetAll_ShouldReturnListOfInventories()
        {
            // Arrange
            var prodId = _productData.Create(new Product(0, 1, "ProdAll", 2, true));
            _dataAccess.Create(new Inventory(1, prodId,null, 10, 3));

            // Act
            List<Inventory> all = _dataAccess.GetAll().ToList();

            // Assert
            Assert.That(all, Is.Not.Null);
            Assert.That(all.Count, Is.GreaterThanOrEqualTo(1), "GetAll debería devolver al menos un registro");
            Assert.That(all.Exists(i => i.ProductId == prodId),
                        "La lista de inventarios no incluye el nuevo registro");
        }

        [Test]
        public void GetByProductId_Nonexistent_ReturnsNull()
        {
            // Act
            var result = _dataAccess.GetByProductId(-9999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Update_ShouldReturnTrue_WhenInventoryExists()
        {
            // Arrange
            var prodId = _productData.Create(new Product(0, 1, "ProdUpd", 3, true));
            _dataAccess.Create(new Inventory(1, prodId, "S", 7, 2));

            var toUpdate = new Inventory(1, prodId, "L", 15, 5);

            // Act
            var updated = _dataAccess.Update(toUpdate);

            // Assert
            Assert.That(updated, Is.True, "Update devolvió false");

            var fetched = _dataAccess.GetByProductId(prodId)!;
            Assert.That(fetched.Size, Is.EqualTo("L"));
            Assert.That(fetched.ItemQuantity, Is.EqualTo(15));
            Assert.That(fetched.LimitUntilRestock, Is.EqualTo(5));
        }

        [Test]
        public void Delete_ShouldReturnTrue_WhenInventoryExists()
        {
            // Arrange
            var prodId = _productData.Create(new Product(0, 1, "ProdDel", 3, true));
            _dataAccess.Create(new Inventory(2, prodId, null, 20, 10));

            // Act
            var deleted = _dataAccess.Delete(prodId);

            // Assert
            Assert.That(deleted, Is.True, "Delete devolvió false");
            var fetched = _dataAccess.GetByProductId(prodId);
            Assert.That(fetched, Is.Null);
        }
    }
}
