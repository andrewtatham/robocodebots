using System.Drawing;
using System.IO;
using AndrewTatham.Helpers;
using AndrewTatham.Logic;
using AndrewTatham.Logic.Behaviors;
using AndrewTatham.Logic.Behaviors.Strategies;
using Moq;
using NUnit.Framework;
using Robocode;

namespace AndrewTatham.UnitTests
{
    [TestFixture]
    public class RunSimulationUnitTestFixture
    {
        [Test]
        public void Run()
        {
            var mockOut = new Mock<TextWriter>();
            var mockGraphics = new Mock<IGraphics>();
            var myBot = new Mock<IBaseAdvancedRobot>(MockBehavior.Strict);

            IContext context = new Context(myBot.Object);

            BaseBehavior.Context = context;
            BaseStrategy.Context = context;

            var behaviours = new BaseBehavior[]
                    {
                        // new RunAwayAndHideBehaviour(),
                        new SearchBehavior(),
                        new MeleeBehavior(),
                        new OneVsOneBehavior(),
                        new VictoryBehavior()
                    };

            IBrain brain = new Brain(
               myBot.Object,
               behaviours,
               context);

            myBot.Setup(bot => bot.Out).Returns(mockOut.Object);
            myBot.Setup(bot => bot.SetColors(It.IsAny<Color>(), It.IsAny<Color>(), It.IsAny<Color>()));
            myBot.SetupProperty(bot => bot.IsAdjustGunForRobotTurn);
            myBot.SetupProperty(bot => bot.IsAdjustRadarForGunTurn);
            myBot.SetupProperty(bot => bot.IsAdjustRadarForRobotTurn);
            myBot.Setup(bot => bot.BattleFieldWidth).Returns(800d);
            myBot.Setup(bot => bot.BattleFieldHeight).Returns(600d);
            var startAt = RandomHelper.RandomLocation();
            myBot.Setup(bot => bot.X).Returns(startAt.X);
            myBot.Setup(bot => bot.Y).Returns(startAt.Y);

            brain.RunInit();

            myBot.Setup(bot => bot.GunHeading).Returns(0d);
            myBot.Setup(bot => bot.GunTurnRemaining).Returns(0d);
            myBot.Setup(bot => bot.GunHeat).Returns(0d);
            myBot.Setup(bot => bot.Others).Returns(10);

            myBot.Setup(bot => bot.SetTurnRadarRight(It.IsAny<double>()));
            myBot.Setup(bot => bot.SetTurnGunRight(It.IsAny<double>()));
            myBot.Setup(bot => bot.SetTurnRight(It.IsAny<double>()));
            myBot.Setup(bot => bot.SetAhead(It.IsAny<double>()));
            myBot.Setup(bot => bot.SetFireBullet(It.IsAny<double>()))
                .Returns(RandomHelper.RandomBullet());

            myBot.Setup(bot => bot.Heading).Returns(RandomHelper.RandomHeading());

            myBot.Setup(bot => bot.Execute());

            myBot.Setup(bot => bot.GunCoolingRate).Returns(5);
            myBot.Setup(bot => bot.Energy).Returns(RandomHelper.RandomEnergy());

            for (int turn = 0; turn < 200; turn++)
            {
                brain.Render(mockGraphics.Object);

                myBot.Setup(bot => bot.Time).Returns(turn);

                if (turn % 7 == 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        brain.OnScannedRobot(new ScannedRobotEvent(
                            RandomHelper.RandomRobotName(),
                            RandomHelper.RandomEnergy(),
                            RandomHelper.RandomBearing(),
                            RandomHelper.RandomDistance(),
                            RandomHelper.RandomHeading(),
                            RandomHelper.RandomVelocity()
                            ));
                    }
                }

                brain.Run();
            }
        }
    }
}