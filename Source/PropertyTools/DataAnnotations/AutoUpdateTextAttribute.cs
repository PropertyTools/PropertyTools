// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoUpdateTextAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the text binding should be triggered at every change.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoUpdateTextAttribute : Attribute
    {
    }
}