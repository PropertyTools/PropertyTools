// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleDiscovery.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace DemoLauncher
{
    /// <summary>
    /// Discovers example windows from assemblies.
    /// </summary>
    public class ExampleDiscovery
    {
        /// <summary>
        /// Discovers all example windows from the specified directory and subdirectories.
        /// </summary>
        /// <param name="searchPath">The path to search for assemblies.</param>
        /// <returns>A list of discovered examples.</returns>
        public static List<ExampleInfo> DiscoverExamples(string searchPath = null)
        {
            var examples = new List<ExampleInfo>();
            
            // Default to the application directory if no search path specified
            if (string.IsNullOrEmpty(searchPath))
            {
                searchPath = AppDomain.CurrentDomain.BaseDirectory;
            }

            // Get all DLL files in the directory and subdirectories
            var assemblyFiles = Directory.GetFiles(searchPath, "*.dll", SearchOption.AllDirectories)
                .Where(f => !f.Contains("\\ref\\")) // Exclude reference assemblies
                .ToList();

            // Also check the current directory
            assemblyFiles.AddRange(Directory.GetFiles(searchPath, "*.exe", SearchOption.TopDirectoryOnly));

            foreach (var assemblyFile in assemblyFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(assemblyFile);
                    var examplesInAssembly = DiscoverExamplesInAssembly(assembly);
                    examples.AddRange(examplesInAssembly);
                }
                catch
                {
                    // Skip assemblies that can't be loaded
                }
            }

            return examples.OrderBy(e => e.AssemblyName).ThenBy(e => e.Title).ToList();
        }

        /// <summary>
        /// Discovers example windows in a specific assembly.
        /// </summary>
        /// <param name="assembly">The assembly to search.</param>
        /// <returns>A list of discovered examples.</returns>
        public static List<ExampleInfo> DiscoverExamplesInAssembly(Assembly assembly)
        {
            var examples = new List<ExampleInfo>();
            var windowType = typeof(Window);

            try
            {
                var types = assembly.GetTypes()
                    .Where(t => t.Name.EndsWith("Example") && 
                                windowType.IsAssignableFrom(t) &&
                                !t.IsAbstract &&
                                t.IsPublic);

                foreach (var type in types)
                {
                    try
                    {
                        examples.Add(new ExampleInfo(type));
                    }
                    catch
                    {
                        // Skip types that can't be instantiated
                    }
                }
            }
            catch
            {
                // Skip assemblies that throw exceptions during type enumeration
            }

            return examples;
        }
    }
}
