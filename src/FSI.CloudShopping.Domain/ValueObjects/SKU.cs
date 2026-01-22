using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.CloudShopping.Domain.ValueObjects
{
    public record SKU
    {
        public string Code { get; }

        public SKU(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("SKU code cannot be empty.");

            Code = code.ToUpper().Trim();
        }
    }
}
