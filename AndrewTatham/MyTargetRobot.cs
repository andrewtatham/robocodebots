using AndrewTatham.Logic.Behaviors;

namespace AndrewTatham
{
    public class MyTargetRobot : BaseAdvancedRobot
    {
        public MyTargetRobot()
            : base(new BaseBehavior[]
        {
            new TargetBehavior()
        })
        {
        }
    }
}