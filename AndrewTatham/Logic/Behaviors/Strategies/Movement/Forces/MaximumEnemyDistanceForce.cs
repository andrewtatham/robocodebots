using System.Linq;
using AndrewTatham.Helpers;

namespace AndrewTatham.Logic.Behaviors.Strategies.Movement.Forces
{
    public class MaximumEnemyDistanceForce : Force
    {
        private Vector _bestLocation;
        private Vector[] _locations;

        public override Vector GetForceAt(Vector origin)
        {
            if (Context != null && _locations == null)
            {
                _locations = Vector.GetRandom(100, Context.BattlefieldWidth, Context.BattlefieldHeight);
            }

            if (Context.Enemies != null)
            {
                _bestLocation = _locations.OrderByDescending(candidate =>
                    {
                        double enemyScore = Context.Enemies.Alive.Average(enemy =>
                            {
                                //var energyFactor = (enemy.Energy / 100d);
                                double distanceFactor = (candidate - enemy.Location).Magnitude /
                                                        Context.BattlefieldDiag;
                                return distanceFactor;
                            });

                        return enemyScore;
                    }).FirstOrDefault();

                return _bestLocation - origin;
            }
            return null;
        }
    }
}