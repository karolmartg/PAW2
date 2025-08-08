using PAW2.Models;

namespace PAW2.Business.Validation
{
    public interface ISupplierValidator
    {
        void ValidateForSave(Supplier s);
        void ValidateForDelete(Supplier s);
        void ValidateId(int id);
    }

    public class SupplierValidator : ISupplierValidator
    {
        public void ValidateForSave(Supplier s)
        {
            if (s is null) throw new ArgumentNullException(nameof(s));
            if (string.IsNullOrWhiteSpace(s.SupplierName) || s.SupplierName.Length > 255)
                throw new ArgumentException("SupplierName is required (max 255).", nameof(s.SupplierName));
        }

        public void ValidateForDelete(Supplier s)
        {
            if (s is null) throw new ArgumentNullException(nameof(s));
            if (s.SupplierId <= 0)
                throw new ArgumentException("SupplierId must be > 0.", nameof(s.SupplierId));
        }

        public void ValidateId(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be > 0.", nameof(id));
        }
    }
}
