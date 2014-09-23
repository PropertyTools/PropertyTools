// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies that the value is a comment.
// </summary>
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