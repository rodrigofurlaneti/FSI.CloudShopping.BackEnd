using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Domain.Entities
{
    public class Customer : Entity
    {
        public Email Email { get; private set; }
        public TaxId Document { get; private set; }
        public Password Password { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<Address> _addresses = new();
        public IReadOnlyCollection<Address> Addresses => _addresses;

        private readonly List<Contact> _contacts = new();
        public IReadOnlyCollection<Contact> Contacts => _contacts;

        protected Customer() { }

        public Customer(Email email, TaxId document, Password password)
        {
            Email = email;
            Document = document;
            Password = password;
            IsActive = true;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public void UpdateAddress(Address address)
        {
            if (address.IsDefault)
                DeactivateOtherDefaultAddresses();

            _addresses.Add(address);
        }

        public void AddContact(Contact contact)
        {
            if (_contacts.Any(x => x.Email.Address == contact.Email.Address))
                throw new DomainException("Este contato já está cadastrado para este cliente.");

            _contacts.Add(contact);
        }

        private void DeactivateOtherDefaultAddresses()
        {
            foreach (var address in _addresses)
                address.SetNonDefault();
        }
    }
}