using AndrewTatham.Logic.Behaviors.Strategies.Movement;
using AndrewTatham.Logic.Behaviors.Strategies.Radar;

namespace AndrewTatham.Logic.Behaviors
{
    public class SearchBehavior : BaseBehavior
    {
        public SearchBehavior()
        {
            Strategies.Add(new FullScanRadar());
            Strategies.Add(new SearchMovement());
        }

        public override bool Condition()
        {
            return Context.OthersCount > 0 && Context.ScannedCount == 0;
        }
    }
}