using System;
using System.Collections.Generic;
using Steady_Management.Domain;
using Steady_Management.Data;

namespace Steady_Management.Business
{
    /// <summary>
    /// Capa de negocio para operaciones relacionadas con Productos.
    /// </summary>
    public class ProductBusiness
    {
        private readonly ProductData _data;

        public ProductBusiness(ProductData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// Crea un nuevo producto tras validar sus datos.
        /// </summary>
        public int CreateProduct(Product product)
        {
            ValidateProduct(product, isNew: true);
            return _data.Create(product);
        }

        /// <summary>
        /// Obtiene todos los productos.
        /// </summary>
        public IEnumerable<Product> GetAllProducts()
        {
            return _data.GetAll();
        }

        /// <summary>
        /// Obtiene un producto por su ID. Lanza excepción si no existe.
        /// </summary>
        public Product GetProductById(int productId)
        {
            if (productId <= 0)
                throw new ArgumentException("El ID de producto debe ser mayor que cero.", nameof(productId));

            var product = _data.GetById(productId);
            if (product == null)
                throw new KeyNotFoundException($"No se encontró un producto con ID={productId}.");

            return product;
        }

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        public void UpdateProduct(Product product)
        {
            ValidateProduct(product, isNew: false);

            bool updated = _data.Update(product);
            if (!updated)
                throw new InvalidOperationException($"No se pudo actualizar el producto con ID={product.ProductId}.");
        }

        /// <summary>
        /// Elimina un producto por su ID.
        /// </summary>
        public void DeleteProduct(int productId)
        {
            if (productId <= 0)
                throw new ArgumentException("El ID de producto debe ser mayor que cero.", nameof(productId));

            bool deleted = _data.Delete(productId);
            if (!deleted)
                throw new KeyNotFoundException($"No se encontró el producto con ID={productId} para eliminar.");
        }

        /// <summary>
        /// Valida los datos de un producto según reglas de negocio.
        /// </summary>
        private void ValidateProduct(Product product, bool isNew)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (!isNew && product.ProductId <= 0)
                throw new ArgumentException("El ID de producto debe ser mayor que cero para actualizar.", nameof(product.ProductId));

            if (product.CategoryId <= 0)
                throw new ArgumentException("El ID de categoría debe ser mayor que cero.", nameof(product.CategoryId));

            if (string.IsNullOrWhiteSpace(product.ProductName))
                throw new ArgumentException("El nombre del producto no puede estar vacío.", nameof(product.ProductName));

            if (product.Price < 0)
                throw new ArgumentException("El precio del producto no puede ser negativo.", nameof(product.Price));
        }
    }
}
