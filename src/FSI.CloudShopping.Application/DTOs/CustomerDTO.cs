namespace FSI.CloudShopping.Application.DTOs
{
    public record CustomerDTO
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public string TaxId { get; init; } // CPF ou CNPJ
        public string Phone { get; init; }
        public bool IsCompany { get; init; }
    }
}
