using AndrewTatham.Logic;
using AndrewTatham.Logic.Behaviors;
using AndrewTatham.Logic.Behaviors.Strategies.Movement;
using AndrewTatham.Logic.Behaviors.Strategies.Radar;
using Moq;
using NUnit.Framework;

namespace AndrewTatham.UnitTests.Logic.Behaviours
{
    [TestFixture]
    public class SearchBehaviourUnitTests
    {
        private readonly BaseBehavior _behaviour = new SearchBehavior();

        [Test]
        [TestCase(2, 0, true)]
        [TestCase(1, 0, true)]
        [TestCase(1, 1, false)]
        public void Condition(
            int otherscount,
            int scannedCount,
            bool expected)
        {
            var mockContext = new Mock<IContext>();
            mockContext.Setup(x => x.OthersCount).Returns(otherscount);
            mockContext.Setup(x => x.ScannedCount).Returns(scannedCount);
            BaseBehavior.Context = mockContext.Object;
            Assert.AreEqual(expected, _behaviour.Condition());
        }

        [Test]
        public void Strategies()
        {
            Assert.IsInstanceOf(typeof(FullScanRadar), _behaviour.Strategies[0]);
            Assert.IsInstanceOf(typeof(SearchMovement), _behaviour.Strategies[1]);
        }
    }
}