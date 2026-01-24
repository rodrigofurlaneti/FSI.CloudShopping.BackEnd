using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Entities
{
    public class Individual : Entity
    {
        public TaxId TaxId { get; private set; } 
        public PersonName FullName { get; private set; } 
        public DateTime? BirthDate { get; private set; }
        public virtual Customer Customer { get; private set; }
        protected Individual() { }
        public Individual(int customerId, TaxId taxId, PersonName fullName, DateTime? birthDate)
        {
            Id = customerId;
            TaxId = taxId;
            FullName = fullName;
            BirthDate = birthDate;
        }
        public bool IsAdult()
        {
            if (!BirthDate.HasValue) return false;
            return BirthDate.Value.AddYears(18) <= DateTime.Today;
        }
        public void UpdateName(PersonName newName) => FullName = newName;
    }
}