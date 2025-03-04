using BuildingBlocks.Abstractions.Aggregates;
using BuildingBlocks.Abstractions.Entities;
using BuildingBlocks.Results;
using ProductService.Domain.Errors;

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
                return Result.Failure<Color>(ColorError.ColorInvalidName);

            return Result.Success(new Color(name));
        }
    }
}