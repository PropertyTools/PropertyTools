// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleWindowSource.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.ExampleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;

    /// <summary>
    /// Discovers example windows in the referenced demo assemblies.
    /// The same convention as the DemoLauncher is used: public, non-abstract
    /// <see cref="Window" /> subclasses whose type name ends with "Example".
    /// </summary>
    public static class ExampleWindowSource
    {
        /// <summary>
        /// The names of the demo assemblies to search for example windows.
        /// Keep in sync with the project references of the DemoLauncher.
        /// </summary>
        private static readonly string[] DemoAssemblyNames =
        {
            "ControlDemos",
            "DialogDemos",
            "DataGridDemo",
            "ItemsBagDemo",
            "PropertyGridDemo",
            "PropertyGridDemos",
            "SimpleDemo",
            "CustomFactoryDemo",
            "UndoRedoDemo",
            "TreeListBoxDemo",
            "DirectoryDemo",
            "AddRemoveDemo"
        };

        /// <summary>
        /// Example type full names that are excluded from the automated smoke/snapshot tests,
        /// e.g. examples that require user interaction or external resources at construction time.
        /// </summary>
        private static readonly HashSet<string> ExcludedExamples = new HashSet<string>();

        /// <summary>
        /// Gets all example window types from the referenced demo assemblies.
        /// </summary>
        /// <returns>The example window types.</returns>
        public static IEnumerable<Type> GetExampleWindowTypes()
        {
            var windowType = typeof(Window);
            foreach (var assemblyName in DemoAssemblyNames)
            {
                Assembly assembly;
                try
                {
                    assembly = Assembly.Load(assemblyName);
                }
                catch (Exception)
                {
                    continue;
                }

                var types = assembly.GetTypes()
                    .Where(t => t.Name.EndsWith("Example", StringComparison.Ordinal)
                                && windowType.IsAssignableFrom(t)
                                && !t.IsAbstract
                                && t.IsPublic
                                && t.GetConstructor(Type.EmptyTypes) != null
                                && !ExcludedExamples.Contains(t.FullName))
                    .OrderBy(t => t.FullName);

                foreach (var type in types)
                {
                    yield return type;
                }
            }
        }
    }
}
