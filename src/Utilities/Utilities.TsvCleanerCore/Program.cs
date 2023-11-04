using CommandLine;

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Utilities.Common.Helpers.Setup;
using Utilities.Common.Helpers.Utilities;

namespace Utilities.TsvCleanerCore
{
    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The Main.
        /// </summary>
        /// <param name="args">The args<see cref="string[]"/>.</param>
        private static void Main(string[] args)
        {
            FunctionsAssemblyResolver.RedirectAssembly(); // needed for .net standard libraries

            try
            {
                _ = Parser.Default.ParseArguments<ArgumentOptions>(args)
                         .WithParsed(options =>
                         {
                             Log.SetLogFileLocation(options.LogFile.Trim());

                             if (File.Exists(options.InputFile))
                             {
                                 // thread-safe list
                                 ConcurrentBag<string> cleanedLines = new ConcurrentBag<string>();

                                 if (bool.Parse(options.Parallel))
                                 {
                                     _ = Parallel.ForEach(File.ReadAllLines(options.InputFile), line =>
                                     {
                                         string clean = Clean(line);
                                         if (clean != null)
                                         {
                                             cleanedLines.Add(clean);
                                         }
                                     });
                                 }
                                 else
                                 {
                                     string[] lines = File.ReadAllLines(options.InputFile);

                                     foreach (string line in lines)
                                     {
                                         string clean = Clean(line);

                                         if (clean != null)
                                         {
                                             cleanedLines.Add(clean);
                                         }
                                     }
                                 }

                                 File.WriteAllLines(Path.Combine(Environment.CurrentDirectory, options.ResultsFile), cleanedLines.ToArray());
                             }
                         }).WithNotParsed(error =>
                         {
                             // Catch failure to parse errors that do not cause an exception so we can see them in ps.
                             Environment.Exit(0);
                         });

                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Common.Helpers.ParseErrorHelper.ParseError(ex);
                Environment.Exit(0);
            }
        }

        private static string Clean(string line)
        {
            if (string.IsNullOrEmpty(line.Trim()))
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            string[] fields = line.Split('\t');

            for (int index = 0; index < fields.Length; index++)
            {
                string cleaned = StringCleaner.CleanControlAndSurrogates(fields[index]);
                _ = sb.Append($"{cleaned}");
                if (index != fields.Length - 1)
                {
                    _ = sb.Append("\t");
                }
            }

            return sb.ToString();
        }
    }
}
