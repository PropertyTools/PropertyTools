// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyGridControlFactoryTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests.PropertyGridNamespace
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;

    using NUnit.Framework;

    using PropertyTools.Wpf;

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class PropertyGridControlFactoryTests
    {
        private class TestNotifyDataErrorInfo : INotifyDataErrorInfo
        {
            private readonly Dictionary<string, string> errors = new Dictionary<string, string>();

            public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

            public bool HasErrors => this.errors.Count > 0;

            public string Name { get; set; }

            public string LastName { get; set; }

            public IEnumerable GetErrors(string propertyName)
            {
                if (this.errors.TryGetValue(propertyName, out var message))
                {
                    yield return message;
                }
            }

            public void SetError(string propertyName, string message)
            {
                if (message == null)
                {
                    this.errors.Remove(propertyName);
                }
                else
                {
                    this.errors[propertyName] = message;
                }

                this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        [Test]
        public void CreateErrorControl_NotifyDataErrorInfo_MultiplePropertiesUpdateIndependently()
        {
            // Arrange
            var factory = new PropertyGridControlFactory();
            var instance = new TestNotifyDataErrorInfo();
            var tab = new Tab();
            var group = new Group();
            tab.Groups.Add(group);

            var nameDescriptor = TypeDescriptor.GetProperties(instance)[nameof(TestNotifyDataErrorInfo.Name)];
            var lastNameDescriptor = TypeDescriptor.GetProperties(instance)[nameof(TestNotifyDataErrorInfo.LastName)];
            var namePropertyItem = new PropertyItem(nameDescriptor, TypeDescriptor.GetProperties(instance));
            var lastNamePropertyItem = new PropertyItem(lastNameDescriptor, TypeDescriptor.GetProperties(instance));
            group.Properties.Add(namePropertyItem);
            group.Properties.Add(lastNamePropertyItem);

            var options = new PropertyControlFactoryOptions
            {
                ValidationErrorTemplate = new DataTemplate()
            };

            var nameErrorControl = factory.CreateErrorControl(namePropertyItem, instance, tab, options);
            var lastNameErrorControl = factory.CreateErrorControl(lastNamePropertyItem, instance, tab, options);

            // Force both controls to be attached so bindings are able to refresh.
            var container = new StackPanel();
            container.Children.Add(nameErrorControl);
            container.Children.Add(lastNameErrorControl);

            // Act: set an error on "Name" first, then also set an error on "LastName".
            // The tab's aggregate HasErrors flag will already be true after the first call,
            // so the second call must not rely on that flag changing to refresh the controls.
            instance.SetError("Name", "Name should be specified");
            instance.SetError("LastName", "Last name should be specified");

            // Assert
            Assert.That(nameErrorControl.Visibility, Is.EqualTo(Visibility.Visible));
            Assert.That(nameErrorControl.Content, Is.EqualTo("Name should be specified"));
            Assert.That(lastNameErrorControl.Visibility, Is.EqualTo(Visibility.Visible));
            Assert.That(lastNameErrorControl.Content, Is.EqualTo("Last name should be specified"));
        }
    }
}
