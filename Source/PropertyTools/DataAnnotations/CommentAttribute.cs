// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the value is a comment.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CommentAttribute : Attribute
    {
    }
}