using BuildingBlocks.Abstractions.Extensions;
using Grpc.Net.Client;

namespace BuildingBlocks.Extensions
{
    public class GrpcService : IGrpcService
    {
        public GrpcChannel CreateGrpcChannel(string url)
        {
            return GrpcChannel.ForAddress(url);
        }

        public GrpcChannel CreateInsecureGrpcChannel(string url)
        {
            HttpClientHandler clientHandler = new();

            clientHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            GrpcChannelOptions options = new() { HttpHandler = clientHandler };

            return GrpcChannel.ForAddress(url, options);
        }
    }
}