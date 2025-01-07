using AssetService.Grpc;
using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Enumerations;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Extensions
{
    public class FileService : IFileService
    {
        private readonly AssetsService.AssetsServiceClient _assetsServiceClient;

        public FileService(IConfiguration configuration, IGrpcService grpcService)
        {
            string? url = configuration["GrpcServiceAddress:AssetServer"];
            if (string.IsNullOrEmpty(url))
                throw new InvalidOperationException("The AssetServer gRPC address is not configured.");

            GrpcChannel channel = grpcService.CreateInsecureGrpcChannel(url);
            _assetsServiceClient = new AssetsService.AssetsServiceClient(channel);
        }

        public async Task<string> UploadFile(string base64String, AssetType type)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                throw new ArgumentException("Base64 string cannot be null or empty.");

            try
            {
                UploadImageRequest request = new() { Content = base64String, AssetType = (int)type };

                UploadImageResponse response = await _assetsServiceClient.UploadImageAsync(request);

                if (string.IsNullOrWhiteSpace(response.ImageUrl))
                    throw new Exception("The gRPC server returned an empty ImageUrl.");

                return response.ImageUrl;
            }
            catch (RpcException rpcEx)
            {
                Console.WriteLine($"gRPC error: Status({rpcEx.Status.StatusCode}), Detail: {rpcEx.Status.Detail}");
                throw new Exception($"Failed to upload file via gRPC: {rpcEx.Status.Detail}", rpcEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw new Exception("An error occurred while uploading the file.", ex);
            }
        }
    }
}