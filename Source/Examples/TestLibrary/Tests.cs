// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tests.cs" company="PropertyTools">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Provides a collection of all test objects in the assembly.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
                    new TestSpinnableAttribute(),
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