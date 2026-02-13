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

            // First, try to discover from the current directory
            examples.AddRange(DiscoverExamplesInDirectory(searchPath));

            // If we're in a DemoLauncher output directory, also look in sibling example directories
            if (searchPath.Contains("DemoLauncher"))
            {
                // Navigate up to the Examples directory
                var examplesDir = FindExamplesDirectory(searchPath);
                if (!string.IsNullOrEmpty(examplesDir) && Directory.Exists(examplesDir))
                {
                    // Search in all subdirectories of Examples for both Debug and Release builds
                    var subDirs = Directory.GetDirectories(examplesDir, "*", SearchOption.AllDirectories)
                        .Where(d => d.Contains("\\bin\\Debug\\") || d.Contains("/bin/Debug/") ||
                                    d.Contains("\\bin\\Release\\") || d.Contains("/bin/Release/"))
                        .Where(d => !d.Contains("DemoLauncher"));

                    foreach (var dir in subDirs)
                    {
                        examples.AddRange(DiscoverExamplesInDirectory(dir));
                    }
                }
            }

            return examples.Distinct().OrderBy(e => e.AssemblyName).ThenBy(e => e.Title).ToList();
        }

        private static string FindExamplesDirectory(string currentPath)
        {
            var dir = new DirectoryInfo(currentPath);
            while (dir != null)
            {
                if (dir.Name.Equals("Examples", StringComparison.OrdinalIgnoreCase))
                {
                    return dir.FullName;
                }
                dir = dir.Parent;
            }
            return null;
        }

        private static List<ExampleInfo> DiscoverExamplesInDirectory(string directory)
        {
            var examples = new List<ExampleInfo>();

            // Get all DLL files in the directory
            var assemblyFiles = Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly)
                .Where(f => !f.Contains("\\ref\\") && !f.Contains("/ref/")) // Exclude reference assemblies
                .Where(f => !Path.GetFileName(f).StartsWith("System.", StringComparison.OrdinalIgnoreCase))
                .Where(f => !Path.GetFileName(f).StartsWith("Microsoft.", StringComparison.OrdinalIgnoreCase))
                .Where(f => !Path.GetFileName(f).Equals("PropertyTools.dll", StringComparison.OrdinalIgnoreCase))
                .Where(f => !Path.GetFileName(f).Equals("PropertyTools.Wpf.dll", StringComparison.OrdinalIgnoreCase))
                .Where(f => !Path.GetFileName(f).Equals("DemoLauncher.dll", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Also check EXE files
            assemblyFiles.AddRange(Directory.GetFiles(directory, "*.exe", SearchOption.TopDirectoryOnly)
                .Where(f => !Path.GetFileName(f).Equals("DemoLauncher.exe", StringComparison.OrdinalIgnoreCase)));

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

            return examples;
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
