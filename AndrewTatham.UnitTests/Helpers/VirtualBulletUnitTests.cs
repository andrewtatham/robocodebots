using System;
using System.IO;
using AndrewTatham.Helpers;
using AndrewTatham.Logic;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction.GF;
using AndrewTatham.Logic.Enemies;
using Moq;
using NUnit.Framework;
using Robocode;

namespace AndrewTatham.UnitTests.Helpers
{
    [TestFixture]
    [Ignore]
    public class VirtualBulletUnitTests
    {
        [SetUp]
        public void Setup()
        {
            var mockOut = new Mock<TextWriter>();

            var mockAimResult = new Mock<AimResult>();
            double power = Rules.MIN_BULLET_POWER + 0.5d * (Rules.MAX_BULLET_POWER - Rules.MIN_BULLET_POWER);
            double speed = Rules.GetBulletSpeed(power);
            mockAimResult.Setup(mock => mock.Speed).Returns(speed);

            var mockEnemy = new Mock<IEnemy>();
            mockEnemy.Setup(mock => mock.Location).Returns(new Vector(100, 100));
            mockEnemy.Setup(mock => mock.Direct).Returns(new Vector(50, 50));

            var mockInitialContext = new Mock<IContext>();
            mockInitialContext.Setup(mock => mock.Target).Returns(mockEnemy.Object);
            mockInitialContext.Setup(mock => mock.MyLocation).Returns(new Vector(50, 50));
            mockInitialContext.Setup(mock => mock.Time).Returns(50);
            mockInitialContext.Setup(mock => mock.AimResult).Returns(mockAimResult.Object);

            mockInitialContext.Setup(mock => mock.BattlefieldDiag).Returns(Math.Sqrt(1000d * 1000d));
            mockInitialContext.Setup(mock => mock.Out).Returns(mockOut.Object);

            _vb = new VirtualBullet(mockInitialContext.Object);
        }

        private VirtualBullet _vb;

        [Test]
        public void Constructor()
        {
            Assert.IsNotNull(_vb);
        }

        [Test]
        public void IsFinished()
        {
            var mockOut = new Mock<TextWriter>();

            var mockEnemy = new Mock<IEnemy>();
            mockEnemy.Setup(mock => mock.Location).Returns(new Vector(100, 100));
            mockEnemy.Setup(mock => mock.Direct).Returns(new Vector(50, 50));

            var mockAimResult = new Mock<AimResult>();
            double power = Rules.MIN_BULLET_POWER + 0.5d * (Rules.MAX_BULLET_POWER - Rules.MIN_BULLET_POWER);
            double speed = Rules.GetBulletSpeed(power);
            mockAimResult.Setup(mock => mock.Speed).Returns(speed);

            var mockInitialContext = new Mock<IContext>();
            mockInitialContext.Setup(mock => mock.Target).Returns(mockEnemy.Object);
            mockInitialContext.Setup(mock => mock.MyLocation).Returns(new Vector(50, 50));
            mockInitialContext.Setup(mock => mock.Time).Returns(50);
            mockInitialContext.Setup(mock => mock.AimResult).Returns(mockAimResult.Object);
            mockInitialContext.Setup(mock => mock.BattlefieldDiag).Returns(Math.Sqrt(1000d * 1000d));
            mockInitialContext.Setup(mock => mock.Out).Returns(mockOut.Object);

            // Bullet not hit yet
            double? gf;
            GfStatKey key;
            Assert.IsFalse(_vb.IsFinished(mockInitialContext.Object, out key, out gf));
            Assert.IsFalse(gf.HasValue);
            Assert.IsNull(key);

            // Bullet hit
            mockInitialContext.Setup(mock => mock.Time).Returns(55);
            Assert.IsTrue(_vb.IsFinished(mockInitialContext.Object, out key, out gf));
            Assert.IsTrue(gf.HasValue);
            Assert.GreaterOrEqual(gf.Value, -1d);
            Assert.LessOrEqual(gf.Value, 1d);
            Assert.IsNotNull(key);

            //// Bullet out of bounds
            //mockInitialContext.Setup(mock => mock.Time).Returns(150);
            //Assert.IsTrue(vb.IsFinished(mockContext.Object, out key, out gf));
            //Assert.IsFalse(gf.HasValue);
            //Assert.IsNull(key);
        }

        [Test]
        public void Render()
        {
            var mockGraphics = new Mock<IGraphics>();
            _vb.Render(mockGraphics.Object);
        }
    }
}