using PAW2.Business.Validation;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Business
{
    public class ProductValidationDecorator : IBusinessProduct
    {
        private readonly IBusinessProduct _inner;
        private readonly IProductValidator _validator;

        public ProductValidationDecorator(IBusinessProduct inner, IProductValidator validator)
        {
            _inner = inner;
            _validator = validator;
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
            => _inner.GetAllProductsAsync();

        public async Task<Product> GetProductAsync(int id)
        {
            _validator.ValidateId(id);
            return await _inner.GetProductAsync(id);
        }

        public async Task<bool> SaveProductsAsync(Product product)
        {
            _validator.ValidateForSave(product);
            return await _inner.SaveProductsAsync(product);
        }

        public async Task<bool> DeleteProductAsync(Product product)
        {
            _validator.ValidateForDelete(product);
            return await _inner.DeleteProductAsync(product);
        }

        public Task<IEnumerable<ProductViewModel>> Filter(Expression<Func<Product, bool>> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            return _inner.Filter(predicate);
        }

        public async Task<IEnumerable<ProductViewModel>> FilterBusinessAsync(string filter)
        {
            _validator.ValidateBusinessFilter(filter);
            return await _inner.FilterBusinessAsync(filter);
        }
    }
}
