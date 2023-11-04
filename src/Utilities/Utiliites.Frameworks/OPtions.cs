using CommandLine;

namespace Frameworks
{
    internal class Options
    {
        [Option(
        's',
        "Stanard",
        Required = true,
        HelpText = "Standard Results File"
        )]
        public string StdResults { get; set; }

        [Option(
        'c',
        "Core",
        Required = true,
        HelpText = "Core Results File"
        )]
        public string CoreResults { get; set; }
    }
}

