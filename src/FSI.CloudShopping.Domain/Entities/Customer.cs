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
        public GeoLocation? GeoLocation { get; private set; }
        public DeviceInfo? DeviceInfo { get; private set; }
        public virtual Individual? Individual { get; private set; }
        public virtual Company? Company { get; private set; }

        private readonly List<Address> _addresses = new();
        public virtual IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

        private readonly List<Contact> _contacts = new();
        public virtual IReadOnlyCollection<Contact> Contacts => _contacts.AsReadOnly();

        protected Customer() { }

        // 🔹 Criação do Guest (primeiro acesso)
        public Customer(
            Guid sessionToken,
            decimal? latitude = null,
            decimal? longitude = null,
            DeviceInfo? deviceInfo = null)
        {
            if (sessionToken == Guid.Empty)
                throw new DomainException("SessionToken inválido.");

            SessionToken = sessionToken;

            if (latitude.HasValue && longitude.HasValue)
                GeoLocation = new GeoLocation(latitude.Value, longitude.Value);

            DeviceInfo = deviceInfo;

            CustomerType = CustomerType.Guest;
            IsActive = true;
        }

        // 🔹 Atualização explícita de Geolocalização
        public void UpdateGeolocation(decimal latitude, decimal longitude)
        {
            GeoLocation = new GeoLocation(latitude, longitude);
        }

        // 🔹 Guest → Lead
        public void BecomeLead(Email email, Password password)
        {
            if (CustomerType != CustomerType.Guest)
                throw new DomainException("Cliente já não é mais Guest.");

            Email = email ?? throw new DomainException("Email é obrigatório para um Lead.");
            Password = password ?? throw new DomainException("Senha é obrigatória para um Lead.");

            CustomerType = CustomerType.Lead;
        }

        // 🔹 Lead → B2C
        public void BecomeIndividual(string taxId, string fullName, DateTime? birthDate)
        {
            if (CustomerType == CustomerType.Guest)
                throw new DomainException("É necessário tornar-se um Lead antes de completar o cadastro B2C.");

            if (CustomerType == CustomerType.B2B)
                throw new DomainException("Cliente B2B não pode ser convertido para B2C.");

            var voTaxId = new TaxId(taxId);
            var voFullName = new PersonName(fullName);

            Individual = new Individual(Id, voTaxId, voFullName, birthDate);
            CustomerType = CustomerType.B2C;
        }

        // 🔹 Lead → B2B
        public void BecomeCompany(string businessTaxId, string name, string? stateTaxId)
        {
            if (CustomerType == CustomerType.Guest)
                throw new DomainException("É necessário tornar-se um Lead antes de completar o cadastro B2B.");

            if (CustomerType == CustomerType.B2C)
                throw new DomainException("Cliente B2C não pode ser convertido para B2B.");

            var voBusinessTaxId = new BusinessTaxId(businessTaxId);

            Company = new Company(Id, voBusinessTaxId, name, stateTaxId);
            CustomerType = CustomerType.B2B;
        }

        // 🔹 Endereços
        public void AddAddress(Address address)
        {
            if (address == null)
                throw new DomainException("Endereço inválido.");

            if (address.IsDefault)
            {
                foreach (var a in _addresses)
                    a.SetNonDefault();
            }

            _addresses.Add(address);
        }

        // 🔹 Contatos (apenas B2B)
        public void AddContact(Contact contact)
        {
            if (CustomerType != CustomerType.B2B)
                throw new DomainException("Apenas clientes B2B podem ter contatos adicionais.");

            if (contact == null)
                throw new DomainException("Contato inválido.");

            if (_contacts.Any(x => x.Email.Address == contact.Email.Address))
                throw new DomainException("Este contato já está vinculado a esta empresa.");

            _contacts.Add(contact);
        }

        // 🔹 Ativação
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}
