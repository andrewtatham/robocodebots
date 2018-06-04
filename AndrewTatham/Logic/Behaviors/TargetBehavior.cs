using AndrewTatham.Logic.Behaviors.Strategies;
using AndrewTatham.Logic.Behaviors.Strategies.Movement;
using AndrewTatham.Logic.Behaviors.Strategies.Movement.Forces;
using AndrewTatham.Logic.Behaviors.Strategies.Radar;
using AndrewTatham.Logic.Behaviors.Strategies.Targeting;

namespace AndrewTatham.Logic.Behaviors
{
    public class TargetBehavior : BaseBehavior
    {
        public TargetBehavior()
        {
            Strategies.AddRange(new BaseStrategy[]
            {
                new FullScanRadar(),
                new TargetOnly(),
                new ForceMovement(new Force[]
                {
                    new AvoidWallForce(),
                    new AvoidEnemyForce(),
                    new MiddleEnemyDistanceForce(),
                    new TangentialOscillationForce()
                })
            });
        }

        public override bool Condition()
        {
            return true;
        }
    }
}