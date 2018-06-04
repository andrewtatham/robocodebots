using System.Collections.Generic;
using AndrewTatham.Logic.Behaviors.Strategies.Movement.Forces;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies.Movement
{
    public class ForceMovement : BaseStrategy
    {
        private readonly ForceCollection _forceComponents;

        public ForceMovement(IEnumerable<Force> forceComponent)
        {
            _forceComponents = new ForceCollection(forceComponent);
        }

        public override void Execute()
        {
            Context.MoveToAbsolute += _forceComponents.GetRelativeForce(Context);
        }

        public override void Render(IGraphics graphics)
        {
            _forceComponents.Render(graphics);
        }
    }
}