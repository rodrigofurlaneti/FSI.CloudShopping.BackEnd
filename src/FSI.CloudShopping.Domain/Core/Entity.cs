namespace FSI.CloudShopping.Domain.Core
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime UpdatedAt { get; protected set; } = DateTime.Now;
        internal void SetId(int id) => Id = id;
        public override bool Equals(object? obj)
        {
            var compareTo = obj as Entity;
            if (ReferenceEquals(this, compareTo)) return true;
            if (compareTo is null) return false;
            return Id.Equals(compareTo.Id);
        }
        public override int GetHashCode() => (GetType().GetHashCode() * 907) ^ Id.GetHashCode();
        public void Touch() => UpdatedAt = DateTime.Now;
    }
}
