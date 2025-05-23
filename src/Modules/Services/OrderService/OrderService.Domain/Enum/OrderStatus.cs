namespace OrderService.Domain.Enum
{
    public enum OrderStatus : byte
    {
        Submitted = 0,
        AwaitingValidation = 1,
        StockConfirmed = 2,
        Paid = 3,
        Shipped = 4,
        Cancelled = 5
    }
}