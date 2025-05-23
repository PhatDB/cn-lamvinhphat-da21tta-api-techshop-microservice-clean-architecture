using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Errors;
using CustomerService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Application.Commands.Customers.VerifyOTP
{
    public class VerifyOTPCommandHandler : ICommandHandler<VerifyOTPCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VerifyOTPCommandHandler(IUnitOfWork unitOfWork, ICustomerRepository customerRepository)
        {
            _unitOfWork = unitOfWork;
            _customerRepository = customerRepository;
        }

        public async Task<Result> Handle(VerifyOTPCommand request, CancellationToken cancellationToken)
        {
            Customer? customer = await _customerRepository.AsQueryable().Where(c => c.Email == new Email(request.Email))
                .FirstOrDefaultAsync(cancellationToken);

            if (customer == null)
                return Result.Failure(CustomerError.CustomerNotFound);

            if (!customer.ValidateOTP(request.Otp))
                return Result.Failure(Error.Validation("OTP.Invalid", "OTP is invalid"));

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}