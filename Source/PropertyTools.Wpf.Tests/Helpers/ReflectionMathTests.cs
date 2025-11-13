// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionMathTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.Globalization;
    using System.Windows;
    using NUnit.Framework;

    [TestFixture]
    public class ReflectionMathTests
    {
        [Test]
        public void TryAdd_Numbers_ReturnsVector()
        {
            object r;
            Assert.That(ReflectionMath.TryAdd(1, 1, out r), Is.True);
            Assert.That(r, Is.EqualTo(2));
            Assert.That(ReflectionMath.TryAdd(1d, 1d, out r), Is.True);
            Assert.That(r, Is.EqualTo(2));
            Assert.That(ReflectionMath.TryAdd(1, 1d, out r), Is.True);
            Assert.That(r, Is.EqualTo(2));
            Assert.That(ReflectionMath.TryAdd(1d, 1, out r), Is.True);
            Assert.That(r, Is.EqualTo(2));
        }

        [Test]
        public void TryMultiply_Numbers_ReturnsVector()
        {
            object r;
            Assert.That(ReflectionMath.TryMultiply(2, 1, out r), Is.True);
            Assert.That(r, Is.EqualTo(2));
            Assert.That(ReflectionMath.TryMultiply(2d, 1d, out r), Is.True);
            Assert.That(r, Is.EqualTo(2));
            Assert.That(ReflectionMath.TryMultiply(2, 1d, out r), Is.True);
            Assert.That(r, Is.EqualTo(2));
            Assert.That(ReflectionMath.TryMultiply(2d, 1, out r), Is.True);
            Assert.That(r, Is.EqualTo(2));
        }
        [Test]
        public void TryMultiply_Vector_ReturnsVector()
        {
            var v1 = new Vector(10, 10);
            var v2 = v1 * 4;
            object v3;
            Assert.That(ReflectionMath.TryMultiply(v1, 4.0, out v3), Is.True);
            Assert.That(v3, Is.EqualTo(v2));
        }

        [Test]
        public void TrySubtract_DateTime_ReturnsTimeSpan()
        {
            var t1 = DateTime.Now;
            var t2 = t1.AddDays(2);
            object d;
            Assert.That(ReflectionMath.TrySubtract(t2, t1, out d), Is.True);
            Assert.That(d, Is.InstanceOf<TimeSpan>());
            Assert.That(((TimeSpan)d).TotalDays, Is.EqualTo(2));
        }

        [Test]
        public void TrySubtract_Doubles_ReturnsDouble()
        {
            double n1 = 10;
            double n2 = 12;
            object d;
            Assert.That(ReflectionMath.TrySubtract(n2, n1, out d), Is.True);
            Assert.That(d, Is.InstanceOf<double>());
            Assert.That((double)d, Is.EqualTo(2));
        }

        [Test]
        public void TrySubtract_Ints_ReturnsDouble()
        {
            int n1 = 10;
            int n2 = 12;
            object d;
            Assert.That(ReflectionMath.TrySubtract(n2, n1, out d), Is.True);
            Assert.That(d, Is.InstanceOf<int>());
            Assert.That((int)d, Is.EqualTo(2));
        }

        [Test]
        public void TrySubtract_PointFromVector_Fails()
        {
            object r;
            Assert.That(ReflectionMath.TrySubtract(new Vector(0, 0), new Point(1, 1), out r), Is.False);
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
            Assert.That(ReflectionMath.TrySubtract(o2, o1, out d2), Is.True);
            Assert.That(d2, Is.EqualTo(d));
        }

        [Test]
        public void TryParse_Double_ReturnsCorrectValue()
        {
            object pi;
            Assert.That(ReflectionMath.TryParse(typeof(double), "3.14", CultureInfo.InvariantCulture, out pi), Is.True);
            Assert.That(pi, Is.EqualTo(3.14));
        }
    }
}