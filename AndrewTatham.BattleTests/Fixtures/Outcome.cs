using System;
using System.ComponentModel.DataAnnotations;

namespace AndrewTatham.BattleTests.Fixtures
{
    public class Outcome
    {
        [Key]
        public int OutcomeId { get; set; }

        [Required]
        public string MyRobotName { get; set; }

        [Required]
        public string EnemyName { get; set; }

        [Required]
        public BattleType BattleType { get; set; }

        [Required]
        public OutcomeType OutcomeType { get; set; }

        public string Error { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }
    }
}