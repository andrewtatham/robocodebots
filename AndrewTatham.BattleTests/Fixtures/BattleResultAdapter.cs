using System;
using System.Collections.Generic;
using System.Linq;
using AndrewTatham.BattleTests.TestCases;
using AndrewTatham.Helpers;
using Robocode;

namespace AndrewTatham.BattleTests.Fixtures
{
    public class BattleResultAdapter
    {
    

        public IEnumerable<Outcome> RecordOutcomes(
            BattleTestCase testCase,
            IEnumerable<BattleResults> sortedResults,
            IEnumerable<string> battleErrors,
            bool finished)
        {
            var retval = new Dictionary<string, Outcome>();
            var utcNow = DateTime.UtcNow;

            if (finished)
            {
                var actual = sortedResults.Select(r => r.TeamLeaderName);

                var noShows = testCase.EnemyRobots.Except(actual);

                foreach (var expectedRobotName in testCase.EnemyRobots)
                {
                    var name = expectedRobotName;
                    var roboterrors = battleErrors.Where(em => em.Contains(name));

                    //Can't load cb.nano.Chasseur 1.0, because it is invalid robot or team.

                    if (roboterrors.Any())
                    {
                        retval.Add(expectedRobotName, new Outcome
                        {
                            MyRobotName = testCase.MyRobot,
                            EnemyName = expectedRobotName,
                            OutcomeType = OutcomeType.Error,
                            BattleType = testCase.BattleType,
                            TimeStamp = utcNow,
                            Error = roboterrors.Distinct().Aggregate((e1, e2) => e1 + Environment.NewLine + e2)
                        });
                    }
                    else if (noShows.Contains(expectedRobotName))
                    {
                        retval.Add(expectedRobotName, new Outcome
                        {
                            MyRobotName = testCase.MyRobot,
                            EnemyName = expectedRobotName,
                            OutcomeType = OutcomeType.NoShow,
                            BattleType = testCase.BattleType,
                            TimeStamp = utcNow,
                            Error = null
                        });
                    }
                }

                bool isWin = false;
                foreach (var result in sortedResults)
                {
                    if (result.TeamLeaderName == testCase.MyRobot)
                    {
                        isWin = true;
                    }
                    else if (!retval.ContainsKey(result.TeamLeaderName))
                    {
                        retval.Add(result.TeamLeaderName, new Outcome
                        {
                            MyRobotName = testCase.MyRobot,
                            EnemyName = result.TeamLeaderName,
                            OutcomeType = isWin ? OutcomeType.Won : OutcomeType.Lost,
                            BattleType = testCase.BattleType,
                            TimeStamp = utcNow,
                            Error = null
                        });
                    }
                }
            }
            else
            {
                var noShows = testCase.EnemyRobots;
                noShows.ForEach(noShow =>
                {
                    retval.Add(noShow, new Outcome
                    {
                        MyRobotName = testCase.MyRobot,
                        EnemyName = noShow,
                        OutcomeType = testCase.BattleType == BattleType.OneVsOne ? OutcomeType.NoShow : OutcomeType.PossibleNoShow,
                        BattleType = testCase.BattleType,
                        TimeStamp = utcNow,
                        Error = null
                    });
                });
            }

           return retval.Values.ToList();
        }


        public IEnumerable<Outcome> RecordUnfinishedBusiness(BattleTestCase testCase)
        {

            var outcometype = testCase.BattleType == BattleType.OneVsOne ? OutcomeType.NoShow : OutcomeType.PossibleNoShow;
            var utcNow = DateTime.UtcNow;

            return testCase.EnemyRobots.Select(enemyName => new Outcome
            {
                MyRobotName = testCase.MyRobot,
                EnemyName = enemyName,
                OutcomeType = outcometype,
                BattleType = testCase.BattleType,
                TimeStamp = utcNow,
                Error = null
            }).ToList();
        }

 
    }
}