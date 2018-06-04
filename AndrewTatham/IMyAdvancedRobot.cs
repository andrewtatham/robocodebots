using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Robocode;

namespace AndrewTatham
{
    public interface IMyAdvancedRobot
    {
        double DistanceRemaining { get; }

        double TurnRemaining { get; }

        double GunTurnRemaining { get; }

        double RadarTurnRemaining { get; }

        long DataQuotaAvailable { get; }

        bool IsInterruptible { set; }

        double MaxTurnRate { set; }

        double MaxVelocity { set; }

        double HeadingRadians { get; }

        double GunHeadingRadians { get; }

        double RadarHeadingRadians { get; }

        double GunTurnRemainingRadians { get; }

        double RadarTurnRemainingRadians { get; }

        double TurnRemainingRadians { get; }

        TextWriter Out { get; }

        double BattleFieldWidth { get; }

        double BattleFieldHeight { get; }

        double Heading { get; }

        double Height { get; }

        double Width { get; }

        string Name { get; }

        double X { get; }

        double Y { get; }

        double GunCoolingRate { get; }

        double GunHeading { get; }

        double GunHeat { get; }

        int NumRounds { get; }

        int Others { get; }

        double RadarHeading { get; }

        int RoundNum { get; }

        long Time { get; }

        double Velocity { get; }

        bool IsAdjustGunForRobotTurn { get; set; }

        bool IsAdjustRadarForRobotTurn { get; set; }

        bool IsAdjustRadarForGunTurn { get; set; }

        Color BodyColor { get; set; }

        Color GunColor { get; set; }

        Color RadarColor { get; set; }

        Color BulletColor { get; set; }

        Color ScanColor { get; set; }

        double Energy { get; }

        IGraphics Graphics { get; }

        Robot.DebugPropertyH DebugProperty { get; }

        void Run();

        void OnScannedRobot(ScannedRobotEvent evnt);

        void OnRobotDeath(RobotDeathEvent evnt);

        void OnBulletHitBullet(BulletHitBulletEvent evnt);

        void OnHitByBullet(HitByBulletEvent evnt);

        void OnPaint(IGraphics graphics);

        void OnSkippedTurn(SkippedTurnEvent evnt);

        void SetAhead(double distance);

        void SetBack(double distance);

        void SetTurnLeft(double degrees);

        void SetTurnRight(double degrees);

        void SetFire(double power);

        Bullet SetFireBullet(double power);

        void AddCustomEvent(Condition condition);

        void AddCustomEvent(string name, int priority, ConditionTest test);

        void RemoveCustomEvent(Condition condition);

        void ClearAllEvents();

        void Execute();

        IList<Event> GetAllEvents();

        IList<BulletHitBulletEvent> GetBulletHitBulletEvents();

        IList<BulletHitEvent> GetBulletHitEvents();

        IList<BulletMissedEvent> GetBulletMissedEvents();

        string GetDataDirectory();

        Stream GetDataFile(string filename);

        int GetEventPriority(string eventClass);

        IList<HitByBulletEvent> GetHitByBulletEvents();

        IList<HitRobotEvent> GetHitRobotEvents();

        IList<HitWallEvent> GetHitWallEvents();

        IList<RobotDeathEvent> GetRobotDeathEvents();

        IList<ScannedRobotEvent> GetScannedRobotEvents();

        IList<StatusEvent> GetStatusEvents();

        void OnCustomEvent(CustomEvent evnt);

        void SetEventPriority(string eventClass, int priority);

        void SetResume();

        void SetStop();

        void SetStop(bool overwrite);

        void SetTurnGunLeft(double degrees);

        void SetTurnGunRight(double degrees);

        void SetTurnRadarLeft(double degrees);

        void SetTurnRadarRight(double degrees);

        void WaitFor(Condition condition);

        void OnDeath(DeathEvent evnt);

        void SetTurnLeftRadians(double radians);

        void SetTurnRightRadians(double radians);

        void TurnLeftRadians(double radians);

        void TurnRightRadians(double radians);

        void SetTurnGunLeftRadians(double radians);

        void SetTurnGunRightRadians(double radians);

        void SetTurnRadarLeftRadians(double radians);

        void SetTurnRadarRightRadians(double radians);

        void TurnGunLeftRadians(double radians);

        void TurnGunRightRadians(double radians);

        void TurnRadarLeftRadians(double radians);

        void TurnRadarRightRadians(double radians);

        void Ahead(double distance);

        void Back(double distance);

        void TurnLeft(double degrees);

        void TurnRight(double degrees);

        void DoNothing();

        void Fire(double power);

        Bullet FireBullet(double power);

        void OnBulletHit(BulletHitEvent evnt);

        void OnBulletMissed(BulletMissedEvent evnt);

        void OnHitRobot(HitRobotEvent evnt);

        void OnHitWall(HitWallEvent evnt);

        void OnWin(WinEvent evnt);

        void OnRoundEnded(RoundEndedEvent evnt);

        void OnBattleEnded(BattleEndedEvent evnt);

        void Scan();

        void SetColors(Color bodyColor, Color gunColor, Color radarColor);

        void SetColors(Color bodyColor, Color gunColor, Color radarColor, Color bulletColor, Color scanArcColor);

        void SetAllColors(Color color);

        void Stop();

        void Stop(bool overwrite);

        void Resume();

        void TurnGunLeft(double degrees);

        void TurnGunRight(double degrees);

        void TurnRadarLeft(double degrees);

        void TurnRadarRight(double degrees);

        void OnKeyPressed(KeyEvent e);

        void OnKeyReleased(KeyEvent e);

        void OnKeyTyped(KeyEvent e);

        void OnMouseClicked(MouseEvent e);

        void OnMouseEntered(MouseEvent e);

        void OnMouseExited(MouseEvent e);

        void OnMousePressed(MouseEvent e);

        void OnMouseReleased(MouseEvent e);

        void OnMouseMoved(MouseEvent e);

        void OnMouseDragged(MouseEvent e);

        void OnMouseWheelMoved(MouseWheelMovedEvent e);

        void OnStatus(StatusEvent e);
    }
}