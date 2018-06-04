using AndrewTatham.Helpers;

namespace AndrewTatham.Logic.Behaviors.Strategies.Movement.Forces
{
    public class AvoidWallForce : Force
    {
        public override Vector GetForceAt(Vector origin)
        {
            double w = Context.BattlefieldWidth;
            double h = Context.BattlefieldHeight;
            double x = origin.X;
            double y = origin.Y;

            Vector wallforce =
                new Vector(w * w / ((w - x) * (w - x)), new Angle(270))
                + new Vector(h * h / ((h - y) * (h - y)), new Angle(180))
                + new Vector(w * w / (x * x), new Angle(90))
                + new Vector(h * h / (y * y), new Angle(0));
            return wallforce;
        }
    }
}