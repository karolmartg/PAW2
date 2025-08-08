using PAW2.Business.Validation;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Business
{
    public class CategoryValidationDecorator : IBusinessCategory
    {
        private readonly IBusinessCategory _inner;
        private readonly ICategoryValidator _validator;

        public CategoryValidationDecorator(IBusinessCategory inner, ICategoryValidator validator)
        {
            _inner = inner;
            _validator = validator;
        }

        public Task<IEnumerable<Category>> GetAllCategoriesAsync()
            => _inner.GetAllCategoriesAsync();

        public async Task<Category> GetCategoryAsync(int id)
        {
            _validator.ValidateId(id);
            return await _inner.GetCategoryAsync(id);
        }

        public async Task<bool> SaveCategoryAsync(Category category)
        {
            _validator.ValidateForSave(category);
            return await _inner.SaveCategoryAsync(category);
        }

        public async Task<bool> DeleteCategoryAsync(Category category)
        {
            _validator.ValidateForDelete(category);
            return await _inner.DeleteCategoryAsync(category);
        }

        public Task<IEnumerable<CategoryViewModel>> Filter(Expression<Func<Category, bool>> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            return _inner.Filter(predicate);
        }

        public async Task<IEnumerable<CategoryViewModel>> FilterBusinessAsync(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter) || filter.Length > 255)
                throw new ArgumentException("Filter is required (max 255).", nameof(filter));

            return await _inner.FilterBusinessAsync(filter);
        }
    }
}
