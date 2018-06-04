using System.Collections.Generic;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction.GF;
using Robocode;

namespace AndrewTatham.Logic.Enemies
{
    public interface IEnemy : IRender
    {
        Vector Location { get; }

        bool IsDead { get; }

        double Energy { get; }

        Angle Heading { get; }

        Vector Direct { get; }

        Vector EnemyDirect { get; }

        Vector VelocityVector { get; }

        Vector LateralVelocityVector { get; }

        Vector RadialVelocityVector { get; }

        double LateralVelocityScalar { get; }

        double RadialVelocityScalar { get; }

        Vector AccelerationVector { get; }

        Vector LateralAccelerationVector { get; }

        Vector RadialAccelerationVector { get; }

        double LateralAccelerationScalar { get; }

        double RadialAccelerationScalar { get; }

        PredictionCollection Predictions { get; }

        Blips Blips { get; }

        WaveCollection Waves { get; }

        VirtualBullets VirtualBullets { get; }

        void OnScannedRobot(IContext context, ScannedRobotEvent evnt);

        GuessFactorStatistics GuessFactorStats { get; }

        void RegisterHitByBullet(HitByBulletEvent evnt);

        void RegisterBulletHitBullet(BulletHitBulletEvent evnt);

        void RegisterRobotDeath(string name);

        GuessFactorData GetGuessFactorData();

        EnemyGuessFactorData EnemyGuessFactorData { get; }

        void OnTurn(Context context);

        Vector LastBlipDirect { get; }

        Vector LastBlipLocation { get; }

        Vector LastBlipEnemyDirect { get; }

        Vector PenultimateBlipLocation { get; }

        BulletCollection MyBullets { get; }

        double? BulletPower { get; set; }

        IEnumerable<Vector> GetPifData(long delta);
    }
}