// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObservableTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using NUnit.Framework;
    using PropertyTools;

    [TestFixture]
    public class ObservableTests
    {
        /// <summary>
        /// Test class that inherits from Observable to test inherited property verification.
        /// </summary>
        private class BaseClass : Observable
        {
            private string baseProperty;

            public string BaseProperty
            {
                get => this.baseProperty;
                set => this.SetValue(ref this.baseProperty, value);
            }
        }

        /// <summary>
        /// Derived class to test that inherited properties can be verified.
        /// </summary>
        private class DerivedClass : BaseClass
        {
            private string derivedProperty;

            public string DerivedProperty
            {
                get => this.derivedProperty;
                set => this.SetValue(ref this.derivedProperty, value);
            }
        }

        [Test]
        public void SetValue_DeclaredProperty_SetsValueSuccessfully()
        {
            // Arrange
            var obj = new DerivedClass();
            var newValue = "Test";

            // Act
            obj.DerivedProperty = newValue;

            // Assert
            Assert.That(obj.DerivedProperty, Is.EqualTo(newValue));
        }

        [Test]
        public void SetValue_InheritedProperty_SetsValueSuccessfully()
        {
            // Arrange
            var obj = new DerivedClass();
            var newValue = "Inherited Test";

            // Act
            obj.BaseProperty = newValue;

            // Assert
            Assert.That(obj.BaseProperty, Is.EqualTo(newValue));
        }

        [Test]
        public void SetValue_DeclaredProperty_RaisesPropertyChanged()
        {
            // Arrange
            var obj = new DerivedClass();
            var propertyChangedRaised = false;
            string changedPropertyName = null;

            obj.PropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
                changedPropertyName = e.PropertyName;
            };

            // Act
            obj.DerivedProperty = "Test";

            // Assert
            Assert.That(propertyChangedRaised, Is.True);
            Assert.That(changedPropertyName, Is.EqualTo("DerivedProperty"));
        }

        [Test]
        public void SetValue_InheritedProperty_RaisesPropertyChanged()
        {
            // Arrange
            var obj = new DerivedClass();
            var propertyChangedRaised = false;
            string changedPropertyName = null;

            obj.PropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
                changedPropertyName = e.PropertyName;
            };

            // Act
            obj.BaseProperty = "Inherited Test";

            // Assert
            Assert.That(propertyChangedRaised, Is.True);
            Assert.That(changedPropertyName, Is.EqualTo("BaseProperty"));
        }

        [Test]
        public void SetValue_SameValue_DoesNotRaisePropertyChanged()
        {
            // Arrange
            var obj = new DerivedClass();
            obj.DerivedProperty = "Initial";
            var propertyChangedRaised = false;

            obj.PropertyChanged += (sender, e) =>
            {
                propertyChangedRaised = true;
            };

            // Act
            obj.DerivedProperty = "Initial";

            // Assert
            Assert.That(propertyChangedRaised, Is.False);
        }

        [Test]
        public void SetValue_DifferentValues_ReturnsTrue()
        {
            // Arrange
            var obj = new TestObservable();

            // Act
            var result = obj.TestSetValue("Old", "New");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void SetValue_SameValues_ReturnsFalse()
        {
            // Arrange
            var obj = new TestObservable();

            // Act
            var result = obj.TestSetValue("Same", "Same");

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Helper class to expose SetValue for testing.
        /// </summary>
        private class TestObservable : Observable
        {
            public bool TestSetValue(string oldValue, string newValue)
            {
                string field = oldValue;
                return this.SetValue(ref field, newValue, "TestProperty");
            }

            public string TestProperty { get; set; }
        }

        /// <summary>
        /// Base class with a Gender property.
        /// </summary>
        private class AnimalBase : Observable
        {
            private string gender;

            public string Gender
            {
                get => this.gender;
                set => this.SetValue(ref this.gender, value);
            }
        }

        /// <summary>
        /// Derived class that shadows Gender property with "new" keyword.
        /// </summary>
        private class PersonDerived : AnimalBase
        {
            private GenderEnum gender;

            public new GenderEnum Gender
            {
                get => this.gender;
                set => this.SetValue(ref this.gender, value);
            }
        }

        /// <summary>
        /// Gender enumeration for testing property shadowing.
        /// </summary>
        private enum GenderEnum
        {
            Male,
            Female
        }

        [Test]
        public void SetValue_ShadowedProperty_DoesNotThrowAmbiguousMatchException()
        {
            // Arrange
            var obj = new PersonDerived();
            
            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                obj.Gender = GenderEnum.Female;
            }, "SetValue should handle shadowed properties without throwing AmbiguousMatchException");
        }

        [Test]
        public void SetValue_ShadowedProperty_SetsValueSuccessfully()
        {
            // Arrange
            var obj = new PersonDerived();
            var newValue = GenderEnum.Male;

            // Act
            obj.Gender = newValue;

            // Assert
            Assert.That(obj.Gender, Is.EqualTo(newValue));
        }
    }
}
