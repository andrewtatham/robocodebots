using System;
using AndrewTatham.Helpers;

namespace AndrewTatham.Logic.Behaviors.Strategies.Movement.Forces
{
    public class MiddleEnemyDistanceForce : Force
    {
        private double _botWallDistance;
        private double _idealRadius;

        private double _leftWallDistance;

        // private Vector orbitCentre;
        private double _radialMagnitude;

        private double _radius;
        private double _rightWallDistance;
        private double _topWallDistance;

        public override Vector GetForceAt(Vector origin)
        {
            if (Context != null && Context.Target != null && Context.Target.Location != null && Context.Target.EnemyDirect != null && Context.CentreRelative != null)
            {
                //orbitCentre = Context.Target.Location;
                _leftWallDistance = Context.Target.Location.X;
                _rightWallDistance = Context.BattlefieldWidth - Context.Target.Location.X;
                _topWallDistance = Context.Target.Location.Y;
                _botWallDistance = Context.BattlefieldHeight - Context.Target.Location.Y;

                _idealRadius = 0.5d * Math.Max(
                    Math.Max(_leftWallDistance, _rightWallDistance),
                    Math.Max(_topWallDistance, _botWallDistance));
                _radius = Context.Target.EnemyDirect.Magnitude;

                // Todo tangential velocity, avoid walls, enenmy, favour centre
                _radialMagnitude = 0.1d * (_idealRadius - _radius);

                return new Vector(_radialMagnitude, Context.Target.EnemyDirect.Heading);
            }
            return null;
        }
    }
}