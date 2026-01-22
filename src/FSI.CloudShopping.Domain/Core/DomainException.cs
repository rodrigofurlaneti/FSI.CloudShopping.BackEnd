namespace FSI.CloudShopping.Domain.Core
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}
