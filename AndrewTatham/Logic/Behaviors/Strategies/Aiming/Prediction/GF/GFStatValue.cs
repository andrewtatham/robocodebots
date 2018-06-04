using System;
using System.Diagnostics.Contracts;
using AndrewTatham.Helpers;

namespace AndrewTatham.Logic.Behaviors.Strategies.Aiming.Prediction.GF
{
    public class GfStatValue
    {
        private const int Max = 4;
        private const int Length = 31;
        private const int HalfLength = (Length - 1) / 2;
        private readonly int[] _data;

        public GfStatValue()
        {
            _data = new int[Length];
        }

        public void Update(double gf)
        {
            Contract.Requires(-1 <= gf);
            Contract.Requires(gf <= 1);
            var index = (int)Math.Round(HalfLength + HalfLength * gf);

            //Contract.Assert(0 <= index);
            //Contract.Assert(index < length);
            _data[index]++;

            if (_data[index] > Max)
            {
                _data.ForEach(i => { if (i > 0) _data[i]--; });
            }
        }

        public GuessFactorData GetGuessFactorData()
        {
            int bestIndex = HalfLength;
            for (int i = 0; i < _data.Length; i++)
            {
                if (_data[bestIndex] < _data[i])
                {
                    bestIndex = i;
                }
            }

            //// ReSharper disable CompareOfFloatsByEqualityOperator
            //double confidence = sum == 0d ? 0d : _data[bestIndex] / sum;

            // ReSharper restore CompareOfFloatsByEqualityOperator
            double gf = (bestIndex - (double)HalfLength) / HalfLength;
            var retval = new GuessFactorData
                {
                    Data = _data,
                    Index = bestIndex,
                    GuessFactor = gf
                };
            return retval;
        }
    }
}