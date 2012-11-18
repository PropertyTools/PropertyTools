// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsEditableAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the property is editable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IsEditableAttribute : Attribute
    {
    }
}