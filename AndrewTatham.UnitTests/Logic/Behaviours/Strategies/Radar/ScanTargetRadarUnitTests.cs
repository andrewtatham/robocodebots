using System.IO;
using AndrewTatham.Helpers;
using AndrewTatham.Logic;
using AndrewTatham.Logic.Behaviors.Strategies;
using AndrewTatham.Logic.Behaviors.Strategies.Radar;
using AndrewTatham.Logic.Enemies;
using Moq;
using NUnit.Framework;

namespace AndrewTatham.UnitTests.Logic.Behaviours.Strategies.Radar
{
    [TestFixture]
    [Ignore]
    public class ScanTargetRadarUnitTests
    {
        [Test]
        [TestCase(0d, 500d, 270d)]
        [TestCase(500d, 0d, 180d)]
        [TestCase(1000d, 500d, 90d)]
        [TestCase(500d, 1000d, 0d)]
        public void Execute(double x, double y, double expected)
        {
            var mockBot = new Mock<BaseAdvancedRobot>();
            mockBot.Setup(mock => mock.X).Returns(500d);
            mockBot.Setup(mock => mock.Y).Returns(500d);
            mockBot.Setup(mock => mock.RadarTurnRemaining).Returns(0d);

            var mockOut = new Mock<TextWriter>();

            var mockEnemy = new Mock<IEnemy>();
            mockEnemy.Setup(mock => mock.Location).Returns(new Vector(x, y));
            mockEnemy.Setup(mock => mock.Direct).Returns(new Vector(x, y) - new Vector(500d, 500d));
            var str = new ScanTargetRadar();
            BaseStrategy.Out = mockOut.Object;

            double arc = str.Arc;

            IContext context = new Context(mockBot.Object)
                {
                    Target = mockEnemy.Object
                };
            BaseStrategy.Context = context;
            for (int repeat = 0; repeat < 2; repeat++)
            {
                str.Execute();

                Assert.AreEqual(ScanType.TurnTo, context.RadarResult.ScanType);
                Assert.AreEqual(new Angle(expected + arc).Degrees, context.RadarResult.TurnTo.Degrees);

                str.Execute();

                Assert.AreEqual(ScanType.TurnTo, context.RadarResult.ScanType);
                Assert.AreEqual(new Angle(expected - arc).Degrees, context.RadarResult.TurnTo.Degrees);
            }
        }
    }
}