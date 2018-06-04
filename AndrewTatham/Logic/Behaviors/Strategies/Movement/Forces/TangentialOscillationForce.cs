using System;
using AndrewTatham.Helpers;

namespace AndrewTatham.Logic.Behaviors.Strategies.Movement.Forces
{
    public class TangentialOscillationForce : Force
    {
        private double _tangentialMagnitude;

        public override Vector GetForceAt(Vector origin)
        {
            if (Context == null || Context.Target == null || Context.Target.Location == null ||
                Context.Target.EnemyDirect == null || Context.CentreRelative == null)
                return null;
            // pull toward centre
            _tangentialMagnitude =
                0.5d*Context.CentreRelative.Magnitude
                *Math.Sin((Context.CentreRelative.Heading - Context.Target.EnemyDirect.Heading).Radians);

            // dodge
            _tangentialMagnitude += 0.1d*Context.BattlefieldDiag
                //* Math.Sin(2d * Math.PI * Context.Time / 120d)
                                    *Math.Cos(2d*Math.PI*Context.Time/70d);

            return new Vector(_tangentialMagnitude, Context.Target.EnemyDirect.Heading.Perpendicular);
        }
    }
}