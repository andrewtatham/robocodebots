using AndrewTatham.Helpers;

namespace AndrewTatham.Logic.Behaviors.Strategies.Movement
{
    public class SearchMovement : BaseStrategy
    {
        private static Vector[] _waypoints;
        private static int _i;

        public override void Execute()
        {
            _waypoints = new[]
                {
                    new Vector(0d, 0d),
                    new Vector(Context.BattlefieldWidth, 0d),
                    new Vector(0d, Context.BattlefieldHeight),
                    new Vector(Context.BattlefieldWidth, Context.BattlefieldHeight)
                };
            if (Context.MyLocation != null
                && (Context.MyLocation - _waypoints[_i]).Magnitude < 0.25D * Context.BattlefieldDiag)
            {
                // move to next waypoint
                _i++;

                if (_i >= _waypoints.Length)
                {
                    _i = 0;
                }
            }
            Context.MoveToAbsolute = _waypoints[_i];
        }
    }
}