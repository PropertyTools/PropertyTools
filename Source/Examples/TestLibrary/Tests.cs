// -----------------------------------------------------------------------
// <copyright file="TestCollection.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TestLibrary
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides a collection of all test objects in the assembly.
    /// </summary>
    public static class Tests
    {
        /// <summary>
        /// Gets a collection of all test objects in the assembly.
        /// </summary>
        /// <returns>A list of objects.</returns>
        public static List<object> Get()
        {
            return new List<object>
                {
                    new TestSimpleTypes(),
                    new TestAdvancedTypes(),
                    new TestDisplayName(),
                    new TestCategory(),
                    new TestEnums(),
                    new TestSubClass(),
                    new TestReadOnlyProperties(),
                    new TestDataErrorInfo(),
                    new TestExceptions(),
                    new TestEnabledProperties(),
                    new TestVisibleProperties(),
                    new TestOptionalProperties(),
                    new TestAutomaticDisplayNames(),
                    new TestFormatStringAttribute(),
                    new TestConverterAttribute(),
                    new TestSlidableAttribute(),
                    new TestFilePathAttribute(),
                    new TestDirectoryPathAttribute(),
                    new TestCommentAttribute(),
                    new TestHeaderPlacementAttribute(),
                    new TestPassword(),
                    new TestPerformance(),
                    new TestCollections(),
                    new TestDictionary(),
                    new TestItemsSourcePropertyAttribute(),
                    new TestImageSource(),
                    new TestAutoUpdateTextAttribute(),
                    new TestDataAnnotations(),
                    new TestDataTypes(),
                };
        }
    }
}
