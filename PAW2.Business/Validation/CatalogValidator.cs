using Microsoft.Identity.Client;
using PAW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAW2.Business.Validation
{
    public interface ICatalogValidator
    {
        void ValidateForSave(Catalog catalog);
        void ValidateForDelete(Catalog catalog);
        void ValidateId(int id);
    }

    public class CatalogValidator : ICatalogValidator
    {
        public void ValidateForSave(Catalog catalog)
        {
            if (catalog is null) throw new ArgumentNullException(nameof(catalog));

            if (string.IsNullOrWhiteSpace(catalog.Name)) throw new ArgumentException("Name is required", nameof(catalog.Name));

            if (catalog.Name.Length > 50) throw new ArgumentException("Name max length is 50", nameof(catalog.Name));

            if (catalog.Rating is < 0 or > 5) throw new ArgumentException("Raiting must be between 0 and 5", nameof(catalog.Rating));
        }

        public void ValidateForDelete(Catalog catalog)
        {
            if (catalog is null) throw new ArgumentNullException(nameof(catalog));

            if (catalog.Identifier <= 0) throw new ArgumentException("Identifier must be > 0", nameof(catalog.Identifier));
        }

        public void ValidateId(int id)
        {
            if (id <= 0) throw new ArgumentException("Identifier must be > 0", nameof(id));
        }
    }
}
