using System.Linq;

namespace AndrewTatham.Logic.Behaviors.Strategies.Targeting
{
    public class TargetOnly : BaseStrategy
    {
        public override void Execute()
        {
            Context.Target = Context.Enemies.Alive.FirstOrDefault();
        }
    }
}