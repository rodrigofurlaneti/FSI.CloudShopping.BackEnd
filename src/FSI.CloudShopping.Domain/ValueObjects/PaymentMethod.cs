using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record PaymentMethod
    {
        public string Description { get; }

        private PaymentMethod(string description) => Description = description;

        public static PaymentMethod CreditCard => new("CreditCard");
        public static PaymentMethod Pix => new("Pix");
        public static PaymentMethod Invoice => new("Invoice"); // Essencial para B2B

        public static PaymentMethod FromString(string value)
        {
            return value switch
            {
                "CreditCard" => CreditCard,
                "Pix" => Pix,
                "Invoice" => Invoice,
                _ => throw new ArgumentException("Invalid Payment Method")
            };
        }
    }
}
