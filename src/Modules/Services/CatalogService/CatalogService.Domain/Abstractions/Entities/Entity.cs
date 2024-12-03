namespace CatalogService.Domain.Abstractions.Entities
{
    public abstract class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            Entity<T> entity = (Entity<T>)obj;
            return EqualityComparer<T>.Default.Equals(entity.Id, Id);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Id);
        }
    }
}