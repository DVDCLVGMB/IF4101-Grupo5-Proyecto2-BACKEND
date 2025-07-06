using System;
using System.Collections.Generic;
using Steady_Management.Data;
using Steady_Management.DataAccess;
using Steady_Management.Domain;

namespace Steady_Management.Business
{
    public class InventoryBusiness
    {
        private readonly InventoryData _data;
        private readonly ProductData _productData;

        public InventoryBusiness(InventoryData inventoryData,
                                 ProductData productData)
        {
            _data = inventoryData
                ?? throw new ArgumentNullException(nameof(inventoryData));
            _productData = productData
                ?? throw new ArgumentNullException(nameof(productData));
        }

        /// <summary>
        /// Crea un nuevo registro de inventario.
        /// </summary>
        public Inventory CreateInventory(Inventory inv)
        {
            if (inv == null) throw new ArgumentNullException(nameof(inv));

            // lanza el INSERT y captura el identity
            int newId = _data.Create(inv);
            inv.InventoryId = newId;
            return inv;
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

        public Inventory GetById(int inventoryId)
        {
            var inv = _data.GetById(inventoryId);
            if (inv == null)
                throw new KeyNotFoundException($"No existe inventario con ID {inventoryId}.");
            return inv;
        }

        public void UpdateInventory(Inventory inv)
        {
            // 1) Me aseguro que exista ese inventoryId
            GetById(inv.InventoryId);
            // 2) Aplico el UPDATE
            _data.Update(inv);
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

        public void DeleteInventoryById(int inventoryId)
        {
            // 1) verifico que exista ese registro
            var inv = _data.GetById(inventoryId);
            if (inv == null)
                throw new KeyNotFoundException($"No existe inventario con ID {inventoryId}.");

            // 2) borro por PK
            _data.DeleteById(inventoryId);
        }


    }
}
