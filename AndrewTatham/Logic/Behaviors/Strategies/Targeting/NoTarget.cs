namespace AndrewTatham.Logic.Behaviors.Strategies.Targeting
{
    public class NoTarget : BaseStrategy
    {
        public override void Execute()
        {
            Context.Target = null;
            Context.AimResult = null;
        }
    }
}