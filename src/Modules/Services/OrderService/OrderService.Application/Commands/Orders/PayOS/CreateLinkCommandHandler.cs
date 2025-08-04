using BuildingBlocks.Abstractions.Repository;
using BuildingBlocks.Contracts.Carts;
using BuildingBlocks.Contracts.Orders;
using BuildingBlocks.Contracts.Products;
using BuildingBlocks.CQRS;
using BuildingBlocks.Error;
using BuildingBlocks.Results;
using MassTransit;
using Net.payOS.Types;
using OrderService.Application.Abstractions;
using OrderService.Application.DTOs;
using OrderService.Domain.Abstractions.Repositories;
using OrderService.Domain.Entities;
using OrderService.Domain.Enum;

namespace OrderService.Application.Commands.Orders.PayOS
{
    public class CreateLinkCommandHandler : ICommandHandler<CreateLinkCommand, string>
    {
        private readonly IEmailSender _emailSender;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;
        private readonly IPayOsService _payOsService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLinkCommandHandler(
            IOrderRepository orderRepository, IPayOsService payOsService, IOrderService orderService,
            IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint, IEmailSender emailSender)
        {
            _orderRepository = orderRepository;
            _payOsService = payOsService;
            _orderService = orderService;
            _unitOfWork = unitOfWork;
            _publishEndpoint = publishEndpoint;
            _emailSender = emailSender;
        }

        public async Task<Result<string>> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
        {
            Result<GetCartInfoResponse> cartResult = await _orderService.GetCartInfo(request.CartId);
            if (cartResult.IsFailure)
                return Result.Failure<string>(cartResult.Error);

            GetCartInfoResponse cart = cartResult.Value!;
            if (!cart.CartItems.Any())
                return Result.Failure<string>(Error.Validation("Cart.Empty", "Cart does not contain any items."));

            List<CheckStockRequest> stockReqs =
                cart.CartItems.Select(ci => new CheckStockRequest(ci.ProductId, ci.Quantity)).ToList();
            Result<CheckStockResponse> stockCheck = await _orderService.CheckStockAsync(stockReqs);
            if (stockCheck.IsFailure)
                return Result.Failure<string>(stockCheck.Error);

            List<CheckStockResult> insufficient = stockCheck.Value!.Results.Where(r => !r.IsAvailable).ToList();
            if (insufficient.Any())
            {
                CheckStockResult s = insufficient.First();
                return Result.Failure<string>(Error.Conflict("Stock.Insufficient",
                    $"Product {s.ProductId} only has {s.QuantityAvailable} in stock, but you requested {s.QuantityRequested}."));
            }

            Result<Order> orderResult = Order.Create(cart.CustomerId, OrderStatus.Confirmed, request.Amount,
                request.ReceiverName, request.ReceiverPhone, request.ReceiverAddress, request.Note, request.SessionId,
                request.PaymentMethod);

            if (orderResult.IsFailure)
                return Result.Failure<string>(orderResult.Error);

            Order order = orderResult.Value;
            foreach (CartItemDTO ci in cart.CartItems)
                order.AddItem(ci.ProductId, ci.Quantity, ci.Price);

            await _orderRepository.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            List<OrderItemInfo> itemsInfo = cart.CartItems
                .Select(ci => new OrderItemInfo(ci.ProductId, ci.Quantity, ci.Price)).ToList();
            OrderCreated orderCreated = new(order.Id, order.CustomerId, request.CartId, itemsInfo);
            await _publishEndpoint.Publish(orderCreated, cancellationToken);

            CreatePaymentResult payOsResponse = await _payOsService.CreatePaymentLinkAsync(order.Id, request.Amount,
                $"Thanh toán đơn #{order.Id}", request.ReturnUrl, request.CancelUrl);

            string checkoutUrl = payOsResponse.checkoutUrl;

            Result<GetCustomerInfoResponse> customerInfo = await _orderService.GetCustomerInfo(cart.CustomerId);
            Result<GetListProductInfoResponse> productInfo =
                await _orderService.GetListProductInfo(cart.CartItems.Select(x => x.ProductId).ToList());

            if (customerInfo.IsSuccess && productInfo.IsSuccess)
            {
                string htmlBody = BuildOrderEmailHtml(order, cart, productInfo.Value!, customerInfo.Value!);
                EmailDto email = new()
                {
                    ToEmail = customerInfo.Value.Email,
                    Subject = $"[TechShop] Thanh toán đơn hàng #{order.Id}",
                    Body = htmlBody
                };
                await _emailSender.SendEmailAsync(email);
            }

            return Result.Success(checkoutUrl);
        }

        private async Task SendOrderConfirmationEmail(
            Order order, GetCartInfoResponse cart, GetListProductInfoResponse products,
            GetCustomerInfoResponse customer)
        {
            string htmlBody = BuildOrderEmailHtml(order, cart, products, customer);

            EmailDto email = new()
            {
                ToEmail = customer.Email, Subject = $"[TechShop] Xác nhận đơn hàng #{order.Id}", Body = htmlBody
            };

            await _emailSender.SendEmailAsync(email);
        }

