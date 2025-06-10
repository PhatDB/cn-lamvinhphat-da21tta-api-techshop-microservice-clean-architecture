namespace BuildingBlocks.Contracts.Customers
{
    public record GetProductsRatingResponse(List<RatingRespone> Ratings);

    public record RatingRespone(int ProductId, byte Rating);
}