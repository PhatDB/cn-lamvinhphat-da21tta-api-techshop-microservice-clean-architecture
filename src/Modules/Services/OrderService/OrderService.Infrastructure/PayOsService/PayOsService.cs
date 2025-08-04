using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using OrderService.Application.Abstractions;

namespace OrderService.Infrastructure.PayOsService
{
    public class PayOsService : IPayOsService
    {
        private readonly string _checksumKey;
        private readonly PayOS _client;

        public PayOsService(IConfiguration config)
        {
            IConfigurationSection section = config.GetSection("PayOS");
            _client = new PayOS(section["ClientId"]!, section["ApiKey"]!, section["ChecksumKey"]!);
            _checksumKey = section["ChecksumKey"]!;
        }

        public async Task<CreatePaymentResult> CreatePaymentLinkAsync(
            long orderCode, long amount, string description, string returnUrl, string cancelUrl)
        {
            List<ItemData> items = new();

            PaymentData body = new(orderCode, (int)amount, description, items, cancelUrl, returnUrl);

            return await _client.createPaymentLink(body);
        }

        public bool VerifySignatureOnData(JsonElement dataEl, string signature)
        {
            IEnumerable<string> parts = dataEl.EnumerateObject().OrderBy(p => p.Name, StringComparer.Ordinal).Select(
                p =>
                {
                    string key = p.Name;
                    JsonElement v = p.Value;
                    string val = v.ValueKind switch
                    {
                        JsonValueKind.Null => "",
                        JsonValueKind.String or JsonValueKind.Number or JsonValueKind.True or JsonValueKind.False => v
                            .ToString()!,
                        JsonValueKind.Object => JsonSerializer.Serialize(v.EnumerateObject()
                            .OrderBy(x => x.Name, StringComparer.Ordinal).ToDictionary(x => x.Name, x => x.Value)),
                        JsonValueKind.Array => JsonSerializer.Serialize(v.EnumerateArray().Select(item =>
                            item.ValueKind == JsonValueKind.Object
                                ? JsonSerializer.Serialize(item.EnumerateObject()
                                    .OrderBy(x => x.Name, StringComparer.Ordinal)
                                    .ToDictionary(x => x.Name, x => x.Value))
                                : item.ToString())),
                        _ => v.ToString()!
                    };
                    return $"{key}={val}";
                });

            string payloadString = string.Join("&", parts);

            byte[] keyBytes = Encoding.UTF8.GetBytes(_checksumKey);
            byte[] dataBytes = Encoding.UTF8.GetBytes(payloadString);
            using HMACSHA256 hmac = new(keyBytes);
            byte[] hash = hmac.ComputeHash(dataBytes);
            string computed = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

            return computed == signature.ToLowerInvariant();
        }
    }
}