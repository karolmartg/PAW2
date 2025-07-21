using PAW2.Models;
using PAW2.Mvc.Models;

namespace PAW2.Mvc.Helper.Converters
{
    public class Converter
    {
        public static CatalogViewModel ToCatalogViewModel(Catalog catalog)
        {
            return new CatalogViewModel
            {
                Id = catalog.Identifier,
                Name = catalog.Name
            };
        }
    }
}
