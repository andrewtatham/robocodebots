using System.Collections.Generic;
using System.Data.Entity;
using AndrewTatham.Helpers;

namespace AndrewTatham.BattleTests.Fixtures
{
    /// <summary>
    /// Persist the scores in this object
    /// </summary>
    ///

    public class Outcomes : DbContext
    {
        public DbSet<Outcome> AllOutcomes { get; set; }

        public void AddOutcomes(IEnumerable<Outcome> newOutcomes)
        {
            newOutcomes.ForEach(newOutcome =>
            {
                AllOutcomes.Add(newOutcome);
            });

            SaveChanges();
        }
    }
}