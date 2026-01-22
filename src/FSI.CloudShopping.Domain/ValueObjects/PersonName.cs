using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record PersonName
    {
        public string FullName { get; }

        public PersonName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Name cannot be empty.");

            if (!fullName.Trim().Contains(" "))
                throw new ArgumentException("Please provide a full name (first and last name).");

            FullName = fullName;
        }

        public static implicit operator string(PersonName name) => name.FullName;
    }
}
