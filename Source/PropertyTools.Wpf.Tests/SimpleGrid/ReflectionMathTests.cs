using System;
using System.Windows;
using NUnit.Framework;
using PropertyTools.Wpf;

namespace SpreadsheetDemo.Tests
{
    [TestFixture]
    public class ReflectionMathTests
    {
        [Test]
        public void TryMultiply_Vector_ReturnsVector()
        {
            var v1 = new Vector(10, 10);
            var v2 = v1*4;
            object v3;
            Assert.IsTrue(ReflectionMath.TryMultiply(v1, 4.0, out v3));
            Assert.AreEqual(v2, v3);
        }

        [Test]
        public void TrySubtract_DateTime_ReturnsTimeSpan()
        {
            DateTime t1 = DateTime.Now;
            DateTime t2 = t1.AddDays(2);
            object d;
            Assert.IsTrue(ReflectionMath.TrySubtract(t2, t1, out d));
            Assert.IsTrue(d is TimeSpan);
            Assert.AreEqual(2, ((TimeSpan) d).TotalDays);
        }

        [Test]
        public void TrySubtract_Doubles_ReturnsDouble()
        {
            double n1 = 10;
            double n2 = 12;
            object d;
            Assert.IsTrue(ReflectionMath.TrySubtract(n2, n1, out d));
            Assert.IsTrue(d is Double);
            Assert.AreEqual(2, (double) d);
        }

        [Test]
        public void TrySubtract_Ints_ReturnsDouble()
        {
            int n1 = 10;
            int n2 = 12;
            object d;
            Assert.IsTrue(ReflectionMath.TrySubtract(n2, n1, out d));
            Assert.IsTrue(d is Int32);
            Assert.AreEqual(2, (int) d);
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