namespace BuildingBlocks.Extensions
{
    public interface IDateTimeProvider
    {
        public DateTime UtcNow { get; }
    }
}