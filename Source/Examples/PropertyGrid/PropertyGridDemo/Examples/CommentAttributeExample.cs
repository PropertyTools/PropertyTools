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
        private string comment1;
        private string comment2;
        private string comment3;
        private string comment4;
        private string comment5;

        [Comment]
        public string Comment1 { get => this.comment1; private set { this.comment1 = value; this.RaisePropertyChanged(nameof(Comment1)); } }

        [Comment]
        public string Comment2 { get => this.comment2; private set { this.comment2 = value; this.RaisePropertyChanged(nameof(Comment2)); } }

        [Comment]
        public string Comment3 { get => this.comment3; private set { this.comment3 = value; this.RaisePropertyChanged(nameof(Comment3)); } }

        [Comment]
        [HeaderPlacement(HeaderPlacement.Hidden)]
        public string Comment4 { get => this.comment4; private set { this.comment4 = value; this.RaisePropertyChanged(nameof(Comment4)); } }

        [Comment]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public string Comment5 { get => this.comment5; private set { this.comment5 = value; this.RaisePropertyChanged(nameof(Comment5)); } }

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