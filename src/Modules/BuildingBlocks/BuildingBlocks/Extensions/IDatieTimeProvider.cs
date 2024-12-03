namespace BuildingBlocks.Extensions
{
    public interface IDatieTimeProvider
    {
        public DateTime UtcNow { get; }
    }
}