using PAW2.Models;

namespace PAW2.Business.Validation
{
    public interface IProductValidator
    {
        void ValidateForSave(Product p);
        void ValidateForDelete(Product p);
        void ValidateId(int id);
        void ValidateBusinessFilter(string filter);
    }

    public class ProductValidator : IProductValidator
    {
        public void ValidateForSave(Product p)
        {
            if (p is null) throw new ArgumentNullException(nameof(p));
            if (string.IsNullOrWhiteSpace(p.ProductName) || p.ProductName.Length > 255)
                throw new ArgumentException("ProductName is required (max 255).", nameof(p.ProductName));

            if (p.Rating is < 0 or > 5)
                throw new ArgumentException("Rating must be between 0 and 5.", nameof(p.Rating));
        }

        public void ValidateForDelete(Product p)
        {
            if (p is null) throw new ArgumentNullException(nameof(p));
            if (p.ProductId <= 0)
                throw new ArgumentException("ProductId must be > 0.", nameof(p.ProductId));
        }

        public void ValidateId(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be > 0.", nameof(id));
        }

        public void ValidateBusinessFilter(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter) || filter.Length > 50)
                throw new ArgumentException("Filter is required (max 50).", nameof(filter));

            var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "no-supplierid", "no-inventoryid",
                "highest-rating", "lowest-rating", "most-common-rating"
            };
            if (!allowed.Contains(filter))
                throw new ArgumentException("Unsupported filter value.", nameof(filter));
        }
    }
}
