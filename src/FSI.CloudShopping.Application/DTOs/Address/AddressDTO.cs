using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.CloudShopping.Application.DTOs.Address
{
    public record AddressDTO(
        int Id,
        int CustomerId,
        string AddressType,
        string Street,
        string Number,
        string? Neighborhood,
        string City,
        string State,
        string ZipCode,
        bool IsDefault);
}
