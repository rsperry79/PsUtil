﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Utilities.Common.Helpers
{
    /// <summary>
    /// Defines the <see cref="CoreUtils" />.
    /// </summary>
    public static class CoreUtils
    {
        /// <summary>
        /// The OpenDeviceCode.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        /// <param name="code">The code<see cref="string"/>.</param>
        public static void OpenDeviceCode(string url, string code)
        {
            // uses https://github.com/SimonCropp/TextCopy/
            //  TextCopy.Clipboard.SetText(code);
            OpenURl(url);
        }

        /// <summary>
        /// The OpenURl.
        /// </summary>
        /// <param name="url">The url<see cref="string"/>.</param>
        public static void OpenURl(string url)
        {
            if (url == null)
            {
                throw new ArgumentException(typeof(string).ToString());
            }

            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
