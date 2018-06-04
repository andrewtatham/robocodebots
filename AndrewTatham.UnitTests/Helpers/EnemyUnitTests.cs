using System;
using AndrewTatham.Helpers;
using AndrewTatham.Logic;
using AndrewTatham.Logic.Enemies;
using Moq;
using NUnit.Framework;
using Robocode;

namespace AndrewTatham.UnitTests.Helpers
{
    [TestFixture]
    [Ignore]
    public class EnemyUnitTests
    {
        [Test]
        [Ignore]
        public void Update()
        {
            var myLocation = new Vector(0d, 0d);
            const double myHeading = 0;

            const string name = "";
            const double energy = 100d;
            const double bearing = 0d;
            const double distance = 100d;
            const double heading = 90d;

            var mockEvent = new ScannedRobotEvent(name, energy, bearing, distance, heading * Math.PI / 180d, Rules.MAX_VELOCITY);

            var mockContext = new Mock<IContext>();
            mockContext.Setup(mock => mock.MyLocation).Returns(myLocation);
            mockContext.Setup(mock => mock.MyHeading).Returns(myHeading);

            var enemy = new Enemy(mockContext.Object, mockEvent);

            var expectedLocation = myLocation + new Vector(distance, new Angle(myHeading + bearing));
            AssertEqualVectors(expectedLocation, enemy.Location);

            Assert.AreEqual(energy, enemy.Energy);
            AssertEqualVectors(expectedLocation - myLocation, enemy.Direct);
            AssertEqualVectors(myLocation - expectedLocation, enemy.EnemyDirect);
            Assert.AreEqual(heading, enemy.Heading.Degrees);
        }

        public static void AssertEqualVectors(Vector expected, Vector actual)
        {
            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
        }
    }
}