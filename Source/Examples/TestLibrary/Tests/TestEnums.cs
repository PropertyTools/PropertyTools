// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestEnums.cs" company="PropertyTools">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TestLibrary
{
    using System;
    using System.ComponentModel;

    public enum Fruits1 { Apple, Pears, Bananas }

    public enum Fruits2 { }

    public enum Fruits3 { [Browsable(false)] Apple, Pears, Bananas }

    public enum Fruits4 { [Browsable(false)] Apple, [Browsable(false)] Pears, [Browsable(false)] Bananas }

    public enum Fruits5 { [Description("Epler")] Apple, [Description("P�rer")] Pears, [Description("Bananer")] Bananas }

    [Flags]
    public enum Fruits6 { Apple=1, Pears=2, Bananas=4 }

    public class TestEnums : TestBase
    {
        [Description("Normal enum.")]
        public Fruits1 Fruits1 { get; set; }

        [Description("Empty enum.")]
        public Fruits2 Fruits2 { get; set; }

        [Description("First item is not browsable.")]
        public Fruits3 Fruits3 { get; set; }

        [Description("No items are browsable.")]
        public Fruits4 Fruits4 { get; set; }

        [Description("With descriptions.")]
        public Fruits5 Fruits5 { get; set; }

        [Description("Flags.")]
        public Fruits6 Fruits6 { get; set; }

        public override string ToString()
        {
            return "Enumerations";
        }
    }
}