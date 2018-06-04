using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AndrewTatham.Logic;
using AndrewTatham.Logic.Behaviors;
using AndrewTatham.Logic.Behaviors.Strategies;
using AndrewTatham.Logic.Enemies;
using Moq;
using NUnit.Framework;
using Robocode;

namespace AndrewTatham.UnitTests.Logic.Behaviours
{
    [TestFixture]
    public class BaseBehaviourUnitTests
    {
        private BaseBehavior _behaviour;

        private readonly Mock<IContext> _mockContext = new Mock<IContext>();

        private readonly Mock<IGraphics> _mockGraphics = new Mock<IGraphics>();

        private readonly Mock<TextWriter> _mockOut = new Mock<TextWriter>();

        private Mock<BaseStrategy>[] _strategies;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            BaseBehavior.Out = _mockOut.Object;

            int n = 5;
            _strategies = new Mock<BaseStrategy>[n];
            for (int i = 0; i < n; i++)
            {
                var mock = new Mock<BaseStrategy>();
                _strategies[i] = mock;
            }
            BaseBehavior.Context = _mockContext.Object;
            _behaviour = new BaseBehaviourAccessor(_strategies.Select(mock => mock.Object));
        }

        [Test]
        public void Execute()
        {
            _behaviour.Execute();

            _strategies.ToList().ForEach(mock => mock.Verify(m => m.Execute()));
        }

        [Test]
        public void OnFireBullet()
        {
            double power = 3.2d;
            _behaviour.OnFireBullet(power);

            _strategies.ToList().ForEach(mock => mock.Verify(m => m.OnFireBullet(power)));
        }

        [Test]
        public void OnScannedRobot()
        {
            var mockEnemy = new Mock<IEnemy>();

            _behaviour.OnScannedRobot(mockEnemy.Object);

            _strategies.ToList().ForEach(mock => mock.Verify(m => m.OnScannedRobot(mockEnemy.Object)));
        }

        [Test]
        public void Render()
        {
            _behaviour.Render(_mockGraphics.Object);

            _strategies.ToList().ForEach(mock => mock.Verify(m => m.Render(_mockGraphics.Object)));
        }

        private class BaseBehaviourAccessor : BaseBehavior
        {
            public BaseBehaviourAccessor(IEnumerable<BaseStrategy> strategies)
            {
                Strategies.AddRange(strategies);
            }

            public override bool Condition()
            {
                throw new NotImplementedException();
            }
        }
    }
}