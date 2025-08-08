using PAW2.Business.Validation;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PAW2.Business
{
    public class CatalogValidationDecorator : IBusinessCatalog
    {
        private readonly IBusinessCatalog _inner;
        private readonly ICatalogValidator _validator;

        public CatalogValidationDecorator(IBusinessCatalog inner, ICatalogValidator validator)
        {
            _inner = inner;
            _validator = validator;
        }

        public Task<IEnumerable<Catalog>> GetAllCatalogsAsync() => _inner.GetAllCatalogsAsync();

        public async Task<Catalog> GetCatalogAsync(int id)
        {
            _validator.ValidateId(id);
            return await _inner.GetCatalogAsync(id);
        }

        public async Task<bool> SaveCatalogAsync(Catalog catalog)
        {
            _validator.ValidateForSave(catalog);
            return await _inner.SaveCatalogAsync(catalog);
        }

        public async Task<bool> DeleteCatalogAsync(Catalog catalog)
        {
            _validator.ValidateForDelete(catalog);
            return await _inner.DeleteCatalogAsync(catalog);
        }

        public Task<IEnumerable<CatalogViewModel>> Filter(Expression<Func<Catalog, bool>> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            return _inner.Filter(predicate);
        }
    }
}
