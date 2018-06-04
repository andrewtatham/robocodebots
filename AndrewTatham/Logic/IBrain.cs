using AndrewTatham.Helpers;
using AndrewTatham.Logic.Behaviors;
using Robocode;

namespace AndrewTatham.Logic
{
    public interface IBrain : IRender
    {
        void RunInit();

        void Run();

        void OnScannedRobot(ScannedRobotEvent evnt);

        void OnRobotDeath(RobotDeathEvent evnt);

        void OnBulletHitBullet(BulletHitBulletEvent evnt);

        void OnHitByBullet(HitByBulletEvent evnt);

        void OnSkippedTurn(SkippedTurnEvent evnt);

        BaseBehavior SelectedBehavior { get; }
    }
}