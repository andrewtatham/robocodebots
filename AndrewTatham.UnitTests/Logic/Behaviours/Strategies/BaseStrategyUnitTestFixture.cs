using AndrewTatham.Logic.Behaviors.Strategies;
using NUnit.Framework;
using Robocode;

namespace AndrewTatham.UnitTests.Logic.Behaviours.Strategies
{
    [TestFixture]
    public class BaseStrategyUnitTestFixture
    {
        private class BaseStrategyAccessor : BaseStrategy
        {
            public override void Render(IGraphics graphics)
            {
            }

            public override void Execute()
            {
            }
        }

        private readonly BaseStrategy _baseStrategy = new BaseStrategyAccessor();

        [Test]
        public void Render()
        {
            _baseStrategy.Render(null);
        }

        [Test]
        public void Execute()
        {
            _baseStrategy.Execute();
        }

        [Test]
        public void OnScannedRobot()
        {
            _baseStrategy.OnScannedRobot(null);
        }

        [Test]
        public void OnFireBullet()
        {
            _baseStrategy.OnFireBullet(0d);
        }
    }
}