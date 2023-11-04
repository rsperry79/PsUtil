using CommandLine;

using System;
using System.IO;

using Utilities.Common.Helpers;
using Utilities.Common.TokenHelpers;

namespace ps.Common.TokenFetcher
{
    public class Program
    {
        private static void Main(string[] args)
        {
            FunctionsAssemblyResolver.RedirectAssembly(); // needed for .net standard libraries

            try
            {
                Parser.Default.ParseArguments<ArguementOptions>(args)
                            .WithParsed<ArguementOptions>(options =>
                            {
                                Log.SetLogFileLocation(options.LogFile.Trim());
                                TokenGet getter = new TokenGet();
                                Microsoft.Identity.Client.AuthenticationResult result = getter.GetToken(options.CopyToClipboard, options.EmailRecipents).Result;

                                if (options.CopyToClipboard == true)
                                {
                                    //    TextCopy.Clipboard.SetText(result.IdToken);
                                }
                                else
                                {
                                    getter.DisplaySignedInAccount();
                                    WriteToken(options.TokenFile, result.IdToken);
                                }
                            });

                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                ParseErrorHelper.ParseError(ex);
                Environment.Exit(0);
            }
        }

        private static void WriteToken(string outFile, string token)
        {
            StreamWriter sw = new StreamWriter(outFile);
            sw.WriteLine(token);
            sw.Close();
        }
    }
}
