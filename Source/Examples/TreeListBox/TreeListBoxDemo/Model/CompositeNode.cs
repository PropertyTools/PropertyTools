// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeNode.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TreeListBoxDemo
{
    using System.Collections.Generic;

    public class CompositeNode : Node
    {
        public List<Node> Children { get; private set; }

        public CompositeNode()
        {
            Children = new List<Node>();
        }
    }
}