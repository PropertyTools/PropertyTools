// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePickerTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests.Controls
{
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class FilePickerTests
    {
        [Test]
        public void BrowseCommand_UseOpenDialogTrue_UsesOpenFileDialogAndUpdatesFilePath()
        {
            // Arrange
            var service = new StubFileDialogService { FileNameToReturn = @"C:\Base\opened.txt" };
            var control = new FilePicker
            {
                BasePath = @"C:\Base",
                FileDialogService = service,
                FilePath = "current.txt",
                UseOpenDialog = true
            };

            // Act
            control.BrowseCommand.Execute(null);

            // Assert
            Assert.That(service.OpenFileDialogCallCount, Is.EqualTo(1));
            Assert.That(service.SaveFileDialogCallCount, Is.EqualTo(0));
            Assert.That(control.FilePath, Is.EqualTo("opened.txt"));
        }

        [Test]
        public void BrowseCommand_UseOpenDialogFalse_UsesSaveFileDialogAndUpdatesFilePath()
        {
            // Arrange
            var service = new StubFileDialogService { FileNameToReturn = @"C:\Base\saved.txt" };
            var control = new FilePicker
            {
                BasePath = @"C:\Base",
                FileDialogService = service,
                FilePath = "current.txt",
                UseOpenDialog = false
            };

            // Act
            control.BrowseCommand.Execute(null);

            // Assert
            Assert.That(service.OpenFileDialogCallCount, Is.EqualTo(0));
            Assert.That(service.SaveFileDialogCallCount, Is.EqualTo(1));
            Assert.That(control.FilePath, Is.EqualTo("saved.txt"));
        }

        [Test]
        public void SaveCommand_UseOpenDialogTrue_UsesSaveFileDialogAndUpdatesFilePath()
        {
            // Arrange
            var service = new StubFileDialogService { FileNameToReturn = @"C:\Base\saved-as.txt" };
            var control = new FilePicker
            {
                BasePath = @"C:\Base",
                FileDialogService = service,
                FilePath = "current.txt",
                UseOpenDialog = true
            };

            // Act
            control.SaveCommand.Execute(null);

            // Assert
            Assert.That(service.OpenFileDialogCallCount, Is.EqualTo(0));
            Assert.That(service.SaveFileDialogCallCount, Is.EqualTo(1));
            Assert.That(service.LastSaveFileName, Is.EqualTo(@"C:\Base\current.txt"));
            Assert.That(control.FilePath, Is.EqualTo("saved-as.txt"));
        }

        private class StubFileDialogService : IFileDialogService
        {
            public string FileNameToReturn { get; set; }

            public string[] FileNamesToReturn { get; set; }

            public string LastSaveFileName { get; private set; }

            public int OpenFileDialogCallCount { get; private set; }

            public int OpenFilesDialogCallCount { get; private set; }

            public int SaveFileDialogCallCount { get; private set; }

            public bool ShowOpenFileDialog(ref string filename, string filter, string defaultExtension)
            {
                this.OpenFileDialogCallCount++;
                filename = this.FileNameToReturn;
                return true;
            }

            public bool ShowOpenFilesDialog(ref string[] filenames, string filter, string defaultExtension)
            {
                this.OpenFilesDialogCallCount++;
                filenames = this.FileNamesToReturn;
                return true;
            }

            public bool ShowSaveFileDialog(ref string filename, string filter, string defaultExtension)
            {
                this.SaveFileDialogCallCount++;
                this.LastSaveFileName = filename;
                filename = this.FileNameToReturn;
                return true;
            }
        }
    }
}
