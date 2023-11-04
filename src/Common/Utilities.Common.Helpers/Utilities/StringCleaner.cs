using System.Collections.Generic;

namespace Utilities.Common.Helpers
{
    /// <summary>
    /// Defines the <see cref="StringCleaner" />.
    /// </summary>
    public static class StringCleaner
    {
        /// <summary>
        /// The CleanControlAndSurrogates.
        /// </summary>
        /// <param name="toClean">The toClean<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string CleanControlAndSurrogates(string toClean, bool firstline = false)
        {
            if (toClean != null)
            {
                char[] dirtyArray;
                List<char> clean = new List<char>();
                dirtyArray = toClean.ToCharArray();
                bool isInt = int.TryParse(toClean, out int number);

                bool isDouble = double.TryParse(toClean, out double dNumber);


                if (!firstline && !isDouble && !isInt)
                {
                    clean.Add('\"');
                }

                if (isDouble || isInt)
                {
                    if (isDouble)
                    {
                        return dNumber.ToString();
                    }

                    if (isInt)
                    {
                        return number.ToString();
                    }
                }


                foreach (char letter in dirtyArray)
                {

                    if (!char.IsControl(letter) && !char.IsSurrogate(letter))
                    {

                        clean.Add(letter);
                    }
                }

                if (!firstline && !isDouble && !isInt)
                {
                    clean.Add('\"');
                }


                return new string(clean.ToArray());
            }

            return null;
        }
    }
}
