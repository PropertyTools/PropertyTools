// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeListBoxTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class TreeListBoxTests
    {
        private class Node
        {
            public ObservableCollection<Node> Children { get; } = new ObservableCollection<Node>();

            public ObservableCollection<Node> Nodes { get; } = new ObservableCollection<Node>();
        }

        [Test]
        public void GetChildrenCollectionByReflection_MissingProperty_ThrowsInvalidOperationException()
        {
            var treeListBox = new TreeListBox { ChildrenPath = "MissingChildren" };
            var method = typeof(TreeListBox).GetMethod("GetChildrenCollectionByReflection", BindingFlags.Instance | BindingFlags.NonPublic);

            var exception = Assert.Throws<TargetInvocationException>(() => method.Invoke(treeListBox, new object[] { new Node() }));

            Assert.That(exception.InnerException, Is.TypeOf<InvalidOperationException>());
            Assert.That(exception.InnerException.Message, Does.Contain("MissingChildren"));
        }

        [Test]
        public void GetChildrenCollectionByReflection_ChildrenPathChanged_UsesUpdatedProperty()
        {
            var treeListBox = new TreeListBox { ChildrenPath = nameof(Node.Children) };
            var method = typeof(TreeListBox).GetMethod("GetChildrenCollectionByReflection", BindingFlags.Instance | BindingFlags.NonPublic);
            var node = new Node();
            node.Children.Add(new Node());
            node.Nodes.Add(new Node());

            var initial = (object)method.Invoke(treeListBox, new object[] { node });

            treeListBox.ChildrenPath = nameof(Node.Nodes);
            var updated = method.Invoke(treeListBox, new object[] { node });

            Assert.That(initial, Is.SameAs(node.Children));
            Assert.That(updated, Is.SameAs(node.Nodes));
        }
    }
}
