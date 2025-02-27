using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using BuildingBlocks.Error;

namespace ProductService.Domain.Entities
{
    public class Color : Entity
    {
        private readonly List<ProductColor> _productColors;
        public string Name { get; private set; }
        public IReadOnlyCollection<ProductColor> ProductColors => _productColors.AsReadOnly();

        private Color(string name)
        {
            Name = name;
            _productColors = new List<ProductColor>();
        }

        public static Result<Color> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Color>(Error.Validation("Color.EmptyName", "Color name cannot be empty."));

            return Result.Success(new Color(name));
        }
    }
}