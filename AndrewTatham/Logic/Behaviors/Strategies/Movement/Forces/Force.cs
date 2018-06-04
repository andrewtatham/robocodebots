using AndrewTatham.Helpers;

namespace AndrewTatham.Logic.Behaviors.Strategies.Movement.Forces
{
    public abstract class Force
    {
        public static IContext Context { get; set; }

        public abstract Vector GetForceAt(Vector origin);
    }
}