using System;
using System.Linq;
using System.Reflection;

namespace Utilities.Common.Helpers
{
    /// <summary>
    /// Defines the <see cref="FunctionsAssemblyResolver" />.
    /// </summary>
    public static class FunctionsAssemblyResolver
    {
        /// <summary>
        /// The RedirectAssembly.
        /// </summary>
        public static void RedirectAssembly()
        {
            System.Collections.Generic.List<string> list = AppDomain.CurrentDomain.GetAssemblies().OrderByDescending(a => a.FullName)
                .Select(a => a.FullName)
                .ToList();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        /// <summary>
        /// The CurrentDomain_AssemblyResolve.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/>.</param>
        /// <param name="args">The args<see cref="ResolveEventArgs"/>.</param>
        /// <returns>The <see cref="Assembly"/>.</returns>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName requestedAssembly = new AssemblyName(args.Name);
            Assembly assembly = null;
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            try
            {
                assembly = Assembly.Load(requestedAssembly.Name);
            }
            catch (Exception)
            {
            }

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            return assembly;
        }
    }
}
