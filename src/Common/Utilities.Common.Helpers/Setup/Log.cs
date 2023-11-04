using Newtonsoft.Json;

using NLog;
using NLog.Config;
using NLog.Targets;

using System;
using System.IO;
using System.Linq;

namespace Utilities.Common.Helpers.Setup
{
    /// <summary>
    /// Defines the <see cref="Log" />.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Defines the Config.
        /// </summary>
        private static readonly LoggingConfiguration Config;

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private static Logger logger;

        /// <summary>
        /// Defines the memoryTarget.
        /// </summary>
        private static MemoryTarget memoryTarget;

        /// <summary>
        /// Defines the log file.
        /// </summary>
        private static FileTarget logfile;

        /// <summary>
        /// Defines the log console.
        /// </summary>
        private static ColoredConsoleTarget logconsole;

        /// <summary>
        /// Initializes static members of the <see cref="Log"/> class.
        /// </summary>
        static Log()
        {
            Config = new LoggingConfiguration();
            Config.LoggingRules.Clear();

            LoadMemoryLogger();
            LoadConsoleLogger();

            ReconfigureLogger();
        }

        /// <summary>
        /// The SetLogFileLocation.
        /// </summary>
        /// <param name="location">The location<see cref="string"/>.</param>
        public static void SetLogFileLocation(string location)
        {
            if (location != null)
            {
                logfile = new FileTarget("logfile") { FileName = location };
                Config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            }
        }

        /// <summary>
        /// The Warn.
        /// </summary>
        /// <param name="warning">The warning<see cref="string"/>.</param>
        public static void Warn(string warning)
        {
            logger.Warn(warning);
        }

        /// <summary>
        /// The Info.
        /// </summary>
        /// <param name="info">The info<see cref="string"/>.</param>
        public static void Info(string info)
        {
            logger.Info(info);
        }

        /// <summary>
        /// The Debug.
        /// </summary>
        /// <param name="debug">The debug<see cref="string"/>.</param>
        public static void Debug(string debug)
        {
            logger.Debug(debug);
        }

        /// <summary>
        /// The Error.
        /// </summary>
        /// <param name="error">The error<see cref="string"/>.</param>
        public static void Error(string error)
        {
            logger.Error(error);
        }

        /// <summary>
        /// The Write.
        /// </summary>
        /// <param name="fileLocation">The fileLocation<see cref="string"/>.</param>
        public static void Write(string fileLocation)
        {
            string[] tempErrors = memoryTarget.Logs.ToArray() ?? Array.Empty<string>();
            File.WriteAllLines(fileLocation, tempErrors);
        }

        /// <summary>
        /// The WriteJson.
        /// </summary>
        /// <param name="fileLocation">The fileLocation<see cref="string"/>.</param>
        public static void WriteJson(string fileLocation)
        {
            File.WriteAllText(fileLocation, ToJson());
        }

        /// <summary>
        /// The ToArray.
        /// </summary>
        /// <returns>The <see cref="string[]"/>.</returns>
        public static string[] ToArray()
        {
            return memoryTarget.Logs.ToArray() ?? Array.Empty<string>();
        }

        /// <summary>
        /// The ToJson.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToJson()
        {
            string[] tempErrors = memoryTarget.Logs.ToArray() ?? Array.Empty<string>();
            string output = JsonConvert.SerializeObject(tempErrors, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });

            return output;
        }

        /// <summary>
        /// The LogException.
        /// </summary>
        /// <param name="exception">The exception to be logged<see cref="Exception"/>.</param>
        public static void LogException(Exception exception)
        {
            if (exception == null)
            {
                throw new ApplicationException("Attempted to log an null exception.");
            }

            if (exception.InnerException != null)
            {
                if (exception.InnerException.InnerException != null)
                {
                    logger.Error(exception.InnerException.InnerException);
                }
                else
                {
                    logger.Error(exception.InnerException);
                }
            }
            else
            {
                logger.Error(exception);
            }
        }

        /// <summary>
        /// The LoadMemoryLogger.
        /// </summary>
        private static void LoadMemoryLogger()
        {
            memoryTarget = new MemoryTarget
            {
                Layout = "${longdate:universalTime=true} ${message}"
            };
            Config.AddRule(LogLevel.Info, LogLevel.Fatal, memoryTarget);
        }

        /// <summary>
        /// The LoadConsoleLogger.
        /// </summary>
        private static void LoadConsoleLogger()
        {
            logconsole = new ColoredConsoleTarget("logconsole")
            {
                Layout = "${message}"
            };

            Config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole);
        }

        /// <summary>
        /// The ReconfigureLogger.
        /// </summary>
        private static void ReconfigureLogger()
        {
            LogManager.ThrowExceptions = false;
            LogManager.ThrowConfigExceptions = false;
            LogManager.Configuration = Config;
            LogManager.ReconfigExistingLoggers();
            logger = LogManager.GetCurrentClassLogger();
        }
    }
}
