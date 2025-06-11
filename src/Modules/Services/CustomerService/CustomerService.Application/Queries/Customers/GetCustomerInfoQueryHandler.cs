using AutoMapper;
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
        private readonly IMapper _mapper;

        public GetCustomerInfoQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }


        public async Task<Result<CustomerDto>> Handle(GetCustomerInfoQuery request, CancellationToken cancellationToken)
        {
            Customer? customer = await _customerRepository.AsQueryable().AsNoTracking().Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);

            if (customer == null)
                return Result.Failure<CustomerDto>(CustomerError.CustomerNotFound);

            CustomerDto? dto = _mapper.Map<CustomerDto>(customer);
            return Result.Success(dto);
        }
    }
}