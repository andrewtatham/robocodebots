using AndrewTatham.Helpers;
using Robocode;

namespace AndrewTatham.Logic.Behaviors.Strategies.Radar
{
    public class ScanTargetRadar : BaseStrategy
    {
        private Vector _direct;
        private Vector _myLocation;
        private bool _right = true;

        public ScanTargetRadar()
        {
            Arc = Rules.RADAR_TURN_RATE / 5d;
        }

        public double Arc { get; private set; }

        public override void Execute()
        {
            _myLocation = Context.MyLocation;
            _direct = Context.Target.LastBlipDirect;

            if (_direct != null)
            {
                double turnto = _right ? _direct.Heading.Degrees + Arc : _direct.Heading.Degrees - Arc;

                Out.WriteLine("Radar: {0} {1}", _right, turnto);

                Context.RadarResult = new RadarResult
                    {
                        ScanType = ScanType.TurnTo,
                        TurnTo = new Angle(turnto)
                    };
                _right = !_right;
            }
            else
            {
                Context.RadarResult = new RadarResult
                    {
                        ScanType = ScanType.FullScan,
                        TurnTo = null
                    };
            }
        }

        public override void Render(IGraphics graphics)
        {
            if (_myLocation != null && _direct != null)
            {
                new[]
                    {
                        _myLocation + new Vector(_direct.Magnitude, new Angle(_direct.Heading.Degrees - Arc)),
                        _myLocation,
                        _myLocation + new Vector(_direct.Magnitude, new Angle(_direct.Heading.Degrees + Arc))
                    }.Render(graphics, ColourPalette.RadarPen);
            }
        }
    }
}