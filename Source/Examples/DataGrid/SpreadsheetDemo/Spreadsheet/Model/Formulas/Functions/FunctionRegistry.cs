// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionRegistry.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   A case-insensitive registry of formula functions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet.Model.Formulas.Functions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A case-insensitive registry of formula functions.
    /// </summary>
    public sealed class FunctionRegistry
    {
        private readonly Dictionary<string, IFunction> functions = new Dictionary<string, IFunction>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Registers a function, replacing any existing function with the same name.
        /// </summary>
        public void Register(IFunction function)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            this.functions[function.Name] = function;
        }

        /// <summary>
        /// Registers a function implemented by a delegate.
        /// </summary>
        /// <param name="name">The function name.</param>
        /// <param name="minArgumentCount">The minimum number of arguments.</param>
        /// <param name="maxArgumentCount">The maximum number of arguments, or -1 for unlimited.</param>
        /// <param name="invoke">The implementation.</param>
        public void Register(string name, int minArgumentCount, int maxArgumentCount, Func<FunctionCallContext, CellValue> invoke)
        {
            this.Register(new DelegateFunction(name, minArgumentCount, maxArgumentCount, invoke));
        }

        /// <summary>
        /// Tries to find a function by name (case-insensitive).
        /// </summary>
        public bool TryGet(string name, out IFunction function)
        {
            return this.functions.TryGetValue(name, out function);
        }

        /// <summary>
        /// Creates a registry with all built-in functions registered.
        /// </summary>
        public static FunctionRegistry CreateDefault()
        {
            var registry = new FunctionRegistry();
            MathFunctions.RegisterAll(registry);
            StatisticalFunctions.RegisterAll(registry);
            LogicalFunctions.RegisterAll(registry);
            TextFunctions.RegisterAll(registry);
            DateFunctions.RegisterAll(registry);
            InformationFunctions.RegisterAll(registry);
            return registry;
        }
    }
}
