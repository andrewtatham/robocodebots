using AndrewTatham.Logic;
using AndrewTatham.Logic.Behaviors;
using AndrewTatham.Logic.Behaviors.Strategies.Targeting;
using Moq;
using NUnit.Framework;

namespace AndrewTatham.UnitTests.Logic.Behaviours
{
    [TestFixture]
    public class VictoryBehaviourUnitTests
    {
        private readonly BaseBehavior _behaviour = new VictoryBehavior();

        [Test]
        [TestCase(3, false)]
        [TestCase(2, false)]
        [TestCase(1, false)]
        [TestCase(0, true)]
        public void Condition(
            int othersCount,
            bool expected)
        {
            var mockContext = new Mock<IContext>();
            mockContext.Setup(x => x.OthersCount).Returns(othersCount);
            BaseBehavior.Context = mockContext.Object;
            Assert.AreEqual(expected, _behaviour.Condition());
        }

        [Test]
        public void Strategies()
        {
            Assert.IsInstanceOf(typeof(NoTarget), _behaviour.Strategies[0]);
        }
    }
}