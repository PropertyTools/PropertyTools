// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionMathTests.cs" company="PropertyTools">
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
// --------------------------------------------------------------------------------------------------------------------
namespace SpreadsheetDemo.Tests
{
    using System;
    using System.Windows;
    using NUnit.Framework;
    using PropertyTools.Wpf;

    [TestFixture]
    public class ReflectionMathTests
    {
        [Test]
        public void TryMultiply_Vector_ReturnsVector()
        {
            var v1 = new Vector(10, 10);
            var v2 = v1 * 4;
            object v3;
            Assert.IsTrue(ReflectionMath.TryMultiply(v1, 4.0, out v3));
            Assert.AreEqual(v2, v3);
        }

        [Test]
        public void TrySubtract_DateTime_ReturnsTimeSpan()
        {
            var t1 = DateTime.Now;
            var t2 = t1.AddDays(2);
            object d;
            Assert.IsTrue(ReflectionMath.TrySubtract(t2, t1, out d));
            Assert.IsTrue(d is TimeSpan);
            Assert.AreEqual(2, ((TimeSpan)d).TotalDays);
        }

        [Test]
        public void TrySubtract_Doubles_ReturnsDouble()
        {
            double n1 = 10;
            double n2 = 12;
            object d;
            Assert.IsTrue(ReflectionMath.TrySubtract(n2, n1, out d));
            Assert.IsTrue(d is double);
            Assert.AreEqual(2, (double)d);
        }

        [Test]
        public void TrySubtract_Ints_ReturnsDouble()
        {
            int n1 = 10;
            int n2 = 12;
            object d;
            Assert.IsTrue(ReflectionMath.TrySubtract(n2, n1, out d));
            Assert.IsTrue(d is int);
            Assert.AreEqual(2, (int)d);
        }

        [Test]
        public void TrySubtract_PointFromVector_Fails()
        {
            object r;
            Assert.IsFalse(ReflectionMath.TrySubtract(new Vector(0, 0), new Point(1, 1), out r));
        }

        [Test]
        public void TrySubtract_Points_ReturnsVector()
        {
            var p1 = new Point(10, 10);
            var p2 = new Point(8, 3);
            var d = p2 - p1;

            object o1 = p1;
            object o2 = p2;
            object d2;
            Assert.IsTrue(ReflectionMath.TrySubtract(o2, o1, out d2));
            Assert.AreEqual(d, d2);
        }
    }
}