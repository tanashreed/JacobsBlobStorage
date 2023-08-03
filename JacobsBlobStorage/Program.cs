// See https://aka.ms/new-console-template for more information.
using Azure;
using JacobsBlobStorage;

try
{
    var blobContainer = await BlobStorageHelper.ConnectToBlobContainerAsync();

    Console.WriteLine("Getting Blobs...");
    List<BlobInfo> blobInfoList = await BlobStorageHelper.GetBlobList(blobContainer!);

    Console.WriteLine($"Total no of Blobs : {blobInfoList.Count}");
    if (blobInfoList != null && blobInfoList.Count > 0)
    {
        Console.WriteLine("Updating Content Type of Blobs...");
        int _blobCount = await BlobStorageHelper.UpdateBlobContentTypes(blobInfoList, blobContainer!);
        if (_blobCount > 0)
        {
            Console.WriteLine($"{_blobCount} Blobs Updated out of {blobInfoList.Count} Blobs...");
            Console.WriteLine("\nGetting refresh data of Blobs...\n");

            List<BlobInfo> updatedBlobList = await BlobStorageHelper.GetBlobList(blobContainer!);
            foreach (BlobInfo blobInfo in updatedBlobList)
            {
                Console.WriteLine($"Blob Name: {blobInfo.Name}  Content-Type:{blobInfo.ContentType}  File Extension:{blobInfo.FileExtension}");
            }

            Console.WriteLine("===========================================\n");
            Console.WriteLine($"{_blobCount} Blobs Updated out of {blobInfoList.Count} Blobs...");
            Console.WriteLine("\n===========================================");
        }
        else
        {
            Console.WriteLine("No eligible Blobs for update...");
        }
    }
    else
    {
        Console.WriteLine("No Blobs found.");
    }
}
catch (RequestFailedException reqEx)
{
    Console.ForegroundColor = ConsoleColor.Red;
    if (reqEx.ErrorCode == "ContainerNotFound")
        Console.WriteLine($"\n{Constants.ErrorMessage} {Constants.NoSuchContainerMessage}\n");
    else
        Console.WriteLine($"\n{Constants.ErrorMessage} {Constants.AuthenticationFailedMessage}\n");
    Console.ResetColor();
}
catch (Exception)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\n{Constants.ErrorMessage} {Constants.NoSuchAccountMessage}\n");
    Console.ResetColor();
}

