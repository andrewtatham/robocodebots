using NUnit.Framework;

namespace AndrewTatham.UnitTests.Logic.Behaviours.Strategies.Aiming.GuessFactor
{
    [TestFixture]
    public class GuessFactorAimingUnitTests
    {
        //[Test]
        //public void Execute()
        //{
        //    BaseStrategy.Out = new Mock<TextWriter>().Object;
        //    var mockEnemy = new Mock<IEnemy>();
        //    mockEnemy.Setup(mock => mock.Direct).Returns(new Vector(0, 25));
        //    mockEnemy
        //        .Setup(mock => mock.GetGuessFactorData(It.IsAny<IContext>()))
        //        .Returns(new GuessFactorData
        //            {
        //                Data = new[] {0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
        //                GuessFactor = 0.75d,
        //                Index = 17,
        //                ConfidenceFactor = 0.33d
        //            });

        //    var mockContext = new Mock<IContext>();
        //    mockContext.Setup(mock => mock.MyLocation).Returns(new Vector(50, 50));
        //    mockContext.Setup(mock => mock.Target).Returns(mockEnemy.Object);

        //    var strategy = new GuessFactorAiming();

        //    strategy.GetFuturePosition(mockContext.Object);

        //    //mockTurnContext.VerifySet(mock => mock.AimResult != null);
        //    var mockGraphics = new Mock<IGraphics>();

        //    strategy.Render(mockGraphics.Object);

        //    //mockGraphics.Verify(mock =>
        //    //    mock.FillEllipse(
        //    //    It.IsAny<Brush>(),
        //    //    It.IsAny<float>(),
        //    //    It.IsAny<float>(),
        //    //    It.IsAny<float>(),
        //    //    It.IsAny<float>()));
        //}
    }
}