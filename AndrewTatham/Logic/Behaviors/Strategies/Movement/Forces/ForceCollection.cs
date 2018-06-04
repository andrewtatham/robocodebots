using System.Collections.Generic;
using System.Linq;
using AndrewTatham.Helpers;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies.Movement.Forces
{
    public class ForceCollection : IRender
    {
        private readonly IEnumerable<Force> _forceComponents;

        //public readonly IEnumerable<Vector[]> fieldPairs = null;

        public ForceCollection(IEnumerable<Force> forceComponents)
        {
            _forceComponents = forceComponents.AsParallel();

            //var temp = new List<Vector[]>();
            //for (int x = 0; x < N; x++)
            //{
            //    for (int y = 0; y < N; y++)
            //    {
            //        var origin = new Vector(
            //                BattleFieldWidth * (double)x / (double)N,
            //                BattleFieldHeight * (double)y / (double)N
            //            );
            //        var force = new Vector(0, 0);
            //        temp.Add(new Vector[]
            //        {
            //            origin,
            //            force
            //        });
            //    }
            //}
            //fieldPairs = temp.AsParallel();
        }

        //public static int N { get; set; }
        //public static double BattleFieldWidth { get; set; }
        //public static double BattleFieldHeight { get; set; }

        public void Render(IGraphics graphics)
        {
            //fieldPairs.ForEach(p =>
            //{
            //    //p.Render(graphics, ColourPallette.AntiGravityMovementPen);
            //});
        }

        public Vector GetRelativeForce(IContext context)
        {
            Force.Context = context;

            //CalculateField();

            Vector relativeResultant = _forceComponents.Aggregate(
                new Vector(0, 0),
                (seed, component) =>
                {
                    Vector rel = component.GetForceAt(context.MyLocation);
                    if (rel != null)
                    {
                        seed += rel;
                    }
                    return seed;
                });
            return relativeResultant;
        }

        //public void CalculateField()
        //{
        //    fieldPairs.ForEach(p =>
        //        {
        //            p[1] = p[0] + _forceComponents.Aggregate(
        //                new Vector(0, 0),
        //                (seed, component) =>
        //                    {
        //                        Vector rel = component.GetForceAt(p[0]);
        //                        if (rel != null)
        //                        {
        //                            seed += rel;
        //                        }
        //                        return seed;
        //                    });
        //        });
        //}
    }
}