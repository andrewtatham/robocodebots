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
    public class OneVsOneBehaviourUnitTests
    {
        private readonly BaseBehavior _behaviour = new OneVsOneBehavior();

        [Test]
        [TestCase(3, 3, false)]
        [TestCase(2, 2, false)]
        [TestCase(1, 1, true)]
        [TestCase(0, 0, false)]
        public void Condition(
            int othersCount,
            int scannedCount,
            bool expected)
        {
            var mockContext = new Mock<IContext>();
            mockContext.Setup(x => x.OthersCount).Returns(othersCount);
            mockContext.Setup(x => x.ScannedCount).Returns(scannedCount);
            BaseBehavior.Context = mockContext.Object;
            Assert.AreEqual(expected, _behaviour.Condition());
        }

        [Test]
        public void Strategies()
        {
            Assert.IsInstanceOf(typeof(TargetOnly), _behaviour.Strategies[0]);
            Assert.IsInstanceOf(typeof(ScanTargetRadar), _behaviour.Strategies[1]);
            Assert.IsInstanceOf(typeof(ForceMovement), _behaviour.Strategies[2]);
            Assert.IsInstanceOf(typeof(OneVsOneFiringStrategy), _behaviour.Strategies[3]);
            Assert.IsInstanceOf(typeof(PredictionAlgorithms), _behaviour.Strategies[4]);
        }
    }
}