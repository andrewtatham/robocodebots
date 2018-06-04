using System;
using AndrewTatham.Helpers;
using NUnit.Framework;

namespace AndrewTatham.UnitTests.Helpers
{
    [TestFixture]
    public class AngleUnitTests
    {
        [Test]
        [TestCase(0, 0, 0)]
        [TestCase(90, 90, 180)]
        [TestCase(180, 180, 0)]
        [TestCase(360, 360, 0)]
        public void Add(double left, double right, double expected)
        {
            Angle result = new Angle(left) + new Angle(right);
            Assert.AreEqual(expected, result.Degrees);
        }

        //static double? nullDouble = (double?)null;
        //[Test]
        ////[TestCase(nullDouble, nullDouble, false)]
        ////[TestCase(nullDouble, 0d, false)]
        ////[TestCase(0d, nullDouble, false)]

        //[TestCase(90d, 90d, true)]
        //[TestCase(-90d, 270d, true)]
        //[TestCase(360d, 0d, true)]

        //[TestCase(0d, 90d, false)]
        //[TestCase(180d, 90d, false)]
        //[TestCase(360d, 180d, false)]
        //public void Equals(double? left, double? right, bool expected)
        //{
        //    Angle Angle1 = left.HasValue ? new Angle(left.Value) : null ;
        //    Angle Angle2 = right.HasValue ? new Angle(right.Value) : null ;

        //    Assert.AreEqual(left.HasValue && right.HasValue && expected, Angle1 == Angle2);
        //    Assert.AreEqual(left.HasValue && right.HasValue && expected, Angle1.Equals(Angle2));
        //    Assert.AreEqual(left.HasValue && right.HasValue && expected, Angle1.Equals((object)Angle2));
        //    Assert.AreEqual(left.HasValue && right.HasValue && !expected, Angle1 != Angle2);

        //    Assert.AreEqual(expected, Angle1.GetHashCode() == Angle2.GetHashCode());
        //}

        [Test]
        [TestCase(-90, 270)]
        [TestCase(-45, 315)]
        [TestCase(0, 0)]
        [TestCase(45, 45)]
        [TestCase(90, 90)]
        [TestCase(135, 135)]
        [TestCase(180, 180)]
        [TestCase(225, 225)]
        [TestCase(270, 270)]
        [TestCase(315, 315)]
        [TestCase(360, 0)]
        [TestCase(405, 45)]
        [TestCase(450, 90)]
        public void Degrees(double value, double expected)
        {
            Assert.AreEqual(expected, new Angle(value).Degrees);
        }

        [Test]
        [TestCase(-180, 180)]
        [TestCase(-135, -135)]
        [TestCase(-90, -90)]
        [TestCase(-45, -45)]
        [TestCase(0, 0)]
        [TestCase(45, 45)]
        [TestCase(90, 90)]
        [TestCase(135, 135)]
        [TestCase(180, 180)]
        [TestCase(225, -135)]
        [TestCase(270, -90)]
        [TestCase(315, -45)]
        [TestCase(360, 0)]
        [TestCase(405, 45)]
        [TestCase(450, 90)]
        public void Degrees180(double value, double expected)
        {
            Assert.AreEqual(expected, new Angle(value).Degrees180);
        }

        [Test]

        //[TestCase(null, null, false)]
        [TestCase(0d, null, false)]

        //[TestCase(null, 0d, false)]
        [TestCase(0d, 90d, false)]
        [TestCase(0d, 180d, false)]
        [TestCase(0d, 360d, true)]
        [TestCase(90d, 90d, true)]
        [TestCase(180d, -180d, true)]
        [TestCase(270d, -90d, true)]
        public void EqualsAngle(double? a, double? b, bool expected)
        {
            if (expected)
            {
                Assert.IsTrue(
                    (a.HasValue ? new Angle(a.Value) : null)
                        .Equals(
                            b.HasValue ? new Angle(b.Value) : null
                        )
                    );
            }
            else
            {
                Assert.IsFalse(
                    (a.HasValue ? new Angle(a.Value) : null).Equals(b.HasValue ? new Angle(b.Value) : null));
            }
        }

        [Test]
        public void EqualsObject1()
        {
            Assert.IsFalse(new Angle(0d).Equals(null));
        }