        private static string BuildOrderEmailHtml(
            Order order, GetCartInfoResponse cart, GetListProductInfoResponse products,
            GetCustomerInfoResponse customer)
        {
            Dictionary<int, string> productNameMap =
                products.ProductInfos.ToDictionary(p => p.ProductId, p => p.ProductName);

            string FormatMoney(decimal v)
            {
                return string.Format("{0:#,##0} ₫", v);
            }

            string itemsRows = string.Join("", cart.CartItems.Select(ci =>
            {
                string name = productNameMap.TryGetValue(ci.ProductId, out string? n) ? n : $"#{ci.ProductId}";
                return $@"
                <tr>
                    <td style=""padding:8px;border:1px solid #ddd;"">{ci.ProductId}</td>
                    <td style=""padding:8px;border:1px solid #ddd;"">{name}</td>
                    <td style=""padding:8px;border:1px solid #ddd;text-align:right;"">{ci.Quantity}</td>
                    <td style=""padding:8px;border:1px solid #ddd;text-align:right;"">{FormatMoney(ci.Price)}</td>
                    <td style=""padding:8px;border:1px solid #ddd;text-align:right;"">{FormatMoney(ci.Price * ci.Quantity)}</td>
                </tr>";
            }));

            return $@"
            <!DOCTYPE html>
            <html lang=""vi"">
            <head>
              <meta charset=""utf-8"" />
              <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
              <title>Xác nhận đơn hàng #{order.Id}</title>
            </head>
            <body style=""font-family:Arial,Helvetica,sans-serif;color:#333;line-height:1.5;margin:0;padding:0;background:#f5f6f8;"">
              <div style=""max-width:640px;margin:0 auto;background:#fff;border:1px solid #e5e7eb;"">
                <div style=""padding:24px 32px;border-bottom:1px solid #e5e7eb;background:#0f172a;color:#fff;"">
                  <h2 style=""margin:0;font-size:20px;"">TechShop.com</h2>
                  <p style=""margin:4px 0 0;font-size:14px;"">Xác nhận đơn hàng #{order.Id}</p>
                </div>

                <div style=""padding:24px 32px;"">
                  <p>Xin chào <strong>{customer.CustomerName}</strong>,</p>
                  <p>Cảm ơn bạn đã đặt hàng tại <strong>TechShop.com</strong>. Chúng tôi đã nhận được đơn hàng và đang tiến hành xử lý.</p>

                  <h3 style=""margin-top:24px;margin-bottom:8px;font-size:16px;"">Thông tin đơn hàng</h3>
                  <table style=""width:100%;border-collapse:collapse;font-size:14px;"">
                    <thead>
                      <tr style=""background:#f1f5f9;"">
                        <th style=""padding:8px;border:1px solid #ddd;text-align:left;"">Mã SP</th>
                        <th style=""padding:8px;border:1px solid #ddd;text-align:left;"">Sản phẩm</th>
                        <th style=""padding:8px;border:1px solid #ddd;text-align:right;"">SL</th>
                        <th style=""padding:8px;border:1px solid #ddd;text-align:right;"">Đơn giá</th>
                        <th style=""padding:8px;border:1px solid #ddd;text-align:right;"">Thành tiền</th>
                      </tr>
                    </thead>
                    <tbody>
                      {itemsRows}
                      <tr>
                        <td colspan=""4"" style=""padding:8px;border:1px solid #ddd;text-align:right;font-weight:bold;"">Tổng cộng</td>
                        <td style=""padding:8px;border:1px solid #ddd;text-align:right;font-weight:bold;"">{FormatMoney(order.TotalAmount)}</td>
                      </tr>
                    </tbody>
                  </table>

                  <h3 style=""margin-top:24px;margin-bottom:8px;font-size:16px;"">Thông tin nhận hàng</h3>
                  <p style=""margin:4px 0;""><strong>Người nhận:</strong> {order.ReceiverName}</p>
                  <p style=""margin:4px 0;""><strong>SĐT:</strong> {order.ReceiverPhone}</p>
                  <p style=""margin:4px 0;""><strong>Địa chỉ:</strong> {order.ReceiverAddress}</p>

                  <p style=""margin-top:24px;"">Nếu bạn có bất kỳ câu hỏi nào, vui lòng phản hồi email.</p>
                  <p style=""margin-top:8px;"">Trân trọng,<br/><strong>TechShop.com</strong></p>
                </div>

                <div style=""padding:16px 32px;background:#f8fafc;border-top:1px solid #e5e7eb;font-size:12px;color:#64748b;"">
                  Email này được gửi tự động. Vui lòng không trả lời trực tiếp.
                </div>
              </div>
            </body>
            </html>";
        }
    }
}