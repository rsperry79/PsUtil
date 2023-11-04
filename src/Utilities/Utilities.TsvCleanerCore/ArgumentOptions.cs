using CommandLine;

namespace Psu.Tools.ps
{
    /// <summary>
    /// Defines the <see cref="ArgumentOptions" />.
    /// </summary>
    internal class ArgumentOptions
    {
        /// <summary>
        /// Gets or sets the InputFile
        /// Input File.
        /// </summary>
        [Option(
            'i',
            "Input",
            Required = true,
            HelpText = "Input File")]
        public string InputFile { get; set; }

        /// <summary>
        /// Gets or sets the Parallel
        /// Input File.
        /// </summary>
        [Option(
            'p',
            "Parallel",
            Required = false,
            Default = "true",
            HelpText = "Run in Parallel")]
        public string Parallel { get; set; }

        /// <summary>
        /// Gets or sets the ResultsFile
        /// sets the ResultsFile.
        /// </summary>
        [Option(
            'r',
            "Results",
            Required = true,
            HelpText = "Results file name")]
        public string ResultsFile { get; set; }

        /// <summary>
        /// Gets or sets the LogFile.
        /// </summary>
        [Option(
            'l',
            "LogFile",
            Required = false,
            Default = "LogFile.txt",
            HelpText = "The name of the output log file")]
        public string LogFile { get; set; }

        /// <summary>
        /// Gets or sets the Force
        /// sets the Force.
        /// </summary>
        [Option(
            'f',
            "Force",
            Required = false,
            Default = "RunItId",
            HelpText = "Force run id")]
        public string Force { get; set; }
    }
}
