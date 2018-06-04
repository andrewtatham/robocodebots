using AndrewTatham.Helpers;
using NUnit.Framework;

namespace AndrewTatham.UnitTests.Helpers
{
    [TestFixture]
    public class VectorUnitTests
    {
        [Test]
        [TestCase(0, 0, 0, 0, 0, 0, 0, 0)]
        [TestCase(0, 1, 0, 1, 0, 2, 2, 0)]
        [TestCase(1, 0, 1, 0, 2, 0, 2, 90)]
        public void Add(
            double ax, double ay,
            double bx, double by,
            double expectedx,
            double expectedy,
            double expectedMagnitude,
            double expectedHeading)
        {
            var vectorA = new Vector(ax, ay);
            var vectorB = new Vector(bx, by);
            Vector vector = vectorA + vectorB;
            Assert.AreEqual(expectedx, vector.X);
            Assert.AreEqual(expectedy, vector.Y);
            Assert.AreEqual(expectedMagnitude, vector.Magnitude);
            Assert.AreEqual(expectedHeading, vector.Heading.Degrees);
        }

        [Test]
        [TestCase(0, 0, 0, 0)]
        [TestCase(0, 10, 10, 0)]
        [TestCase(10, 0, 10, 90)]
        [TestCase(0, -10, 10, 180)]
        [TestCase(-10, 0, 10, 270)]
        public void Constructor(double x, double y, double magnitude, double heading)
        {
            var vector = new Vector(x, y);
            Assert.AreEqual(x, vector.X);
            Assert.AreEqual(y, vector.Y);
            Assert.AreEqual(magnitude, vector.Magnitude);
            Assert.AreEqual(heading, vector.Heading.Degrees);
        }
    }
}