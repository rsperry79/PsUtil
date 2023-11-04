using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Utilities.TsvCleanerCore.Tests
{
    /// <summary>
    /// Defines the <see cref="TsvCleanerTests" />.
    /// </summary>
    [TestClass]
    public class TsvCleanerTests
    {
        /// <summary>
        /// Defines the AssemblyPath.
        /// </summary>
        private const string AssemblyPath = "TsvCleaner.exe";

        /// <summary>
        /// Defines the SpecialTestFile.
        /// </summary>
        private const string SpecialTestFile = "_test_with_special_char_and_qoute_issue.tsv";

        /// <summary>
        /// Defines the SpecialTestFileCount.
        /// </summary>
        private const int SpecialTestFileCount = 40;

        /// <summary>
        /// Defines the LargeTestFile.
        /// </summary>
        private const string LargeTestFile = "_large_test_file.tsv";

        /// <summary>
        /// Defines the LargeTestFileCount.
        /// </summary>
        private const int LargeTestFileCount = 3167;

        /// <summary>
        /// Tests a file with know surrogates and control chars using a parallel foreach method.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task TsvCleanerValidateReturnParallelWithSpecialAsync()
        {
            int count = 0;
            string outputFile = "specialTestResultsParallel.tsv";

            // remove existing output if it exists
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            int code = await RunProcessAsync(testInputFile: SpecialTestFile, outputpath: outputFile, parallel: true).ConfigureAwait(false);
            if (code == 1)
            {
                string[] lines = File.ReadAllLines(outputFile);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        count++;
                    }
                }

                Assert.AreEqual(SpecialTestFileCount, count);
            }
            else
            {
                Assert.Fail("A failure code was returned.");
            }
        }

        /// <summary>
        /// Tests a file with know surrogates and control chars using a standard foreach method.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task TsvCleanerValidateReturnStandardWithSpecialAsync()
        {
            int count = 0;
            string outputFile = "specialTestResultStandard.tsv";

            // remove existing output if it exists
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            int code = await RunProcessAsync(testInputFile: SpecialTestFile, outputpath: outputFile, parallel: false).ConfigureAwait(false);
            if (code == 1)
            {
                string[] lines = File.ReadAllLines(outputFile);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        count++;
                    }
                }

                Assert.AreEqual(SpecialTestFileCount, count);
            }
            else
            {
                Assert.Fail("A failure code was retuned.");
            }
        }

        /// <summary>
        /// Tests a parallel foreach method with a larger file.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task TsvCleanerValidateReturnParallelWithLarge()
        {
            int count = 0;
            string outputFile = "largeTestResultsParallel.tsv";

            // remove existing output if it exists
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            int code = await RunProcessAsync(testInputFile: LargeTestFile, outputpath: outputFile, parallel: true).ConfigureAwait(false);
            if (code == 1)
            {
                string[] lines = File.ReadAllLines(outputFile);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        count++;
                    }
                }

                Assert.AreEqual(LargeTestFileCount, count);
            }
            else
            {
                Assert.Fail("A failure code was retuned.");
            }
        }

        /// <summary>
        /// Tests standard foreach method with a larger file.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        [TestMethod]
        public async Task TsvCleanerValidateReturnStandardWithLargeAsync()
        {
            int count = 0;
            string outputFile = "largeTestResultsStandard.tsv";

            // remove existing output if it exists
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            int code = await RunProcessAsync(testInputFile: LargeTestFile, outputpath: outputFile, parallel: false).ConfigureAwait(false);

            if (code == 1)
            {
                string[] lines = File.ReadAllLines(outputFile);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        count++;
                    }
                }

                Assert.AreEqual(LargeTestFileCount, count);
            }
            else
            {
                Assert.Fail("A failure code was retuned.");
            }
        }

        /// <summary>
        /// The RunProcessAsync.
        /// </summary>
        /// <param name="testInputFile">The testInputFile<see cref="string"/>.</param>
        /// <param name="outputpath">The outputpath<see cref="string"/>.</param>
        /// <param name="parallel">The parallel<see cref="bool"/>.</param>
        /// <returns>The <see cref="Task{int}"/>.</returns>
        private static Task<int> RunProcessAsync(string testInputFile, string outputpath, bool parallel)
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();

#pragma warning disable CA2000 // Dispose objects before losing scope
            Process process = new Process
            {
                StartInfo =
                {
                    FileName = AssemblyPath,
                    Arguments = $"-i \"{testInputFile}\" -r \"{outputpath}\" -p \"{parallel.ToString(CultureInfo.InvariantCulture)}\"",
                },
                EnableRaisingEvents = true,
            };
#pragma warning restore CA2000 // Dispose objects before losing scope

            process.Exited += (sender, args) =>
            {
                tcs.SetResult(process.ExitCode);
                process.Dispose();
            };

            _ = process.Start();

            return tcs.Task;
        }
    }
}
