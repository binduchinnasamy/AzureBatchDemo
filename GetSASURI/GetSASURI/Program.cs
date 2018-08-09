using System;
using System.IO;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace GetSASURI
{
    class Program
    {
        static void Main(string[] args)
        {
            //Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            //Create the blob client object.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            string containerName = CloudConfigurationManager.GetSetting("ContainerName");
            //Get a reference to a container to use for the sample code, and create it if it does not exist.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
            //Insert calls to the methods created below here...
            //Require user input before closing the console window.
            //Generate a SAS URI for the container, without a stored access policy.
            Console.WriteLine("Container SAS URI: " + GetContainerSasUri(container));
            Console.WriteLine();
            Console.ReadLine();
        }
        static string GetContainerSasUri(CloudBlobContainer container)
        {
            //Set the expiry time and permissions for the container.
            //In this case no start time is specified, so the shared access signature becomes valid immediately.
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(24);
            sasConstraints.Permissions = SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Write;
            //Generate the shared access signature on the container, setting the constraints directly on the signature.
            string sasContainerToken = container.GetSharedAccessSignature(sasConstraints);
            //Return the URI string for the container, including the SAS token.
            return container.Uri + sasContainerToken;
        }
    }
}
