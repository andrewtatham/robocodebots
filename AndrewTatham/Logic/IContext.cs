using System.IO;
using AndrewTatham.Helpers;
using AndrewTatham.Logic.Behaviors.Strategies.Aiming;
using AndrewTatham.Logic.Behaviors.Strategies.Radar;
using AndrewTatham.Logic.Enemies;
using Robocode;

namespace AndrewTatham.Logic
{
    public interface IContext
    {
        void NewTurn();

        void RunInit();

        IEnemy OnScannedRobot(ScannedRobotEvent evnt);

        double BattlefieldDiag { get; }

        double BattlefieldHeight { get; }

        double BattlefieldWidth { get; }

        Vector CentreAbsolute { get; }

        Vector CentreRelative { get; }

        long Time { get; }

        double MyEnergy { get; }

        double MyHeading { get; }

        Vector MyLocation { get; }

        TextWriter Out { get; }

        int ScannedCount { get; }

        int OthersCount { get; }

        Enemies.Enemies Enemies { get; }

        IEnemy Target { get; set; }

        AimResult AimResult { get; set; }

        Vector MoveToAbsolute { get; set; }

        RadarResult RadarResult { get; set; }

        void Render(IGraphics graphics);

        void OnHitByBullet(HitByBulletEvent evnt);

        void OnBulletHitBullet(BulletHitBulletEvent evnt);

        void OnRobotDeath(string name);
    }
}