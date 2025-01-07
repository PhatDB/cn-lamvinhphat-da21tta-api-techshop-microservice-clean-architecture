using BuildingBlocks.Enumerations;

namespace BuildingBlocks.Abstractions.Extensions
{
    public interface IFileService
    {
        Task<string> UploadFile(string base64String, AssetType type);
        Task<bool> DeleteFile(string filePath);
    }
}