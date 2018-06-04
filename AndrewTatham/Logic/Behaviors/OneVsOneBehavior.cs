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
    public class OneVsOneBehavior : BaseBehavior
    {
        public OneVsOneBehavior()
        {
            Strategies.AddRange(new BaseStrategy[]
                {
                    new TargetOnly(), // Before Ratdar
                    new ScanTargetRadar(), // After Targeting
                    new ForceMovement(
                        new Force[]
                            {
                                new AvoidWallForce(),
                                new AvoidEnemyForce(),
                                new MiddleEnemyDistanceForce(),
                                new TangentialOscillationForce()
                            }),
                    new OneVsOneFiringStrategy(),
                    new PredictionAlgorithms(
                        new PredictionAlgorithm[]
                        {
                            new CurrentPosition(),
                            new LastBlipVelocity(),
                            new PlayItForward(),
                            new GuessFactorAiming()
                        })
                });
        }

        public override bool Condition()
        {
            return Context.OthersCount == 1 && Context.ScannedCount == 1;
        }
    }
}