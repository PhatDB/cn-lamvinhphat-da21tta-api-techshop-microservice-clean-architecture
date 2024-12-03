namespace CatalogService.Domain.ValueObjects
{
    public record ProductDetails
    {
        private ProductDetails(string? description, string? brand, string? model)
        {
            Description = description;
            Brand = brand;
            Model = model;
        }

        protected ProductDetails()
        {
        }

        public string? Description { get; }
        public string? Brand { get; }
        public string? Model { get; }

        public static ProductDetails Create(string? description, string? brand, string? model)
        {
            description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            brand = string.IsNullOrWhiteSpace(brand) ? null : brand.Trim();
            model = string.IsNullOrWhiteSpace(model) ? null : model.Trim();

            return new ProductDetails(description, brand, model);
        }
    }
}