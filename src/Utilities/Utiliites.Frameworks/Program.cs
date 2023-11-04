
using CommandLine;

using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.IO;

namespace Frameworks
{
    internal class Program
    {
        private static List<string> std;
        private static List<string> core;

        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(options =>
            {
                std = GetStdFrameworks();
                core = GetCoreFrameworks();
                File.WriteAllLines(options.StdResults, std);
                File.WriteAllLines(options.CoreResults, core);
            });


        }

        private static List<string> GetStdFrameworks()
        {
            string path = @"SOFTWARE\Microsoft\NET Framework Setup\NDP";
            List<string> display_framwork_name = new List<string>();

            RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(path);
            string[] version_names = installed_versions.GetSubKeyNames();

            for (int i = 1; i <= version_names.Length - 1; i++)
            {
                string temp_name = "Microsoft .NET Framework " + version_names[i].ToString() + "  SP" + installed_versions.OpenSubKey(version_names[i]).GetValue("SP");
                display_framwork_name.Add(temp_name);
            }

            return display_framwork_name;
        }

        private static List<string> GetCoreFrameworks()
        {
            List<string> display_framwork_name = new List<string>();
            try
            {
                string path = @"SOFTWARE\WOW6432Node\dotnet\Setup\InstalledVersions\x64\sharedfx\Microsoft.NETCore.App";

                RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(path);
                string[] version_names = installed_versions.GetSubKeyNames();

                for (int i = 1; i <= version_names.Length - 1; i++)
                {
                    string temp_name = "Microsoft .NET Core Framework " + version_names[i].ToString() + "  SP" + installed_versions.OpenSubKey(version_names[i]).GetValue("SP");
                    display_framwork_name.Add(temp_name);
                }
            }
            catch (Exception)
            {
                // Swallow errors
            }

            return display_framwork_name;
        }
    }
}
