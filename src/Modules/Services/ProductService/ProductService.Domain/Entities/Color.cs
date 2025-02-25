using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using BuildingBlocks.Error;

namespace ProductService.Domain.Entities
{
    public class Color : Entity
    {
        public string Name { get; private set; }

        private Color(string name)
        {
            Name = name;
        }

        public static Result<Color> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Color>(Error.Validation("Color.EmptyName", "Color name cannot be empty."));

            return Result.Success(new Color(name));
        }
    }
}