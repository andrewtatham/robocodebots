namespace AndrewTatham.Logic.Behaviors.Strategies.Radar
{
    public class FullScanRadar : BaseStrategy
    {
        private static readonly RadarResult Result = new RadarResult { ScanType = ScanType.FullScan };

        public override void Execute()
        {
            Context.RadarResult = Result;
        }
    }
}