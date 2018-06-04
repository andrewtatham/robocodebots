using System.Collections.Generic;

namespace AndrewTatham.BattleTests.TestCases
{
    public class ScoreBoard
    {
        private readonly Dictionary<ClassificationKey, Score> _scores;

        public ScoreBoard(Dictionary<ClassificationKey, Score> scores)
        {
            _scores = scores;
        }

        public bool ContainsKey(ClassificationKey key)
        {
            return _scores.ContainsKey(key);
        }

        public Score this[ClassificationKey key]
        {
            get
            {
                return _scores[key];
            }
        }
    }
}