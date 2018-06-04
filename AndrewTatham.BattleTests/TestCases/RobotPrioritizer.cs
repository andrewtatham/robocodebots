using System.Collections.Generic;
using System.Linq;
using AndrewTatham.BattleTests.Fixtures;
using AndrewTatham.Helpers;

namespace AndrewTatham.BattleTests.TestCases
{
    public class RobotPrioritizer
    {
        public IEnumerable<BattleTestCase> TestCases { get; private set; }

        public RobotPrioritizer(Dictionary<ClassificationKey, ClassificationValue> classification)
        {
            var excluded = new[]
            {
                RobotClassification.NoShow,
                RobotClassification.Error,
                RobotClassification.Retired
            };

            var order = new Dictionary<RobotClassification, int>
            {
                {RobotClassification.New,0},
                {RobotClassification.Medium,1},
                {RobotClassification.Hard,2},
                {RobotClassification.Easy,3}
            };

            TestCases = classification
                .GroupBy(kvp => new
                {
                    kvp.Key.MyRobotName,
                    kvp.Key.BattleType,
                    kvp.Value.Classification
                },
                kvp => new
                {
                    kvp.Key.EnemyName,
                    kvp.Value.Rounds
                })

                .Where(kvp => !excluded.Contains(kvp.Key.Classification))

                .OrderBy(group => order[group.Key.Classification])

                .SelectMany(group =>
                    {
                        var priority = group
                            .OrderBy(kvp => kvp.Rounds)
                            .Select(r => r.EnemyName);

                        IEnumerable<IEnumerable<string>> enemies;
                        if (group.Key.BattleType == BattleType.OneVsOne)
                        {
                            enemies = priority.Select(enemyRobot => new[] { enemyRobot });
                        }
                        else
                        {
                            enemies = priority.SplitIntoGroupsOf(8);
                        }
                        return enemies.Select(ers => new BattleTestCase(group.Key.MyRobotName, ers));
                    });
        }
    }
}