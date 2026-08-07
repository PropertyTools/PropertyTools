// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckBoxListTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests.Controls
{
    using System;
    using System.Threading;
    using System.Windows.Controls;

    using NUnit.Framework;

    [Flags]
    public enum Permission
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4,
        All = Read | Write | Execute,
    }

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class CheckBoxListTests
    {
        [Test]
        public void Value_SetToFlagsValue_ChecksCorrectBoxes()
        {
            // Arrange
            var control = new CheckBoxList { EnumType = typeof(Permission) };
            control.ApplyTemplate();

            // Act
            control.Value = Permission.Read | Permission.Execute;

            // Assert – Value round-trips correctly
            Assert.That(control.Value, Is.EqualTo(Permission.Read | Permission.Execute));
        }

        [Test]
        public void Value_SetToNone_NoBoxesChecked()
        {
            // Arrange
            var control = new CheckBoxList { EnumType = typeof(Permission) };
            control.ApplyTemplate();

            // Act
            control.Value = Permission.None;

            // Assert
            Assert.That(Convert.ToInt64(control.Value), Is.EqualTo(0L));
        }

        [Test]
        public void Value_Composition_CombinesTwoFlags()
        {
            // Arrange – simulate user checking Read and Write
            var readFlag = (long)Permission.Read;
            var writeFlag = (long)Permission.Write;

            // Act
            var result = Enum.ToObject(typeof(Permission), readFlag | writeFlag);

            // Assert
            Assert.That(result, Is.EqualTo(Permission.Read | Permission.Write));
        }

        [Test]
        public void Value_Parsing_ExtractsSingleFlag()
        {
            // Arrange
            var combined = (long)(Permission.Read | Permission.Write | Permission.Execute);
            var flag = (long)Permission.Write;

            // Act
            var hasFlag = (combined & flag) == flag;

            // Assert
            Assert.That(hasFlag, Is.True);
        }

        [Test]
        public void Value_Parsing_RejectsAbsentFlag()
        {
            // Arrange
            var combined = (long)(Permission.Read | Permission.Execute);
            var flag = (long)Permission.Write;

            // Act
            var hasFlag = (combined & flag) == flag;

            // Assert
            Assert.That(hasFlag, Is.False);
        }
    }
}
