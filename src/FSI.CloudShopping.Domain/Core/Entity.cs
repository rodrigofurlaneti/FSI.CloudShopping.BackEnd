namespace FSI.CloudShopping.Domain.Core
{
    public abstract class Entity
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;

        public override bool Equals(object? obj)
        {
            var compareTo = obj as Entity;
            if (ReferenceEquals(this, compareTo)) return true;
            if (compareTo is null) return false;
            return Id.Equals(compareTo.Id);
        }

        public override int GetHashCode() => (GetType().GetHashCode() * 907) ^ Id.GetHashCode();
    }
}
