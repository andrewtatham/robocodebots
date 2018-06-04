using AndrewTatham.Helpers;

namespace AndrewTatham.Logic.Behaviors.Strategies.Movement.Forces
{
    public class RandomForce : Force
    {
        private Vector _force;

        public override Vector GetForceAt(Vector origin)
        {
            if (Context.Time % 35 == 0)
            {
                _force = new Vector(
                    200d,
                    new Angle(RandomHelper.NextDouble(0, 360)));
            }

            return _force;
        }
    }
}