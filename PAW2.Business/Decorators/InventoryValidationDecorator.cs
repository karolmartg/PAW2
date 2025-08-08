using PAW2.Business.Validation;
using PAW2.Models;
using PAW2.Models.ViewModels;
using System.Linq.Expressions;

namespace PAW2.Business
{
    public class InventoryValidationDecorator : IBusinessInventory
    {
        private readonly IBusinessInventory _inner;
        private readonly IInventoryValidator _validator;

        public InventoryValidationDecorator(IBusinessInventory inner, IInventoryValidator validator)
        {
            _inner = inner;
            _validator = validator;
        }

        public Task<IEnumerable<Inventory>> GetAllInventoriesAsync()
            => _inner.GetAllInventoriesAsync();

        public async Task<Inventory> GetInventoryAsync(int id)
        {
            _validator.ValidateId(id);
            return await _inner.GetInventoryAsync(id);
        }

        public async Task<bool> SaveInventoryAsync(Inventory inventory)
        {
            _validator.ValidateForSave(inventory);
            return await _inner.SaveInventoryAsync(inventory);
        }

        public async Task<bool> DeleteInventoryAsync(Inventory inventory)
        {
            _validator.ValidateForDelete(inventory);
            return await _inner.DeleteInventoryAsync(inventory);
        }

        public Task<IEnumerable<InventoryViewModel>> Filter(Expression<Func<Inventory, bool>> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            return _inner.Filter(predicate);
        }
    }
}
