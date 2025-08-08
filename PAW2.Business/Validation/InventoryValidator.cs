using PAW2.Models;

namespace PAW2.Business.Validation
{
    public interface IInventoryValidator
    {
        void ValidateForSave(Inventory inventory);
        void ValidateForDelete(Inventory inventory);
        void ValidateId(int id);
    }

    public class InventoryValidator : IInventoryValidator
    {
        public void ValidateForSave(Inventory inventory)
        {
            if (inventory is null) throw new ArgumentNullException(nameof(inventory));

            if (inventory.UnitPrice < 0)
                throw new ArgumentException("UnitPrice cannot be negative.", nameof(inventory.UnitPrice));
        }

        public void ValidateForDelete(Inventory inventory)
        {
            if (inventory is null) throw new ArgumentNullException(nameof(inventory));

            if (inventory.InventoryId <= 0)
                throw new ArgumentException("InventoryID must be > 0.", nameof(inventory.InventoryId));
        }

        public void ValidateId(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be > 0.", nameof(id));
        }
    }
}
