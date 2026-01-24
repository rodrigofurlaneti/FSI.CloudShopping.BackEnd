using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Domain.Entities
{
    public class Customer : Entity
    {
        public Email? Email { get; private set; }
        public Password? Password { get; private set; }
        public CustomerType CustomerType { get; private set; }
        public Guid SessionToken { get; private set; }
        public bool IsActive { get; private set; }
        public virtual Individual? Individual { get; private set; }
        public virtual Company? Company { get; private set; }

        private readonly List<Address> _addresses = new();
        public virtual IReadOnlyCollection<Address> Addresses => _addresses;

        private readonly List<Contact> _contacts = new();
        public virtual IReadOnlyCollection<Contact> Contacts => _contacts;
        protected Customer() { }
        public Customer(Guid sessionToken)
        {
            SessionToken = sessionToken;
            CustomerType = CustomerType.Guest;
            IsActive = true;
        }
        public void BecomeLead(Email email, Password password)
        {
            Email = email ?? throw new DomainException("Email é obrigatório para um Lead.");
            Password = password ?? throw new DomainException("Senha é obrigatória para um Lead.");
            CustomerType = CustomerType.Lead;
        }
        public void BecomeIndividual(string taxId, string fullName, DateTime? birthDate)
        {
            if (CustomerType == CustomerType.Guest)
                throw new DomainException("É necessário tornar-se um Lead antes de completar o cadastro B2C.");

            CustomerType = CustomerType.B2C;
            var voTaxId = new TaxId(taxId);
            var voFullName = new PersonName(fullName);

            Individual = new Individual(Id, voTaxId, voFullName, birthDate);
        }
        public void BecomeCompany(string businessTaxId, string name, string? stateTaxId)
        {
            if (CustomerType == CustomerType.Guest)
                throw new DomainException("É necessário tornar-se um Lead antes de completar o cadastro B2B.");
            CustomerType = CustomerType.B2B;
            var voBusinessTaxId = new BusinessTaxId(businessTaxId);
            Company = new Company(Id, voBusinessTaxId, name, stateTaxId);
        }
        public void AddAddress(Address address)
        {
            if (address.IsDefault)
            {
                foreach (var a in _addresses)
                    a.SetNonDefault();
            }
            _addresses.Add(address);
        }
        public void AddContact(Contact contact)
        {
            if (CustomerType != CustomerType.B2B)
                throw new DomainException("Apenas clientes B2B podem ter contatos adicionais.");

            if (_contacts.Any(x => x.Email.Address == contact.Email.Address))
                throw new DomainException("Este contato já está vinculado a esta empresa.");

            _contacts.Add(contact);
        }
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}