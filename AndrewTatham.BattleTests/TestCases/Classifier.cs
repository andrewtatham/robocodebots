using System;
using System.Collections.Generic;
using System.Linq;
using AndrewTatham.BattleTests.Fixtures;

namespace AndrewTatham.BattleTests.TestCases
{
    public class Classifier
    {
        public Dictionary<ClassificationKey, ClassificationValue> Classifications { get; set; }

        public Classifier(
            IEnumerable<string> myRobots,
            IEnumerable<string> allRobots, ScoreBoard scoreboard)
        {
            Classifications = myRobots
                .SelectMany(myRobotName =>
                    Enum.GetValues(typeof(BattleType)).Cast<BattleType>().SelectMany(battleType =>
                    {
                        return allRobots.Select(enemyRobotName =>
                        {
                            var key = new ClassificationKey(myRobotName, battleType, enemyRobotName);

                            ClassificationValue value;

                            if (scoreboard.ContainsKey(key))
                            {
                                var score = scoreboard[key];

                                value = new ClassificationValue(score.Classification, score.Total);
                            }
                            else
                            {
                                value = new ClassificationValue(RobotClassification.New, 0);
                            }

                            return new { Key = key, Value = value };
                        });
                    }))
                .ToDictionary(
                k => k.Key,
                v => v.Value);
        }
    }

    public class ClassificationKey
    {
        public ClassificationKey(string myRobotName, BattleType battleType, string enemyName)
        {
            MyRobotName = myRobotName;
            BattleType = battleType;
            EnemyName = enemyName;
        }

        public string MyRobotName { get; private set; }

        public string EnemyName { get; private set; }

        public BattleType BattleType { get; private set; }

        public override bool Equals(object obj)
        {
            var other = obj as ClassificationKey;
            if (other != null)
            {
                return MyRobotName == other.MyRobotName
                    && EnemyName == other.EnemyName
                    && BattleType == other.BattleType

                    ;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return MyRobotName.GetHashCode()
                ^ EnemyName.GetHashCode()
                ^ BattleType.GetHashCode()
                ;
        }
    }

    public class ClassificationValue
    {
        public RobotClassification Classification { get; private set; }

        public int Rounds { get; private set; }

        public ClassificationValue(RobotClassification classification, int rounds)
        {
            Classification = classification;
            Rounds = rounds;
        }
    }
}