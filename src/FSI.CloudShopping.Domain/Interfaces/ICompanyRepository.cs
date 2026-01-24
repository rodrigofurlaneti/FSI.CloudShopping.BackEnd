using FSI.CloudShopping.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company?> GetByBusinessTaxIdAsync(string businessTaxId);
        Task<Company?> GetByStateTaxIdAsync(string stateTaxId);
        Task<IEnumerable<Company>> SearchByCompanyNameAsync(string name);
    }
}
