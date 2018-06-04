using AndrewTatham.Helpers;

namespace AndrewTatham.Logic.Behaviors.Strategies.Movement.Forces
{
    public class AvoidCentreForce : Force
    {
        public override Vector GetForceAt(Vector origin)
        {
            Vector centreVector = origin - Context.CentreAbsolute;

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (centreVector != null && centreVector.Magnitude != 0d)

            // ReSharper restore CompareOfFloatsByEqualityOperator
            {
                var centreForce = new Vector(
                    Context.BattlefieldDiag / centreVector.Magnitude,
                    centreVector.Heading);
                return centreForce;
            }
            return null;
        }
    }
}