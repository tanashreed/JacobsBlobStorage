
namespace JacobsBlobStorage
{
    internal class Constants
    {
        public const string DefaultContentType = "application/octet-stream";
        public const string AuthenticationFailedMessage = "Authentication failed.";
        public const string NoSuchAccountMessage = "No such account/host name exists.";
        public const string ErrorMessage = "Error retrieving blobs from Azure Blob Storage:";
        public const string NoSuchContainerMessage = "Container not found or does not exist.";

        // Connect to blob container parameters.
        public const string AccountName = "MyAccountName";
        public const string ContainerName = "MyContainer";
        public const string SasToken = "MyToken";
    }
}
