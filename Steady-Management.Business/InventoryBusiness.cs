using System;
using System.Collections.Generic;
using Steady_Management.DataAccess;
using Steady_Management.Domain;

namespace Steady_Management.Business
{
    public class InventoryBusiness
    {
        private readonly InventoryData _data;

        public InventoryBusiness(InventoryData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        /// <summary>
        /// Crea un nuevo registro de inventario.
        /// </summary>
        public void CreateInventory(Inventory inv)
        {
            if (inv == null)
                throw new ArgumentNullException(nameof(inv));

            // Podrías comprobar que ProductId existe en catálogo, size válido, etc.
            _data.Create(inv);
        }

        /// <summary>
        /// Devuelve el inventario de un producto. Lanza KeyNotFoundException si no existe.
        /// </summary>
        public Inventory GetInventoryByProductId(int productId)
        {
            var inv = _data.GetByProductId(productId);
            if (inv == null)
                throw new KeyNotFoundException($"No existe inventario para el producto {productId}.");
            return inv;
        }

        /// <summary>
        /// Devuelve todos los registros de inventario.
        /// </summary>
        public IEnumerable<Inventory> GetAllInventories()
        {
            return _data.GetAll();
        }

        /// <summary>
        /// Actualiza un inventario existente. Lanza KeyNotFoundException si no existe.
        /// </summary>
        public void UpdateInventory(Inventory inv)
        {
            if (inv == null)
                throw new ArgumentNullException(nameof(inv));

            // Verificamos primero que exista
            var existing = _data.GetByProductId(inv.ProductId);
            if (existing == null)
                throw new KeyNotFoundException($"No existe inventario para el producto {inv.ProductId}.");

            var updated = _data.Update(inv);
            if (!updated)
                throw new InvalidOperationException($"La actualización del inventario de producto {inv.ProductId} no afectó ninguna fila.");
        }

        /// <summary>
        /// Elimina el inventario de un producto. Lanza KeyNotFoundException si no existe.
        /// </summary>
        public void DeleteInventory(int productId)
        {
            // Verificamos primero que exista
            var existing = _data.GetByProductId(productId);
            if (existing == null)
                throw new KeyNotFoundException($"No existe inventario para el producto {productId}.");

            var deleted = _data.Delete(productId);
            if (!deleted)
                throw new InvalidOperationException($"La eliminación del inventario de producto {productId} no afectó ninguna fila.");
        }
    }
}
