using BuildingBlocks.CQRS;
using CustomerService.Application.DTOs;

namespace CustomerService.Application.Queries.Customers
{
    public record GetCustomerInfoQuery(int CustomerId) : IQuery<CustomerDto>;
}