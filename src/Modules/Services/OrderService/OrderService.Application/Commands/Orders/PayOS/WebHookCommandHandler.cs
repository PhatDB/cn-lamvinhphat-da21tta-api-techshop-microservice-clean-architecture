using System.Text.Json;
using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using OrderService.Application.Abstractions;
using OrderService.Domain.Abstractions.Repositories;
using OrderService.Domain.Entities;

namespace OrderService.Application.Commands.Orders.PayOS
{
    public class WebHookCommandHandler : ICommandHandler<WebHookCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPayOsService _payOsService;
        private readonly IUnitOfWork _unitOfWork;

        public WebHookCommandHandler(
            IPayOsService payOsService, IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _payOsService = payOsService;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(WebHookCommand request, CancellationToken ct)
        {
            if (!_payOsService.VerifySignatureOnData(request.DataEl, request.Signature))
                return Result.Failure(Error.Validation("PayOS.Signature", "Invalid signature"));

            if (!request.DataEl.TryGetProperty("orderCode", out JsonElement ocEl) ||
                !request.DataEl.TryGetProperty("code", out JsonElement stEl))
                return Result.Failure(Error.Validation("PayOS.Payload", "Missing orderCode or status"));

            long orderCode = ocEl.GetInt64();
            string? status = stEl.GetString();

            Result<Order> orderRes = await _orderRepository.GetByIdAsync((int)orderCode);
            if (orderRes.IsFailure)
                return Result.Failure(Error.NotFound("Order", orderCode.ToString()));

            Order order = orderRes.Value;

            if (status == "00") order.SetPaidStatus();
            else order.SetCancelledStatus();

            await _unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}