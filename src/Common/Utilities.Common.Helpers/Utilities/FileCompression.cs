using System;
using System.IO;
using System.IO.Compression;

namespace Utilities.Common.Helpers.Utilities
{
    /// <summary>
    /// Helper class to compress/decompress a file.
    /// </summary>
    public static class FileCompression
    {
        /// <summary>
        /// Compresses a file.
        /// </summary>
        /// <param name="inputFileName">File to compress.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Compress(string inputFileName)
        {
            string outfileName = null;
            if (inputFileName != null)
            {
                FileInfo fileInfo = new FileInfo(inputFileName);
                // Get the stream of the source file.
                using (FileStream inFile = fileInfo.OpenRead())
                {
                    // Prevent compressing hidden and
                    // already compressed files.
                    if ((File.GetAttributes(fileInfo.FullName)
                        & FileAttributes.Hidden)
                        != FileAttributes.Hidden & string.Compare(fileInfo.Extension, ".gz", StringComparison.Ordinal) != 0)
                    {
                        // Create the compressed file.
                        outfileName = $"{fileInfo.Name}.gz";
                        using (FileStream outFile = File.Create(outfileName))
                        {
                            using (GZipStream compress =
                                new GZipStream(
                                    outFile,
                                    CompressionMode.Compress))
                            {
                                // Copy the source file into
                                // the compression stream.
                                inFile.CopyTo(compress);

                                Console.WriteLine($"Compressed {fileInfo.Name} from {fileInfo.Length} to {outFile.Length} bytes.");
                            }
                        }
                    }
                }
            }

            return outfileName;
        }

        /// <summary>
        /// Decompress a file.
        /// </summary>
        /// <param name="fileName">Compressed file.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Decompress(string fileName)
        {
            string outputFileName = null;

            if (fileName != null)
            {
                outputFileName = Path.GetFileNameWithoutExtension(fileName);
                FileInfo fileInfo = new FileInfo(fileName);

                using (Stream fd = File.Create(outputFileName))
                using (Stream fs = fileInfo.OpenRead())
                using (Stream csStream = new GZipStream(fs, CompressionMode.Decompress))
                {
                    byte[] buffer = new byte[1024];
                    int nRead;
                    while ((nRead = csStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fd.Write(buffer, 0, nRead);
                    }
                }
            }

            return outputFileName;
        }
    }
}
