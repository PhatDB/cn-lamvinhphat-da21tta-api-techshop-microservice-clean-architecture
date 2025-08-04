using System.Text.Json;
using Net.payOS.Types;

namespace OrderService.Application.Abstractions
{
    public interface IPayOsService
    {
        Task<CreatePaymentResult> CreatePaymentLinkAsync(
            long orderCode, long amount, string description, string returnUrl, string cancelUrl);

        bool VerifySignatureOnData(JsonElement dataEl, string signature);
    }
}