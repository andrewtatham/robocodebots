using System.Collections.Generic;
using System.Linq;
using AndrewTatham.BattleTests.Fixtures;

namespace AndrewTatham.BattleTests.TestCases
{
    public class Score
    {

        public Score(IEnumerable<Outcome> outcomes)
        {

            Won = outcomes.Count(o => o.OutcomeType == OutcomeType.Won);
            Lost = outcomes.Count(o => o.OutcomeType == OutcomeType.Lost);
            PossibleNoShow = outcomes.Count(o => o.OutcomeType == OutcomeType.PossibleNoShow);
            NoShow = outcomes.Count(o => o.OutcomeType == OutcomeType.NoShow);
            Error = outcomes.Count(o => o.OutcomeType == OutcomeType.Error);

            Total = new[]
            {
                Won,
                Lost,
                NoShow,
                PossibleNoShow,
                Error
            }.Sum();
            WinRatio =
                Won + Lost == 0
                ? null
                : (double?)(Won / (double)(Won + Lost));
            Classification = Classify();
        }

        public int Won { get; set; }

        public int Lost { get; set; }

        public int NoShow { get; set; }

        public int PossibleNoShow { get; set; }

        public int Error { get; set; }

        public int Total { get; set; }

        public double? WinRatio { get; set; }

        public RobotClassification Classification { get; set; }

        private RobotClassification Classify()
        {
            if (Total <= 2)
            {
                return RobotClassification.New;
            }
            if (Error >= 5)
            {
                return RobotClassification.Error;
            }
            if (NoShow >= 5 || PossibleNoShow >= 5)
            {
                return RobotClassification.NoShow;
            }
            if (WinRatio.HasValue && WinRatio.Value < 0.4)
            {
                return RobotClassification.Hard;
            }
            if (WinRatio.HasValue && 0.6 < WinRatio.Value)
            {
                return RobotClassification.Easy;
            }
            return RobotClassification.Medium;
        }
    }
}