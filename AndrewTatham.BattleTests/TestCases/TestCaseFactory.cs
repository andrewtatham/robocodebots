using System.Collections.Generic;
using System.IO;
using System.Linq;
using AndrewTatham.BattleTests.Fixtures;
using AndrewTatham.BattleTests.Helpers;
using AndrewTatham.BattleTests.Reports;

namespace AndrewTatham.BattleTests.TestCases
{
    public static class TestCaseFactory
    {
        private static readonly string[] MyRobots = {
            "AndrewTatham.MyAdvancedRobot"
        };

        public static IEnumerable<BattleTestCase> TestCases
        {
            get
            {
                return BuildTestCases();
            }
        }

        private static IEnumerable<BattleTestCase> BuildTestCases()
        {
            ZipHelper.DownloadLatestEnemiesZip();

            IEnumerable<string> allRobots = ZipHelper.GetAllRobots();

            string[] banned = { "hamilton.Hamilton 1.0", "apv.TheBrainPi 0.5fix" };

            var allExceptBanned = allRobots.Except(banned);

            using (var outcomes = new Outcomes())
            {
                var scores = outcomes.AllOutcomes.ToList()
                    .GroupBy(o => new ClassificationKey(o.MyRobotName, o.BattleType, o.EnemyName))
                    .ToDictionary(k => k.Key, v => new Score(v));

                var scoreboard = new ScoreBoard(scores);

                var classifier = new Classifier(MyRobots, allExceptBanned, scoreboard);

                var prioritizer = new RobotPrioritizer(classifier.Classifications);

#if DEBUG
                return prioritizer.TestCases.Take(5);
#else
                return prioritizer.TestCases;
#endif
            }
        }

        public static void AddResult(IEnumerable<Outcome> newOutcomes)
        {
            using (var outcomes = new Outcomes())
            {
                outcomes.AddOutcomes(newOutcomes);
            }
        }

        public static void GenerateScoreReport()
        {
            using (var outcomes = new Outcomes())
            {
                var myRobot = MyRobots.Single();
                var x = outcomes.AllOutcomes
                    .Where(o => o.MyRobotName == myRobot);
                var scores = new ScoreReportData(x);
                var report = new ScoreReport(scores);
                report.Save();
            }
        }

        private const string Path = "InProgress.txt";

        internal static void SaveToDisk(BattleTestCase testCase)
        {
            File.WriteAllText(Path, testCase.Serialize());
        }

        internal static void ForgetAnyUnfinishedBusiness()
        {
            File.Delete(Path);
        }

        internal static bool CheckForUnfinishedBusiness()
        {
            if (File.Exists(Path))
            {
                string data = File.ReadAllText(Path);
                var testCase = BattleTestCase.Deserialize(data);
                if (testCase != null)
                {
                    var adapter = new BattleResultAdapter();
                    var outcomes = adapter.RecordUnfinishedBusiness(testCase);
                    AddResult(outcomes);
                    return true;
                }
                File.Delete(Path);
            }
            return false;
        }


    }
}