        [Test]
        public void EqualsObject2()
        {
            Assert.IsFalse(new Angle(0d).Equals(new object()));
        }

        [Test]
        public void EqualsObject3()
        {
            Assert.IsTrue(new Angle(0d).Equals(new Angle(0d)));
        }

        [Test]
        public void EqualsObject4()
        {
            Assert.IsFalse(new Angle(0d).Equals(0d));
        }

        [Test]
        [TestCase(null, null, false)]
        [TestCase(0d, null, false)]
        [TestCase(null, 0d, false)]
        [TestCase(0d, 0d, true)]
        [TestCase(0d, 90d, false)]
        [TestCase(0d, 180d, false)]
        [TestCase(0d, 360d, true)]
        [TestCase(90d, 90d, true)]
        [TestCase(180d, -180d, true)]
        [TestCase(270d, -90d, true)]
        public void EqualsOp(double? a, double? b, bool expected)
        {
            Angle aa = a.HasValue ? new Angle(a.Value) : null;
            Angle bb = b.HasValue ? new Angle(b.Value) : null;

            if (expected)
            {
                Assert.IsTrue(aa == bb);
            }
            else
            {
                Assert.IsFalse(aa == bb);
            }
        }

        [Test]
        [TestCase(0d, 360d)]
        [TestCase(90d, 90d)]
        [TestCase(180d, -180d)]
        [TestCase(270d, -90d)]
        public void GetHashCode(double a, double b)
        {
            Assert.AreEqual(new Angle(a).GetHashCode(), new Angle(b).GetHashCode());
        }

        [Test]
        [TestCase(null, null, false)]
        [TestCase(0d, null, false)]
        [TestCase(null, 0d, false)]
        [TestCase(0d, 90d, true)]
        [TestCase(0d, 180d, true)]
        [TestCase(0d, 360d, false)]
        [TestCase(90d, 90d, false)]
        [TestCase(180d, -180d, false)]
        [TestCase(270d, -90d, false)]
        public void NotEqualsOp(double? a, double? b, bool expected)
        {
            Angle aa = a.HasValue ? new Angle(a.Value) : null;
            Angle bb = b.HasValue ? new Angle(b.Value) : null;

            if (expected)
            {
                Assert.IsTrue(aa != bb);
            }
            else
            {
                Assert.IsFalse(aa != bb);
            }
        }

        [Test]
        [TestCase(0d, 180d)]
        [TestCase(90d, 270d)]
        [TestCase(180d, 0d)]
        [TestCase(270d, 90d)]
        public void Opposite(double value, double expected)
        {
            Assert.AreEqual(expected, new Angle(value).Opposite.Degrees);
        }

        [Test]
        [TestCase(0d, 90d)]
        [TestCase(90d, 180d)]
        [TestCase(180d, 270d)]
        [TestCase(270d, 0d)]
        public void Perpendicular(double value, double expected)
        {
            Assert.AreEqual(expected, new Angle(value).Perpendicular.Degrees);
        }

        [Test]
        [TestCase(-90, 1.5d * Math.PI)]
        [TestCase(-45, 1.75d * Math.PI)]
        [TestCase(0, 0d * Math.PI)]
        [TestCase(45, 0.25d * Math.PI)]
        [TestCase(90, 0.5d * Math.PI)]
        [TestCase(135, 0.75d * Math.PI)]
        [TestCase(180, 1d * Math.PI)]
        [TestCase(225, 1.25d * Math.PI)]
        [TestCase(270, 1.5d * Math.PI)]
        [TestCase(315, 1.75d * Math.PI)]
        [TestCase(360, 0d * Math.PI)]
        [TestCase(405, 0.25d * Math.PI)]
        [TestCase(450, 0.5d * Math.PI)]
        public void Radians(double value, double expected)
        {
            Assert.AreEqual(expected, new Angle(value).Radians);
        }

        [Test]
        [TestCase(0, 90, 270)]
        [TestCase(90, 90, 0)]
        [TestCase(180, 90, 90)]
        [TestCase(360, 180, 180)]
        public void Subtract(double left, double right, double expected)
        {
            Angle result = new Angle(left) - new Angle(right);
            Assert.AreEqual(expected, result.Degrees);
        }
    }
}