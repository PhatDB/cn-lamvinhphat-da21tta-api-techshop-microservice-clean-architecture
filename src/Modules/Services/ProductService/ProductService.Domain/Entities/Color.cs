using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Error;
using BuildingBlocks.Results;

namespace ProductService.Domain.Entities
{
    public class Color : Entity, IAggregateRoot
    {
        private Color(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public static Result<Color> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Color>(Error.Validation("Color.EmptyName",
                    "Color name cannot be empty."));

            return Result.Success(new Color(name));
        }
    }
}