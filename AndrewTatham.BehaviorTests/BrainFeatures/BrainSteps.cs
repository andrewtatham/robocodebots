using System.Drawing;
using System.IO;
using AndrewTatham.Helpers;
using AndrewTatham.Logic;
using AndrewTatham.Logic.Behaviors;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming;
using AndrewTatham.Logic.Behaviors.Strategies.Radar;
using AndrewTatham.Logic.Enemies;
using Moq;
using NUnit.Framework;
using Robocode;
using TechTalk.SpecFlow;

namespace AndrewTatham.BehaviorTests.BrainFeatures
{
    [Binding]
    public class BrainSteps
    {
        private readonly Mock<BaseBehavior> _mockBehave1 = new Mock<BaseBehavior>();
        private readonly Mock<BaseBehavior> _mockBehave2 = new Mock<BaseBehavior>();
        private readonly Mock<BaseBehavior> _mockBehave3 = new Mock<BaseBehavior>();

        private readonly Mock<IBaseAdvancedRobot> _mockBot = new Mock<IBaseAdvancedRobot>();
        private readonly Mock<IContext> _mockContext = new Mock<IContext>();
        private readonly Mock<IGraphics> _mockGraphics = new Mock<IGraphics>();
        private readonly Mock<IEnemy> _mockTarget = new Mock<IEnemy>();
        private readonly Mock<TextWriter> _mockOut = new Mock<TextWriter>();
        private readonly IBrain _brain;

        private string _deadRobotName;

        public BrainSteps()
        {
            _mockBehave1.Setup(mock => mock.Condition()).Returns(false);
            _mockBehave2.Setup(mock => mock.Condition()).Returns(true);
            _mockBehave3.Setup(mock => mock.Condition()).Returns(true);

            _mockBot.Setup(bot => bot.Out).Returns(_mockOut.Object);

            _mockTarget.Setup(c => c.MyBullets).Returns(new BulletCollection(""));
            var mockBullet = RandomHelper.RandomBullet();
            _mockBot
                .Setup(b => b.SetFireBullet(It.IsInRange(Rules.MIN_BULLET_POWER, Rules.MAX_BULLET_POWER, Range.Inclusive)))
                .Returns(mockBullet);

            _brain = new Brain(
                _mockBot.Object,
                new[]
                    {
                        _mockBehave1.Object,
                        _mockBehave2.Object
                    },
                _mockContext.Object);
        }

        [Given(@"A new Brain is created")]
        public void GivenANewBrainIsCreated()
        {
        }

        [Given(@"an aim result is set")]
        public void GivenAnAimResultIsSet()
        {
            _mockContext.Setup(c => c.AimResult)
                .Returns(new AimResult
                {
                    Location = RandomHelper.RandomLocation(),
                    Power = RandomHelper.RandomBulletPower(),
                    Target = _mockTarget.Object
                });
        }

        [Given(@"a target is set")]
        public void GivenATargetIsSet()
        {
            _mockTarget.Setup(e => e.Location)
                .Returns(RandomHelper.RandomLocation());

            _mockTarget.Setup(e => e.Direct)
                .Returns(RandomHelper.RandomLocation());

            _mockContext.Setup(c => c.Target)
                .Returns(_mockTarget.Object);
        }

        [Given(@"the gun is aimed")]
        public void GivenTheGunIsAimed()
        {
            _mockBot.Setup(b => b.GunTurnRemaining).Returns(0);
        }

        [Given(@"the gun is cool")]
        public void GivenTheGunIsCool()
        {
            _mockBot.Setup(b => b.GunHeat).Returns(0);
        }

        [Given(@"the gun is NOT aimed")]
        public void GivenTheGunIsNotAimed()
        {
            _mockBot.Setup(b => b.GunTurnRemaining).Returns(1);
        }

        [Given(@"the gun is NOT cool")]
        public void GivenTheGunIsNotCool()
        {
            _mockBot.Setup(b => b.GunHeat).Returns(1);
        }

        [Given(@"RunInit is called")]
        [When(@"RunInit is called")]
        public void RunInitIsCalled()
        {
            _brain.RunInit();
        }

        [Given(@"Run is called")]
        [When(@"Run is called")]
        public void RunIsCalled()
        {
            _brain.Run();
        }

        [Then(@"a new context is initialised")]
        public void ThenANewContextIsInitialised()
        {
            _mockContext.Verify(mock => mock.RunInit());
        }

