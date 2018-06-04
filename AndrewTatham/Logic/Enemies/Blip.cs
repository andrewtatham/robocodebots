using AndrewTatham.Helpers;
using Robocode;

namespace AndrewTatham.Logic.Enemies
{
    public class Blip
    {
        public Blip(Vector relativeToPosition, double relativeToHeading, ScannedRobotEvent scannedRobotEvent)
        {
            Location = relativeToPosition + new Vector(scannedRobotEvent.Distance, new Angle(relativeToHeading + scannedRobotEvent.Bearing));
            Energy = scannedRobotEvent.Energy;
            Velocity = scannedRobotEvent.Velocity;
            Heading = scannedRobotEvent.Heading;
            Time = scannedRobotEvent.Time;
        }

        public Vector Location { get; private set; }

        public double Energy { get; private set; }

        public double Velocity { get; private set; }

        public double Heading { get; private set; }

        public long Time { get; private set; }
    }
}