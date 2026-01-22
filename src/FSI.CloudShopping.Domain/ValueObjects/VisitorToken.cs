using FSI.CloudShopping.Domain.Core;
namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record VisitorToken
    {
        public Guid Value { get; }
        public VisitorToken(Guid value)
        {
            if (value == Guid.Empty)
                throw new DomainException("Visitor token cannot be empty.");
            Value = value;
        }
        public static VisitorToken NewToken() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}