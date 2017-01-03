// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class CommentAttributeExample : Example
    {
        [Comment]
        public string Comment1 { get; private set; }

        [Comment]
        public string Comment2 { get; private set; }

        [Comment]
        public string Comment3 { get; private set; }

        [Comment]
        [HeaderPlacement(HeaderPlacement.Hidden)]
        public string Comment4 { get; private set; }

        [Comment]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public string Comment5 { get; private set; }

        public CommentAttributeExample()
        {
            this.Comment1 = "This is a comment.";
            this.Comment2 = "This is a multiline\ncomment.";
            this.Comment3 = "This is a wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping wrapping comment.";
            this.Comment4 = "Comment with hidden header";
            this.Comment5 = "Comment with collapsed header";
        }
    }
}