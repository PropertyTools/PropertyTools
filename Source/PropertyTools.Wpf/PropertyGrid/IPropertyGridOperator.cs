// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyGridOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines functionality to build the model for a PropertyGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using PropertyTools.Wpf.Operators;
    using System.Collections.Generic;

    /// <summary>
    /// Defines functionality to build the model for a <see cref="PropertyGrid" />.
    /// </summary>
    /// <remarks>
    /// <para>
    /// An <b>operator</b> in PropertyTools is a strategy object that encapsulates data-access and model-building
    /// logic for a control. It acts as an intermediary between the control and its data source, adapting the control's
    /// behavior to different object types without requiring the control itself to be modified.
    /// </para>
    /// <para>
    /// The <see cref="IPropertyGridOperator"/> is the operator interface for the <see cref="PropertyGrid"/> control.
    /// Its primary responsibility is to inspect an object instance — using reflection, data annotations, and custom
    /// attributes — and produce a model of <see cref="Tab"/> objects that describe how the object's properties should
    /// be organized and displayed. This model drives the tabs, categories, and property rows rendered by the
    /// <see cref="PropertyGrid"/>.
    /// </para>
    /// <para>
    /// The default implementation, <see cref="PropertyGridOperator"/>, handles the common case of reflecting over
    /// public properties and respecting standard data annotations. Consumers can supply a custom operator by setting
    /// the <see cref="PropertyGrid.Operator"/> property to change how properties are discovered, filtered, grouped,
    /// or ordered. This follows the <i>Strategy</i> design pattern (GoF), enabling open/closed extensibility — new
    /// property-discovery strategies can be supported by implementing this interface without modifying the
    /// <see cref="PropertyGrid"/> itself.
    /// </para>
    /// <para>
    /// This interface also extends <see cref="ILocalizableOperator"/> and <see cref="ICustomLocalizableOperator"/>,
    /// which provide localization support for translating display strings and descriptions within the control.
    /// </para>
    /// </remarks>
    /// <seealso cref="PropertyGridOperator"/>
    /// <seealso cref="IPropertyGridControlFactory"/>
    public interface IPropertyGridOperator : ILocalizableOperator, ICustomLocalizableOperator
    {
        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="isEnumerable">if set to <c>true</c> enumerable types instances will use the enumerated objects instead of the instance itself.</param>
        /// <param name="options">The options.</param>
        /// <returns>
        /// The tabs.
        /// </returns>
        IEnumerable<Tab> CreateModel(object instance, bool isEnumerable, IPropertyGridOptions options);
    }
}