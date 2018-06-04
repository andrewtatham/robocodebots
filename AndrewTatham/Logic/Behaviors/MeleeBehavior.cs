using AndrewTatham.Logic.Behaviors.Strategies;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming.Firing;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction;
using AndrewTatham.Logic.Behaviors.Strategies.Movement;
using AndrewTatham.Logic.Behaviors.Strategies.Movement.Forces;
using AndrewTatham.Logic.Behaviors.Strategies.Radar;
using AndrewTatham.Logic.Behaviors.Strategies.Targeting;

namespace AndrewTatham.Logic.Behaviors
{
    public class MeleeBehavior : BaseBehavior
    {
        public MeleeBehavior()
        {
            Strategies.AddRange(new BaseStrategy[]
                {
                    new FullScanRadar(),
                    new ForceMovement(
                        new Force[]
                            {
                                new AvoidCentreForce(),
                                new AvoidWallForce(),
                                new AvoidEnemyForce(),
                                new RandomForce(),
                                new MaximumEnemyDistanceForce()
                            }),
                    new TargetNearestEnemy(),
                    new MeleeFiringStrategy(),
                    new PredictionAlgorithms(
                        new PredictionAlgorithm[]
                        {
                            new CurrentPosition(),
                            new LastBlipVelocity(),
                            new PlayItForward()
                        })
                });
        }

        public override bool Condition()
        {
            return Context.ScannedCount > 1;
        }
    }
}