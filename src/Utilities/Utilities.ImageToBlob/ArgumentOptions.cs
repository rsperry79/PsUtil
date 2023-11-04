using CommandLine;

namespace Utilities.ImageToBlob
{
    /// <summary>
    /// Defines the <see cref="ArgumentOptions" />.
    /// </summary>
    internal class ArgumentOptions
    {
        [Option(
            'b',
            "BlobContainerName",
            Required = true,
            HelpText = "Blob container name")]
        public string ContainerName { get; set; }

        [Option(
            'k',
            "AccountKey",
            Required = true,
            HelpText = "Blob container AccountKey")]
        public string AccountKey { get; set; }

        [Option(
            'a',
            "AccountName",
            Required = true,
            HelpText = "Blob Storage AccountName")]
        public string AccountName { get; set; }

        [Option(
            'i',
            "InputFileName",
            Required = true,
            HelpText = "Input File Name")]
        public string FileName { get; set; }

        [Option(
            's',
            "SetfolderName",
            Required = true,
            HelpText = "Set subfolder name")]
        public string SetFolder { get; set; }

        [Option(
            'p',
            "ImagePosition",
            Required = true,
            HelpText = "The 0 based array location of the image URL in the row.")]
        public string ImagePosition { get; set; }

        [Option(
            'l',
            "LogFile",
            Required = false,
            Default = "LogFile.txt",
            HelpText = "The name of the output log file")]
        public string LogFile { get; set; }

        [Option(
            'r',
            "results",
            Required = false,
            Default = "Results.tsv",
            HelpText = "Results file name")]
        public string ResultsFile { get; set; }
    }
}
