using CommandLine;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Utilities.Common.Helpers;

namespace Utilities.ImageToBlob
{
    internal class Program
    {
        /// <summary>
        /// An address to resolve filename.
        /// </summary>
        private static readonly Uri SomeBaseUri = new Uri("http://localhost");

        private static CloudStorageAccount storageAccount;
        private static CloudBlobContainer blobContainer;

        private static void Main(string[] args)
        {
            FunctionsAssemblyResolver.RedirectAssembly(); // needed for .net standard libraries
            string baseDir = Environment.CurrentDirectory;
            try
            {
                Parser.Default.ParseArguments<ArgumentOptions>(args)
                         .WithParsed(options =>
                         {
                             Log.SetLogFileLocation(options.LogFile.Trim());

                             // Called before we change working directory
                             string[] input = File.ReadAllLines(options.FileName);

                             CreateWorkingDirectory();
                             SetupBlobStorage(options);

                             ConcurrentBag<string> proccessedLines = new ConcurrentBag<string>();
                             Parallel.ForEach(input, line =>
                             {
                                 string proccessed = ProcessAsync(line, int.Parse(options.ImagePosition, CultureInfo.InvariantCulture), options.SetFolder).Result;
                                 if (proccessed != null)
                                 {
                                     proccessedLines.Add(proccessed);
                                 }
                             });

                             // Go back to the base dir and write the results
                             Directory.SetCurrentDirectory(baseDir);
                             File.WriteAllLines(options.ResultsFile, proccessedLines.ToArray());

                             // Write logfile
                             Log.Write(options.LogFile);
                         });

                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                ParseErrorHelper.ParseError(ex);
                Environment.Exit(0);
            }
        }

        private static void CreateWorkingDirectory()
        {
            string imagesFolder = Path.Combine(Environment.CurrentDirectory, "Images");
            _ = Directory.CreateDirectory(imagesFolder);
            Directory.SetCurrentDirectory(imagesFolder);
        }

        private static void SetupBlobStorage(ArgumentOptions options)
        {
            StorageCredentials credentials = new StorageCredentials(options.AccountName, options.AccountKey);
            storageAccount = new CloudStorageAccount(credentials, useHttps: true);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(options.ContainerName);
        }

        private static async Task<string> ProcessAsync(string line, int postionOfImage, string setFolder)
        {
            if (string.IsNullOrEmpty(line.Trim()))
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            string[] fields = line.Split('\t');

            for (int index = 0; index < fields.Length; index++)
            {
                string processed;

                if (index == postionOfImage)
                {
                    Uri uri = await PutImageAsync(fields[index], setFolder).ConfigureAwait(false);
                    if (uri == null)
                    {
                        break;
                    }

                    processed = uri.ToString();
                }
                else
                {
                    processed = fields[index];
                }

                sb.Append($"{processed}");
                if (index != fields.Length - 1)
                {
                    sb.Append("\t");
                }
            }

            return sb.ToString();
        }

        private static async Task<Uri> PutImageAsync(string imageUrl, string setfolder)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string fileName = GetFileNameFromUrl(imageUrl);
                    client.DownloadFile(new Uri(imageUrl), fileName);

                    Guid guid = Guid.NewGuid();
                    string extension = Path.GetExtension(fileName);

                    // This also does not make a service call, it only creates a local object.
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference($"{setfolder}/{guid}{extension}");

                    // This transfers data in the file to the blob on the service.
                    await blob.UploadFromFileAsync(fileName).ConfigureAwait(false);

                    return blob.Uri;
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
            }

            return null;
        }

        private static string GetFileNameFromUrl(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                uri = new Uri(SomeBaseUri, url);
            }

            return Path.GetFileName(uri.LocalPath);
        }
    }
}
