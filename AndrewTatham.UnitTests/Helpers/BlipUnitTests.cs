using AndrewTatham.Helpers;
using AndrewTatham.Logic.Enemies;
using NUnit.Framework;
using Robocode;

namespace AndrewTatham.UnitTests.Helpers
{
    [TestFixture]
    public class BlipUnitTests
    {
        public void ConstructorTest()
        {
            var b = new Blip(
                new Vector(0, 0),
                45,
                new ScannedRobotEvent(
                    "",
                    100,
                    0, 0, 0, 0));
        }
    }
}