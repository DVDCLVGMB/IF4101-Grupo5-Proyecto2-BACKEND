using System;
using System.Collections.Generic;
using Steady_Management.Data;
using Steady_Management.Domain;

namespace Steady_Management.Business
{
    public class CategoryBusiness
    {
        private readonly CategoryData _data;

        public CategoryBusiness(CategoryData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public int CreateCategory(Category category)
        {
            Validate(category, isNew: true);
            return _data.Create(category);
        }

        public IEnumerable<Category> GetAllCategories()
            => _data.GetAll();

        public Category GetCategoryById(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("El ID debe ser > 0", nameof(categoryId));

            var cat = _data.GetById(categoryId);
            if (cat == null)
                throw new KeyNotFoundException($"No existe categoría con ID={categoryId}");

            return cat;
        }

        public void UpdateCategory(Category category)
        {
            Validate(category, isNew: false);
            if (!_data.Update(category))
                throw new InvalidOperationException(
                    $"No se pudo actualizar la categoría ID={category.CategoryId}");
        }

        public void DeleteCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("El ID debe ser > 0", nameof(categoryId));
            if (!_data.Delete(categoryId))
                throw new KeyNotFoundException($"No existe categoría con ID={categoryId}");
        }

        private void Validate(Category category, bool isNew)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            if (!isNew && category.CategoryId <= 0)
                throw new ArgumentException("ID inválido para actualización", nameof(category.CategoryId));
            if (string.IsNullOrWhiteSpace(category.Description))
                throw new ArgumentException("La descripción no puede estar vacía", nameof(category.Description));
        }
    }
}
