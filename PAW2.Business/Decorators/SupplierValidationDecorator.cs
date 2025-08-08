using PAW2.Business.Validation;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Business
{
    public class SupplierValidationDecorator : IBusinessSupplier
    {
        private readonly IBusinessSupplier _inner;
        private readonly ISupplierValidator _validator;

        public SupplierValidationDecorator(IBusinessSupplier inner, ISupplierValidator validator)
        {
            _inner = inner;
            _validator = validator;
        }

        public Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
            => _inner.GetAllSuppliersAsync();

        public async Task<Supplier> GetSupplierAsync(int id)
        {
            _validator.ValidateId(id);
            return await _inner.GetSupplierAsync(id);
        }

        public async Task<bool> SaveSupplierAsync(Supplier supplier)
        {
            _validator.ValidateForSave(supplier);
            return await _inner.SaveSupplierAsync(supplier);
        }

        public async Task<bool> DeleteSupplierAsync(Supplier supplier)
        {
            _validator.ValidateForDelete(supplier);
            return await _inner.DeleteSupplierAsync(supplier);
        }

        public Task<IEnumerable<SupplierViewModel>> Filter(Expression<Func<Supplier, bool>> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            return _inner.Filter(predicate);
        }
    }
}