        [Then(@"a new turn is created in the context")]
        public void ThenANewTurnIsCreatedInTheContext()
        {
            _mockContext.Verify(mock => mock.NewTurn());
        }

        [Then(@"execute is called on the behavior")]
        public void ThenExecuteIsCalledOnTheBehavior()
        {
            _mockBehave2.Verify(mock => mock.Execute());
        }

        [Then(@"execute is called on the robot")]
        public void ThenExecuteIsCalledOnTheRobot()
        {
            _mockBot.Verify(mock => mock.Execute());
        }

        [Then(@"""(.*)"" is logged")]
        public void ThenIsLogged(string logMessage)
        {
            _mockOut.Verify(x => x.WriteLine(logMessage));
        }

        [Then(@"Render is called on the context")]
        public void ThenRenderIsCalledOnTheContext()
        {
            _mockContext.Verify(ctx => ctx.Render(_mockGraphics.Object));
        }

        [Then(@"Render is called on the current behavior")]
        public void ThenRenderIsCalledOnTheCurrentBehavior()
        {
            _mockBehave2.Verify(x => x.Render(_mockGraphics.Object));
        }

        [Then(@"The behaviour whose condition is true is selected")]
        public void ThenTheBehaviourWhoseConditionIsTrueIsSelected()
        {
            _mockBehave1.Verify(mock => mock.Condition());
            _mockBehave2.Verify(mock => mock.Condition());

            Assert.AreEqual(_mockBehave2.Object, _brain.SelectedBehavior);
        }

        [Then(@"the enemies collection is updated with the deceased robot")]
        public void ThenTheEnemiesCollectionIsUpdatedWithTheDeceasedRobot()
        {
            _mockContext.Verify(ctx => ctx.OnRobotDeath(_deadRobotName));
        }

        [Then(@"the enemies collection is updated with the new robot")]
        public void ThenTheEnemiesCollectionIsUpdatedWithTheNewRobot()
        {
            _mockContext.Verify(ctx => ctx.OnScannedRobot(It.IsAny<ScannedRobotEvent>()));
        }

        [Then(@"the gun is aimed at the target")]
        public void ThenTheGunIsAimedAtTheTarget()
        {
            _mockBot.Verify(mock => mock.SetTurnGunRight(It.IsAny<double>()));
        }

        [Then(@"the gun is fired at the target")]
        public void ThenTheGunIsFiredAtTheTarget()
        {
            _mockBot
                .Verify(mock =>
                    mock.SetFireBullet(It.IsInRange(Rules.MIN_BULLET_POWER, Rules.MAX_BULLET_POWER, Range.Inclusive)),
                    Times.Once());
        }

        [Then(@"the gun is NOT aimed at the target")]
        public void ThenTheGunIsNotAimedAtTheTarget()
        {
            _mockBot.Verify(mock => mock.SetTurnGunRight(It.IsAny<double>()), Times.Never());
        }

        [Then(@"the gun is NOT fired at the target")]
        public void ThenTheGunIsNotFiredAtTheTarget()
        {
            _mockBot
                .Verify(mock =>
                mock.SetFireBullet(It.IsInRange(Rules.MIN_BULLET_POWER, Rules.MAX_BULLET_POWER, Range.Inclusive)),
                Times.Never());
        }

        [Then(@"the move calls are made")]
        public void ThenTheMoveCallsAreMade()
        {
            _mockBot.Verify(mock => mock.SetTurnRight(It.IsAny<double>()));
            _mockBot.Verify(mock => mock.SetAhead(It.IsAny<double>()));
        }

        [Then(@"the new target is aimed at")]
        public void ThenTheNewTargetIsAimedAt()
        {
            _mockBot.Verify(mock => mock.SetTurnGunRight(It.IsAny<double>()));
        }

        [Then(@"the radar turn is set")]
        public void ThenTheRadarTurnIsSet()
        {
            // todo
            _mockBot.Verify(mock => mock.SetTurnRadarRight(It.IsAny<double>()));
        }

        [Then(@"The robots colours should be set to Magenta")]
        public void ThenTheRobotsColoursShouldBeSetToMagenta()
        {
            _mockBot.Verify(bot => bot.SetColors(Color.Magenta, Color.Magenta, Color.Magenta));
        }

        [Then(@"The robots radar and turret should move independantly of its body")]
        public void ThenTheRobotsRadarAndTurretShouldMoveIndependantlyOfItsBody()
        {
            _mockBot.VerifySet(bot => bot.IsAdjustGunForRobotTurn = true);
            _mockBot.VerifySet(bot => bot.IsAdjustRadarForGunTurn = true);
            _mockBot.VerifySet(bot => bot.IsAdjustRadarForRobotTurn = true);
        }

        [When(@"a robot is scanned")]
        public void WhenARobotIsScanned()
        {
            _brain.OnScannedRobot(new ScannedRobotEvent(
                RandomHelper.RandomRobotName(),
                RandomHelper.RandomEnergy(),
                RandomHelper.RandomBearing(),
                RandomHelper.RandomDistance(),
                RandomHelper.RandomHeading(),
                RandomHelper.RandomVelocity()

                ));
        }

        [When(@"OnBulletHitBullet is called")]
        public void WhenOnBulletHitBulletIsCalled()
        {
            var b1 = RandomHelper.RandomBullet();

            var b2 = RandomHelper.RandomBullet();

            var e = new BulletHitBulletEvent(b1, b2);

            _brain.OnBulletHitBullet(e);
        }

        [When(@"OnHitByBullet is called")]
        public void WhenOnHitByBulletIsCalled()
        {
            var b = RandomHelper.RandomBullet();
            var e = new HitByBulletEvent(RandomHelper.RandomBearing(), b);
            _brain.OnHitByBullet(e);
        }

        [When(@"OnRobotDeath is called")]
        public void WhenOnRobotDeathIsCalled()
        {
            _deadRobotName = RandomHelper.RandomRobotName();
            var e = new RobotDeathEvent(_deadRobotName);

            _brain.OnRobotDeath(e);
        }

        [When(@"OnScannedRobot is called")]
        public void WhenOnScannedRobotIsCalled()
        {
            var e = new ScannedRobotEvent("x", 100, 0, 100, 0, 0);

            _brain.OnScannedRobot(e);
        }

        [When(@"OnSkippedTurn is called")]
        public void WhenOnSkippedTurnIsCalled()
        {
            _brain.OnSkippedTurn(null);
        }

        [When(@"Render is called")]
        public void WhenRenderIsCalled()
        {
            _brain.Render(_mockGraphics.Object);
        }

        [Given(@"the radar result is set to full scan")]
        public void GivenTheRadarResultIsSetToFullScan()
        {
            _mockContext.Setup(c => c.RadarResult)
                .Returns(new RadarResult
                {
                    ScanType = ScanType.FullScan,
                    TurnTo = null
                });
        }

        [Given(@"the radar result is set to scan target")]
        public void GivenTheRadarResultIsSetToScanTarget()
        {
            _mockContext.Setup(c => c.RadarResult)
                .Returns(new RadarResult
                {
                    ScanType = ScanType.TurnTo,
                    TurnTo = RandomHelper.RandomAngle()
                });
        }

        [Given(@"the radar result is NOT set")]
        public void GivenTheRadarResultIsNotSet()
        {
            _mockContext.Setup(c => c.RadarResult)
                .Returns<RadarResult>(null);
        }

        [Then(@"the radar performs a full scan")]
        public void ThenTheRadarPerformsAFullScan()
        {
            _mockBot.Verify(b => b.SetTurnRadarRight(360));
        }

        [Then(@"the radar performs a target scan")]
        public void ThenTheRadarPerformsATargetScan()
        {
            _mockBot.Verify(b => b.SetTurnRadarRight(It.IsAny<double>()));
        }

        [Given(@"the move to absolute location is set")]
        public void GivenTheMoveToAbsoluteLocationIsSet()
        {
            _mockBot.Setup(b => b.X).Returns(RandomHelper.RandomX());
            _mockBot.Setup(b => b.Y).Returns(RandomHelper.RandomY());
            _mockContext.Setup(c => c.MyLocation).Returns(RandomHelper.RandomLocation());
            _mockContext.Setup(c => c.MoveToAbsolute).Returns(RandomHelper.RandomLocation());
        }

        [Then(@"the robot moves toward the location")]
        public void ThenTheRobotMovesTowardTheLocation()
        {
            _mockBot.Verify(b => b.SetTurnRight(It.IsAny<double>()));
            _mockBot.Verify(b => b.SetAhead(It.IsAny<double>()));
        }
    }
}