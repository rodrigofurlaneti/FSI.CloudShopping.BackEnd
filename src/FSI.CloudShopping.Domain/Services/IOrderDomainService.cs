namespace FSI.CloudShopping.Domain.Services;

using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;

public interface IOrderDomainService
{
    void CalculateTotals(Order order);
}
