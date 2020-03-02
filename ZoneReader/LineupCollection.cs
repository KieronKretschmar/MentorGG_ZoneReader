using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZoneReader.Enums;
using ZoneReader.LineUpClasses;

namespace ZoneReader
{
    /// <summary>
    /// Holds all lineups for a particular map.
    /// </summary>
    public class LineupCollection
    {
        /// <summary>
        /// Dictionary of Targets with key being TargetId
        /// </summary>
        public Dictionary<int, Target> Targets { get; set; }

        /// <summary>
        /// Dictionary of LineUps with key being TargetId
        /// </summary>
        public Dictionary<int, Lineup> Lineups { get; set; }

        public static LineupCollection GetEmptyCollection()
        {
            return new LineupCollection
            {
                Lineups = new Dictionary<int, Lineup>(),
                Targets = new Dictionary<int, Target>(),
            };
        }
    }
}
