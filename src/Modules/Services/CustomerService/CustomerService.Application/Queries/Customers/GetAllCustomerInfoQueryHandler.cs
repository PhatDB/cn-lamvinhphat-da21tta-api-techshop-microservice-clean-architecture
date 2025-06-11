using AutoMapper;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using CustomerService.Application.DTOs;
using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Application.Queries.Customers
{
    public class GetAllCustomerInfoQueryHandler : IQueryHandler<GetAllCustomerInfoQuery, List<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public GetAllCustomerInfoQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<CustomerDto>>> Handle(
            GetAllCustomerInfoQuery request, CancellationToken cancellationToken)
        {
            List<Customer> customers = await _customerRepository.AsQueryable().AsNoTracking().Include(x => x.Addresses)
                .ToListAsync(cancellationToken);

            List<CustomerDto>? result = _mapper.Map<List<CustomerDto>>(customers);

            return Result.Success(result);
        }
    }
}