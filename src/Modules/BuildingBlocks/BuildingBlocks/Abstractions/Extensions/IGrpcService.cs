using Grpc.Net.Client;

namespace BuildingBlocks.Abstractions.Extensions
{
    public interface IGrpcService
    {
        GrpcChannel CreateGrpcChannel(string url);
        GrpcChannel CreateInsecureGrpcChannel(string url);
    }
}