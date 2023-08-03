using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace JacobsBlobStorage
{
    internal class BlobStorageHelper
    {  // Get existing blob list.
        public static async Task<List<BlobInfo>> GetBlobList(BlobContainerClient containerClient)
        {
            List<BlobInfo> blobsInfoList = new();
            try
            {
                if (containerClient != null && containerClient.Exists())
                {
                    await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
                    {
                        BlobInfo blobInfo = new()
                        {
                            Name = blobItem.Name,
                            ContentType = blobItem.Properties.ContentType,
                            FileExtension = Path.GetExtension(blobItem.Name),
                        };
                        blobsInfoList.Add(blobInfo);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return blobsInfoList;
        }

        // Update Content-Type of existing blobs.
        public static async Task<int> UpdateBlobContentTypes(List<BlobInfo> blobInfoList, BlobContainerClient containerClient)
        {
            int _blobCount = 0;
            if (containerClient != null && containerClient.Exists())
            {
                var contentTypeMapping = GetContentTypeMapping();
                List<Task> tasks = new();

                foreach (BlobInfo blobInfo in blobInfoList)
                {
                    string targetContentType = (blobInfo.FileExtension != null) ?
                    RetrieveContentTypeFromExtension(blobInfo.FileExtension, contentTypeMapping) : Constants.DefaultContentType;
                    //var targetContentType = Constants.DefaultContentType;
                    if (!string.Equals(blobInfo.ContentType, targetContentType, StringComparison.OrdinalIgnoreCase))
                    {
                        BlobClient blobClient = containerClient.GetBlobClient(blobInfo.Name);
                        tasks.Add(blobClient.SetHttpHeadersAsync(new BlobHttpHeaders { ContentType = targetContentType }));
                        _blobCount++;
                    }
                }
                await Task.WhenAll(tasks);
            }
            return _blobCount;
        }

        // Get container client.
        public static async Task<BlobContainerClient?> ConnectToBlobContainerAsync()
        {
            try
            {
                string _connectionString = $"BlobEndpoint=https://{Constants.AccountName}.blob.core.windows.net/;SharedAccessSignature={Constants.SasToken}";
                BlobServiceClient blobServiceClient = new(_connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(Constants.ContainerName);
                await containerClient.GetPropertiesAsync();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Connected to Azure Blob Storage successfully.");
                Console.ResetColor();

                return containerClient;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static string RetrieveContentTypeFromExtension(string fileExtension, Dictionary<string, string> contentTypeMapping)
        {
            return contentTypeMapping.TryGetValue(fileExtension.ToLowerInvariant(), out var fileContentType) ? fileContentType : Constants.DefaultContentType;
        }

        private static Dictionary<string, string> GetContentTypeMapping()
        {
            return new Dictionary<string, string>
            {
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" },
                { ".jfif", "image/jpeg" },
                { ".JPG", "image/jpeg" },
                { ".JPEG", "image/jpeg" },
                { ".PNG", "image/png" },
                { ".jpg", "image/jpeg" },
            };
        }

    }
}
