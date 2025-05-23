namespace BuildingBlocks.Contracts.Customers
{
    public record GetProductsRatingRespone(List<RatingRespone> Ratings);

    public record RatingRespone(int ProductId, byte Rating);
}