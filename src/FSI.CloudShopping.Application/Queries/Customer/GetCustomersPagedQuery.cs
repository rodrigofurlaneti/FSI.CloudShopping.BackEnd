namespace FSI.CloudShopping.Application.Queries.Customer;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;

public record GetCustomersPagedQuery(
    int Page = 1,
    int PageSize = 10,
    string? CustomerType = null,
    string? SearchTerm = null
) : IRequest<Result<PagedResult<CustomerDto>>>;

public class GetCustomersPagedQueryHandler : IRequestHandler<GetCustomersPagedQuery, Result<PagedResult<CustomerDto>>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomersPagedQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<CustomerDto>>> Handle(GetCustomersPagedQuery request, CancellationToken cancellationToken)
    {
        var pagedCustomers = await _customerRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.SearchTerm,
            cancellationToken
        );

        var customerDtos = pagedCustomers.Items
            .Select(customer => _mapper.Map<CustomerDto>(customer))
            .ToList();

        var result = new PagedResult<CustomerDto>(customerDtos, pagedCustomers.TotalCount, request.Page, request.PageSize);
        return new Result<PagedResult<CustomerDto>>.Success(result);
    }
}
