using System;
using System.Diagnostics.Contracts;
using AndrewTatham.Helpers;
using Robocode;

namespace AndrewTatham.Logic.Enemies
{
    public class EnemyGuessFactorStatistics : IRender
    {
        private const int Length = 31;
        private const int HalfLength = (Length - 1) / 2;

        public EnemyGuessFactorStatistics()
        {
            Data = RandomHelper.IntArray(Length, 30);
        }

        private int[] Data { get; set; }

        public void Update(double gf)
        {
            Contract.Requires(-1 <= gf);
            Contract.Requires(gf <= 1);

            var index = (int)Math.Round(HalfLength + HalfLength * gf);

            Data[index]++;
        }

        public void Update(double[] newGuessFactors)
        {
            foreach (double gf in newGuessFactors)
            {
                Update(gf);
            }
        }

        public EnemyGuessFactorData GuessFactorData
        {
            get
            {
                int bestIndex = HalfLength;

                for (int i = 0; i < Data.Length; i++)
                {
                    if (Data[bestIndex] > Data[i])
                    {
                        bestIndex = i;
                    }
                }

                return new EnemyGuessFactorData
                    {
                        GuessFactor = (bestIndex - (double)HalfLength) / HalfLength,
                        Statistics = Data
                    };
            }
        }

        #region IRender Members

        public void Render(IGraphics graphics)
        {
        }

        #endregion IRender Members
    }
}