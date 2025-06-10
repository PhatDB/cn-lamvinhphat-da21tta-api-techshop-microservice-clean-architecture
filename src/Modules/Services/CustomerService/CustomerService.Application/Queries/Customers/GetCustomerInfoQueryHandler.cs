using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using CustomerService.Application.DTOs;
using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Application.Queries.Customers
{
    public class GetCustomerInfoQueryHandler : IQueryHandler<GetCustomerInfoQuery, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerInfoQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<CustomerDto>> Handle(GetCustomerInfoQuery request, CancellationToken cancellationToken)
        {
            Customer? customer = await _customerRepository.AsQueryable().Include(c => c.Addresses)
                .Where(c => c.Id == request.CustomerId).FirstOrDefaultAsync(cancellationToken);

            if (customer == null)
                return Result.Failure<CustomerDto>(CustomerError.CustomerNotFound);

            CustomerDto customerDto = new()
            {
                CustomerId = customer.Id,
                CustomerName = customer.CustomerName,
                Email = customer.Email.Value,
                Phone = customer.Phone?.Value,
                Address = customer.Addresses?.Select(a => new AddressDto
                {
                    AddressId = a.Id,
                    Street = a.Street,
                    Hamlet = a.Hamlet,
                    Ward = a.Ward,
                    District = a.District,
                    City = a.City
                }).ToList()
            };

            return Result.Success(customerDto);
        }
    }
}