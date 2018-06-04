using AndrewTatham.Logic.Behaviors.Strategies.Targeting;

namespace AndrewTatham.Logic.Behaviors
{
    public class VictoryBehavior : BaseBehavior
    {
        public VictoryBehavior()
        {
            Strategies.Add(new NoTarget());
        }

        public override bool Condition()
        {
            return Context.OthersCount == 0;
        }
    }
}