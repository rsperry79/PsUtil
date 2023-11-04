using System;

using Utilities.Common.Helpers.Setup;

namespace Utilities.Common.Helpers
{
    /// <summary>
    /// Defines the <see cref="ParseErrorHelper" />.
    /// </summary>
    public static class ParseErrorHelper
    {
        /// <summary>
        /// The ParseError.
        /// </summary>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        public static void ParseError(Exception ex)
        {
            if (ex is null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            if (ex.GetType() == typeof(AggregateException))
            {
                if (!(ex is AggregateException exceptions))
                {
                    throw new ArgumentNullException(nameof(ex));
                }

                foreach (Exception exception in exceptions.InnerExceptions)
                {
                    ParseNonAggregateException(exception);
                }
            }
            else
            {
                ParseNonAggregateException(ex);
            }
        }

        /// <summary>
        /// The ParseNonAggregateException.
        /// </summary>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        private static void ParseNonAggregateException(Exception ex)
        {
            if (ex.InnerException != null)
            {
                if (ex.InnerException.InnerException != null)
                {
                    Log.LogException(ex.InnerException.InnerException);
                }
                else
                {
                    Log.LogException(ex.InnerException);
                }
            }
            else
            {
                Log.LogException(ex);
            }
        }
    }
}
