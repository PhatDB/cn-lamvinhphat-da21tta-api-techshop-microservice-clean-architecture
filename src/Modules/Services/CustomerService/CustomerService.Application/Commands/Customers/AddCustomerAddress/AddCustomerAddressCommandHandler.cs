using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Application.Commands.Customers.AddCustomerAddress
{
    public class AddCustomerAddressCommandHandler : ICommandHandler<AddCustomerAddressCommand, int>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddCustomerAddressCommandHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
        }

        public async Task<Result<int>> Handle(AddCustomerAddressCommand request, CancellationToken cancellationToken)
        {
            Customer? customer = await _customerRepository.AsQueryable().Include(c => c.Addresses)
                .Where(c => c.Id == request.CustomerId).FirstOrDefaultAsync(cancellationToken);

            if (customer == null)
                return Result.Failure<int>(CustomerError.CustomerNotFound);

            Result addAddressResult = customer.AddAddress(request.Street, request.Hamlet, request.Ward,
                request.District, request.City);

            if (addAddressResult.IsFailure)
                return Result.Failure<int>(addAddressResult.Error);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(customer.Id);
        }
    }
}