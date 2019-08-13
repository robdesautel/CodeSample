using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PoolReservation.Infrastructure.Storage
{
    public class AzureStorageManager
    {
        private CloudStorageAccount storageAccount;
        private string containerName;
        public AzureStorageManager()
        {
            this.storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);

            containerName = ConfigurationManager.AppSettings["StorageContainer"];
        }

        public async Task UploadFile(string filename, byte[] contents)
        {
            var client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = client.GetContainerReference(containerName);

            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(filename);

            await blockBlob.UploadFromByteArrayAsync(contents, 0, contents.Length); 
        }

        public async Task<byte[]>  GetFileAsync(string filename)
        {
            var client = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = client.GetContainerReference(containerName);

            CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(filename);
            
            if(blockBlob == null)
            {
                return null;
            }

            if (blockBlob.Exists())
            {
                await blockBlob.FetchAttributesAsync();
            }else
            {
                return null;
            }

            if (blockBlob?.Properties?.Length <= 0)
            {
                return null;
            }

            var dest = new byte[blockBlob.Properties.Length];
            await blockBlob.DownloadToByteArrayAsync(dest, 0);
            return dest;
        }

        public async Task MakeCopyOfItemInStorage(string fileName, string newFileName)
        {

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer sourceContainer = blobClient.GetContainerReference(containerName);
            CloudBlobContainer targetContainer = blobClient.GetContainerReference(containerName);
            CloudBlockBlob sourceBlob = sourceContainer.GetBlockBlobReference(fileName);
            CloudBlockBlob targetBlob = targetContainer.GetBlockBlobReference(newFileName);

            await targetBlob.StartCopyAsync(sourceBlob);
        }

        public async Task DeleteItemFromStorageIfExists(string fileName)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer theContainer = blobClient.GetContainerReference(containerName);

            var blob = theContainer.GetBlockBlobReference(fileName);

            await blob.DeleteIfExistsAsync();
        }




    }
}