// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyTabAttributeTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using NUnit.Framework;
    using PropertyTools.DataAnnotations;

    [TestFixture]
    public class PropertyTabAttributeTests
    {
        [Test]
        public void Constructor_WithTabNameOnly_SetsTabNameAndDefaultScope()
        {
            // Arrange & Act
            var attribute = new PropertyTabAttribute("TestTab");

            // Assert
            Assert.That(attribute.TabName, Is.EqualTo("TestTab"));
            Assert.That(attribute.Scope, Is.EqualTo(PropertyTabScope.Component));
        }

        [Test]
        public void Constructor_WithTabNameAndScope_SetsTabNameAndScope()
        {
            // Arrange & Act
            var attribute = new PropertyTabAttribute("TestTab", PropertyTabScope.Document);

            // Assert
            Assert.That(attribute.TabName, Is.EqualTo("TestTab"));
            Assert.That(attribute.Scope, Is.EqualTo(PropertyTabScope.Document));
        }

        [Test]
        public void Constructor_WithGlobalScope_SetsCorrectScope()
        {
            // Arrange & Act
            var attribute = new PropertyTabAttribute("GlobalTab", PropertyTabScope.Global);

            // Assert
            Assert.That(attribute.TabName, Is.EqualTo("GlobalTab"));
            Assert.That(attribute.Scope, Is.EqualTo(PropertyTabScope.Global));
        }

        [Test]
        public void PropertyTabScope_HasCorrectValues()
        {
            // Assert
            Assert.That((int)PropertyTabScope.Document, Is.EqualTo(0));
            Assert.That((int)PropertyTabScope.Component, Is.EqualTo(1));
            Assert.That((int)PropertyTabScope.Global, Is.EqualTo(2));
        }
    }
}
