using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using ProductService.Domain.Errors;

namespace ProductService.Domain.Entities
{
    public class ProductSpec : Entity
    {
        public ProductSpec(int id, string specName, string? specValue)
        {
            ProductId = id;
            SpecName = specName;
            SpecValue = specValue ?? string.Empty;
        }

        private ProductSpec()
        {
        }

        public int ProductId { get; private set; }
        public string SpecName { get; private set; }
        public string? SpecValue { get; private set; }

        public static Result<ProductSpec> Create(int productId, string specName, string? specValue)
        {
            if (string.IsNullOrWhiteSpace(specName))
                return Result.Failure<ProductSpec>(ProductSpecError.SpecNameInvalid);

            return new ProductSpec(productId, specName, specValue);
        }
    }
}