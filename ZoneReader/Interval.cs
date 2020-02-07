using System;
using System.Collections.Generic;
using System.Text;

namespace ZoneReader
{
    public class Interval
    {
        public float Start { get; private set; }
        public  float End { get; private set; }

        public Interval(float start = float.NegativeInfinity, float end = float.PositiveInfinity)
        {
            Construct(start, end);
        }

        private void Construct(float start, float end)
        {
            Start = start;
            End = end;

            if (Start > End)
            {
                Start = start;
                End = end;
            }
        }

        public Interval(float? start , float? end)
        {
            // https://docs.microsoft.com/de-de/dotnet/csharp/language-reference/operators/null-coalescing-operator
            float startNonNull = start ?? float.NegativeInfinity;
            float endNonNull =  end ?? float.PositiveInfinity;

            Construct(startNonNull, endNonNull);
        }

        public List<float> ToList()
        {
            return new List<float> { Start, End };
        }

        public bool Contains(float test)
        {
            return (Start <= test && test <= End); 
        }

        /// <summary>
        /// Provides an interval that contains every number
        /// </summary>
        public static Interval NullInterval = new Interval();
    }
}
