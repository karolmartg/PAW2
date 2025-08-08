using PAW2.Models;

namespace PAW2.Business.Validation
{
    public interface ICategoryValidator
    {
        void ValidateForSave(Category category);
        void ValidateForDelete(Category category);
        void ValidateId(int id);
    }

    public class CategoryValidator : ICategoryValidator
    {
        public void ValidateForSave(Category category)
        {
            if (category is null) throw new ArgumentNullException(nameof(category));
            
            if (string.IsNullOrWhiteSpace(category.CategoryName) || category.CategoryName.Length > 255)
                throw new ArgumentException("CategoryName is required (max 255).", nameof(category.CategoryName));
        }

        public void ValidateForDelete(Category category)
        {
            if (category is null) throw new ArgumentNullException(nameof(category));
            
            if (category.CategoryId <= 0)
                throw new ArgumentException("CategoryID must be > 0.", nameof(category.CategoryId));
        }

        public void ValidateId(int id)
        {
            if (id <= 0) throw new ArgumentException("Id must be > 0.", nameof(id));
        }
    }
}
