// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataAnnotationsTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Tests
{
    using NUnit.Framework;
    using PropertyTools.DataAnnotations;

    [TestFixture]
    [NUnit.Framework.Category("CrossPlatform")]
    public class DataAnnotationsTests
    {
        [Test]
        public void SlidableAttribute_MinimumMaximum_SetsDefaultChanges()
        {
            // Arrange / Act
            var attribute = new SlidableAttribute(0, 100);

            // Assert
            Assert.That(attribute.Minimum, Is.EqualTo(0));
            Assert.That(attribute.Maximum, Is.EqualTo(100));
            Assert.That(attribute.SmallChange, Is.EqualTo(1));
            Assert.That(attribute.LargeChange, Is.EqualTo(10));
        }

        [Test]
        public void SlidableAttribute_AllArguments_SetsAllProperties()
        {
            // Arrange / Act
            var attribute = new SlidableAttribute(-1, 1, 0.1, 0.5);

            // Assert
            Assert.That(attribute.Minimum, Is.EqualTo(-1));
            Assert.That(attribute.Maximum, Is.EqualTo(1));
            Assert.That(attribute.SmallChange, Is.EqualTo(0.1));
            Assert.That(attribute.LargeChange, Is.EqualTo(0.5));
        }

        [Test]
        public void CategoryAttribute_CategoryOnly_SetsCategory()
        {
            // Arrange / Act
            var attribute = new DataAnnotations.CategoryAttribute("General");

            // Assert
            Assert.That(attribute.Category, Is.EqualTo("General"));
        }

        [Test]
        public void CategoryAttribute_SortIndexes_SetsSortIndexes()
        {
            // Arrange / Act
            var attribute = new DataAnnotations.CategoryAttribute("General", 2, 3);

            // Assert
            Assert.That(attribute.Category, Is.EqualTo("General"));
            Assert.That(attribute.TabSortIndex, Is.EqualTo(2));
            Assert.That(attribute.GroupSortIndex, Is.EqualTo(3));
        }

        [Test]
        public void EnableByAttribute_PropertyNameOnly_SetsNullPropertyValue()
        {
            // Arrange / Act
            var attribute = new EnableByAttribute("IsEnabled");

            // Assert
            Assert.That(attribute.PropertyName, Is.EqualTo("IsEnabled"));
            Assert.That(attribute.PropertyValue, Is.Null);
        }

        [Test]
        public void EnableByAttribute_PropertyNameAndValue_SetsBothProperties()
        {
            // Arrange / Act
            var attribute = new EnableByAttribute("Mode", 42);

            // Assert
            Assert.That(attribute.PropertyName, Is.EqualTo("Mode"));
            Assert.That(attribute.PropertyValue, Is.EqualTo(42));
        }

        [Test]
        public void Column_PropertyNameOnly_SetsDefaults()
        {
            // Arrange / Act
            var column = new Column("Name");

            // Assert
            Assert.That(column.PropertyName, Is.EqualTo("Name"));
            Assert.That(column.Width, Is.EqualTo("Auto"));
            Assert.That(column.Alignment, Is.EqualTo('C'));
            Assert.That(column.IsReadOnly, Is.False);
        }

        [Test]
        public void Column_PropertyNameAndHeader_SetsHeader()
        {
            // Arrange / Act
            var column = new Column("Name", "Full name");

            // Assert
            Assert.That(column.PropertyName, Is.EqualTo("Name"));
            Assert.That(column.Header, Is.EqualTo("Full name"));
        }
    }
}
