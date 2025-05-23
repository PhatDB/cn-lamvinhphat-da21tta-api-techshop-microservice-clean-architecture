using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Application.Commands.Customers.DeleteCustomerAddress
{
    public class DeleteCustomerAddressCommandHandler : ICommandHandler<DeleteCustomerAddressCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCustomerAddressCommandHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
        }

        public async Task<Result> Handle(DeleteCustomerAddressCommand request, CancellationToken cancellationToken)
        {
            Customer? customer = await _customerRepository.AsQueryable().Include(c => c.Addresses)
                .Where(c => c.Addresses.Any(a => a.Id == request.AddressId)).FirstOrDefaultAsync(cancellationToken);

            if (customer == null)
                return Result.Failure(CustomerError.CustomerNotFound);

            Result deleteAddressResult = customer.RemoveAddress(request.AddressId);

            if (deleteAddressResult.IsFailure)
                return Result.Failure<int>(deleteAddressResult.Error);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(customer.Id);
        }
    }
}