namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record CustomerType
    {
        public string Code { get; }
        private CustomerType(string code) => Code = code;
        public static CustomerType Guest => new("Guest");
        public static CustomerType Lead => new("Lead");
        public static CustomerType B2C => new("B2C");
        public static CustomerType B2B => new("B2B");

        public static CustomerType FromString(string value)
        {
            return value?.Trim() switch
            {
                "Guest" => Guest,   //Visitante sem cadastro
                "Lead" => Lead,     //Cliente potencial
                "B2C" => B2C,       //Cliente pessoa física
                "B2B" => B2B,       //Cliente pessoa jurídica
                _ => throw new ArgumentException($"Tipo de cliente inválido: {value}")
            };
        }

        public override string ToString() => Code;
    }
}