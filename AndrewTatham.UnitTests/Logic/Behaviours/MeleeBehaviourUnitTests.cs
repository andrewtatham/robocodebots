using AndrewTatham.Logic;
using AndrewTatham.Logic.Behaviors;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming.Firing;
using AndrewTatham.Logic.Behaviors.Strategies.Movement;
using AndrewTatham.Logic.Behaviors.Strategies.Radar;
using AndrewTatham.Logic.Behaviors.Strategies.Targeting;
using Moq;
using NUnit.Framework;

namespace AndrewTatham.UnitTests.Logic.Behaviours
{
    [TestFixture]
    public class MeleeBehaviourUnitTests
    {
        private readonly BaseBehavior _behaviour = new MeleeBehavior();

        [Test]
        [TestCase(3, true)]
        [TestCase(2, true)]
        [TestCase(1, false)]
        [TestCase(0, false)]
        public void Condition(
            int scannedCount,
            bool expected)
        {
            var mockContext = new Mock<IContext>();
            mockContext.Setup(x => x.ScannedCount).Returns(scannedCount);
            BaseBehavior.Context = mockContext.Object;
            Assert.AreEqual(expected, _behaviour.Condition());
        }

        [Test]
        public void Strategies()
        {
            Assert.IsInstanceOf(typeof(FullScanRadar), _behaviour.Strategies[0]);
            Assert.IsInstanceOf(typeof(ForceMovement), _behaviour.Strategies[1]);
            Assert.IsInstanceOf(typeof(TargetNearestEnemy), _behaviour.Strategies[2]);
            Assert.IsInstanceOf(typeof(MeleeFiringStrategy), _behaviour.Strategies[3]);
            Assert.IsInstanceOf(typeof(PredictionAlgorithms), _behaviour.Strategies[4]);
        }
    }
}