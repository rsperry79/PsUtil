using CommandLine;

namespace ps.Common.TokenFetcher
{
    // Define a class to receive parsed values
    internal class Options
    {
        [Option(
            "clipboard",
            Required = false,
            Default = false,
            HelpText = "Send to Clipboard?")]
        public bool CopyToClipboard { get; set; }
        [Option(
            "output",
            Required = false,
            Default = "token.txt",
            HelpText = "Token file name")]
        public string TokenFile { get; set; }

        [Option(
            "tosend",
            Required = false,
            HelpText = "To send notification to")]
        public string EmailRecipents { get; set; }

        [Option(
            "dummy",
            Required = false,
            HelpText = "Dummy value to force run")]
        public string Dummy { get; set; }
    }
}
