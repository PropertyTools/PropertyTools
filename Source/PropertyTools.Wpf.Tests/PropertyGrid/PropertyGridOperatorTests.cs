// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyGridOperatorTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests.PropertyGrid
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using PropertyTools.Wpf;

    [TestFixture]
    public class PropertyGridOperatorTests
    {
        /// <summary>
        /// Test base class with Gender property.
        /// </summary>
        public class Animal
        {
            public string Gender { get; set; }
        }

        /// <summary>
        /// Test derived class that shadows Gender property with "new" keyword.
        /// </summary>
        public class Person : Animal
        {
            public string Name { get; set; }
            
            public new Genders Gender { get; set; }
        }

        /// <summary>
        /// Enum for Gender in derived class.
        /// </summary>
        public enum Genders
        {
            Male,
            Female
        }

        [Test]
        public void CreateModel_ClassWithNewProperty_DoesNotThrowAmbiguousMatchException()
        {
            // Arrange
            var propertyGrid = new PropertyGrid();
            var person = new Person
            {
                Name = "John Doe",
                Gender = Genders.Male
            };

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                propertyGrid.SelectedObject = person;
                propertyGrid.UpdateModel();
            }, "PropertyGrid should handle classes with 'new' properties without throwing AmbiguousMatchException");
        }

        [Test]
        public void CreatePropertyItems_ClassWithNewProperty_CreatesCorrectPropertyItems()
        {
            // Arrange
            var propertyGrid = new PropertyGrid();
            var person = new Person
            {
                Name = "Jane Doe",
                Gender = Genders.Female
            };
            propertyGrid.SelectedObject = person;

            // Act
            propertyGrid.UpdateModel();

            // Assert
            var properties = propertyGrid.Properties;
            Assert.That(properties, Is.Not.Null, "Properties collection should not be null");
            Assert.That(properties.Any(p => p.PropertyName == "Gender"), Is.True, "Gender property should be present");
            Assert.That(properties.Any(p => p.PropertyName == "Name"), Is.True, "Name property should be present");
            
            var genderProperty = properties.FirstOrDefault(p => p.PropertyName == "Gender");
            Assert.That(genderProperty, Is.Not.Null, "Gender property should exist");
            Assert.That(genderProperty.ActualPropertyType, Is.EqualTo(typeof(Genders)), 
                "Gender property should have type Genders (not string from base class)");
        }
    }
}
