using System.Linq;

namespace AndrewTatham.Logic.Behaviors.Strategies.Targeting
{
    public class TargetNearestEnemy : BaseStrategy
    {
        public override void Execute()
        {
            if (Context != null
                && Context.Enemies.Alive.Any())
            {
                Context.Target = Context.Enemies.Alive.OrderBy(e =>
                    {
                        if (e != null && e.Direct != null)
                        {
                            return (decimal?)e.Direct.Magnitude;
                        }
                        return null;
                    }).FirstOrDefault();
            }
        }
    }
}