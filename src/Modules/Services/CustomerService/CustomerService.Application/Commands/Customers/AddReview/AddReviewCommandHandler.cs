using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Customers;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using CustomerService.Application.Abtractions;
using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Application.Commands.Customers.AddReview
{
    public class AddReviewCommandHandler : ICommandHandler<AddReviewCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerService _customerService;
        private readonly IUnitOfWork _unitOfWork;

        public AddReviewCommandHandler(
            ICustomerRepository customerRepository, IUnitOfWork unitOfWork, ICustomerService customerService)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _customerService = customerService;
        }

        public async Task<Result> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            Result<HasCustomerPaidResponse> hasCustomerPaidResult =
                await _customerService.HasCustomerPaid(request.CustomerId, request.ProductId);

            if (!hasCustomerPaidResult.Value.HasCustomerPaid)
                return Result.Failure(CustomerError.NotPaid);

            Customer? customer = await _customerRepository.AsQueryable().Include(c => c.Reviews)
                .Where(c => c.Id == request.CustomerId).FirstOrDefaultAsync(cancellationToken);

            if (customer == null)
                return Result.Failure(CustomerError.CustomerNotFound);

            if (customer.Reviews.Count != 0 && customer.Reviews.Any(r => r.ProductId == request.ProductId))
                return Result.Failure(CustomerError.AllReadyReview);

            Result<Review> review = Review.Create(request.ProductId, customer.Id, request.Rating, request.Comment);

            customer.AddReview(review.Value);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}