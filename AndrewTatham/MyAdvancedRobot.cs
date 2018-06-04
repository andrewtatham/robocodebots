using AndrewTatham.Logic.Behaviors;

namespace AndrewTatham
{
    // ReSharper disable UnusedMember.Global
    public class MyAdvancedRobot : BaseAdvancedRobot, IMyAdvancedRobot

    // ReSharper restore UnusedMember.Global
    {
        public MyAdvancedRobot()
            : base(new BaseBehavior[]
                    {
                        // new RunAwayAndHideBehaviour(),
                        new SearchBehavior(),
                        new MeleeBehavior(),
                        new OneVsOneBehavior(),
                        new VictoryBehavior()
                    })
        {
        }
    }
}