using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Results;
using CustomerService.Domain.Abtractions.Repositories;
using CustomerService.Domain.Entities;
using CustomerService.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Application.Commands.Customers.AddReview
{
    public class AddReviewCommandHandler : ICommandHandler<AddReviewCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddReviewCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            Customer? customer = await _customerRepository.AsQueryable().Include(c => c.Reviews)
                .Where(c => c.Id == request.CustomerId).FirstOrDefaultAsync(cancellationToken);

            if (customer == null)
                return Result.Failure(CustomerError.CustomerNotFound);

            Result<Review> review = Review.Create(request.ProductId, customer.Id, request.Rating, request.Comment);

            customer.AddReview(review.Value);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}