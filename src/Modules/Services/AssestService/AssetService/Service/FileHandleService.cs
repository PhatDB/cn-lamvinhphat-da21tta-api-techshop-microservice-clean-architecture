using AssetService.Enumerations;
using AssetService.Extentions;
using AssetService.Grpc;
using Grpc.Core;

namespace AssetService.Service
{
    public class FileHandleService : AssetsService.AssetsServiceBase
    {
        private readonly string fileDirectory;
        private readonly string fileDirectoryPath;
        private readonly long maxFileSize;
        private readonly List<AssetType> overwriteTypes;

        private readonly List<string> permittedExtensions = new()
        {
            ".png",
            ".jpg",
            ".jpeg",
            ".pdf",
            ".webp"
        };

        public FileHandleService(IWebHostEnvironment env, IConfiguration configuration)
        {
            fileDirectory = configuration["FileSettings:Directory"];
            fileDirectoryPath = Path.Combine(env.WebRootPath, fileDirectory);
            maxFileSize = configuration.GetValue<long>("FileSettings:MaxSize", 2 * 1024 * 1024);

            overwriteTypes = Enum.GetValues(typeof(AssetType)).Cast<AssetType>()
                .Where(type => type.GetType().GetField(type.ToString()).GetCustomAttributes(typeof(OverwriteAttribute), false).Length > 0).ToList();

            if (!Directory.Exists(fileDirectoryPath)) Directory.CreateDirectory(fileDirectoryPath);
        }

        public override async Task<UploadImageResponse> UploadImage(UploadImageRequest request, ServerCallContext context)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Content))
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "File content cannot be empty"));

                byte[] fileBytes;
                try
                {
                    fileBytes = Convert.FromBase64String(request.Content);
                }
                catch (FormatException)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Base64 content"));
                }

                if (fileBytes.Length > maxFileSize)
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "File size exceeds the maximum allowed size"));

                string fileExtension = GetFileExtensionFromMagicNumber(fileBytes);
                if (!permittedExtensions.Contains(fileExtension))
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "File type is not permitted"));

                string newFileName = CreateNewFileName((AssetType)request.AssetType) + fileExtension;
                string fullPath = GetFilePath((AssetType)request.AssetType, newFileName);

                if (overwriteTypes.Contains((AssetType)request.AssetType))
                {
                    List<string> existingFiles = Directory.GetFiles(Path.GetDirectoryName(fullPath))
                        .Where(f => Path.GetFileNameWithoutExtension(f).StartsWith(request.AssetType.ToString())).ToList();

                    foreach (string existingFile in existingFiles) File.Delete(existingFile);
                }

                await File.WriteAllBytesAsync(fullPath, fileBytes);

                string relativePath = GetRelativePath((AssetType)request.AssetType, newFileName);
                return new UploadImageResponse { ImageUrl = relativePath };
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public override Task<DeleteImageResponse> DeleteImage(DeleteImageRequest request, ServerCallContext context)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ImageUrl))
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Image URL cannot be empty"));

                string fileName = Path.GetFileName(request.ImageUrl);
                string filePath = Path.Combine(fileDirectoryPath, fileName);

                if (!File.Exists(filePath))
                    return Task.FromResult(new DeleteImageResponse { Success = false });

                File.Delete(filePath);

                return Task.FromResult(new DeleteImageResponse { Success = true });
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        private string GetFileExtensionFromMagicNumber(byte[] fileBytes)
        {
            if (fileBytes.Length < 12) return string.Empty;

            if (fileBytes[0] == 0xFF && fileBytes[1] == 0xD8 && fileBytes[2] == 0xFF)
                return ".jpg";
            if (fileBytes[0] == 0x89 && fileBytes[1] == 0x50 && fileBytes[2] == 0x4E && fileBytes[3] == 0x47)
                return ".png";
            if (fileBytes[0] == 0x25 && fileBytes[1] == 0x50 && fileBytes[2] == 0x44 && fileBytes[3] == 0x46)
                return ".pdf";
            if (fileBytes[0] == 0x52 && fileBytes[1] == 0x49 && fileBytes[2] == 0x46 && fileBytes[3] == 0x46 && fileBytes[8] == 0x57 && fileBytes[9] == 0x45 &&
                fileBytes[10] == 0x42 && fileBytes[11] == 0x50)
                return ".webp";

            return string.Empty;
        }

        private string CreateNewFileName(AssetType type)
        {
            string prefix = Enum.GetName(typeof(AssetType), type)?.ToLower().Split('_')[0] ?? "unknown";
            string uniqueId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            return $"{prefix}_{uniqueId}";
        }

        private string GetFilePath(AssetType type, string fileName)
        {
            string folderPath = Path.Combine(fileDirectoryPath, GetFolder(type));
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            return Path.Combine(folderPath, fileName);
        }

        private string GetRelativePath(AssetType type, string fileName)
        {
            return Path.Combine(fileDirectory, GetFolder(type), fileName).Replace("\\", "/");
        }

        private string GetFolder(AssetType type)
        {
            return Enum.GetName(typeof(AssetType), type)?.ToLower().Split('_')[0] ?? "default";
        }
    }
}