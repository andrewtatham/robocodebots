using System.Linq;
using AndrewTatham.Helpers;

namespace AndrewTatham.Logic.Behaviors.Strategies.Movement.Forces
{
    public class AvoidEnemyForce : Force
    {
        public override Vector GetForceAt(Vector origin)
        {
            if (Context.Enemies != null)
            {
                return Context.Enemies.Alive.Aggregate(
                    new Vector(0d, 0d),
                    (seed, enemy) =>
                    {
                        if (enemy != null && enemy.Location != null)
                        {
                            Vector enemyDirect = origin - enemy.Location;
                            if (enemyDirect != null)
                            {
                                seed += new Vector(
                                    Context.BattlefieldDiag * Context.BattlefieldDiag /
                                    (enemyDirect.Magnitude * enemyDirect.Magnitude),
                                    enemyDirect.Heading);
                            }
                        }
                        return seed;
                    });
            }
            return null;
        }
    }
}