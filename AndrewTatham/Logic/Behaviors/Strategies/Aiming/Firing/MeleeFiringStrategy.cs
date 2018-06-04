using System;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Firing
{
    public class MeleeFiringStrategy : BaseStrategy
    {
        public override void Execute()
        {
            CalculateBulletPowerFor(Context.Target);
        }

        private static void CalculateBulletPowerFor(IEnemy enemy)
        {
            if (enemy != null
                && enemy.Direct != null)
            {
                //var velocityFactor = context.TargetVelocity == null ? 1d : 1d - context.TargetVelocity.Magnitude / Rules.MAX_VELOCITY;
                //var orientationFactor = Math.Cos((context.Target.Heading - context.Target.EnemyDirect.Heading).Radians);

                // TODO set bullet power based on confidence of hitting / energy conservation / damage required
                double distance = enemy.Direct.Magnitude;
                double distanceFactor = 1d - distance / Context.BattlefieldDiag;
                double powerRange = Rules.MAX_BULLET_POWER - Rules.MIN_BULLET_POWER;
                double? bulletPower = Rules.MIN_BULLET_POWER + powerRange * distanceFactor;

                //* velocityFactor
                // * orientationFactor;

                // if the enemy has low energy, fire bullets of appropriate damage
                if (Rules.GetBulletDamage(bulletPower.Value) > enemy.Energy)
                {
                    bulletPower = Math.Max(Rules.MIN_BULLET_POWER,
                                           Math.Min(enemy.Energy / 4d, Rules.MAX_BULLET_POWER));
                }

                // fire weak bullets when my energy is low
                if (bulletPower.Value > Context.MyEnergy)
                {
                    bulletPower = Rules.MIN_BULLET_POWER;
                }

                // Do not fire if many bullets in the air
                if (enemy.Energy > 0d && enemy.Energy * 2d < enemy.MyBullets.DamageInTransit)
                {
                    bulletPower = null;
                }

                enemy.BulletPower = bulletPower;
            }
            else
            {
                enemy.BulletPower = null;
            }
        }

        public override void OnScannedRobot(IEnemy scannedEnemy)
        {
            CalculateBulletPowerFor(scannedEnemy);
        }
    }
}