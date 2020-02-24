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
        public Dictionary<int, Target> IdTargets;
        public Dictionary<int, LineUp> IdLineUps;
        public Enums.Map Map;
        
        public static LineupCollection EmptyOnMap(Enums.Map map)
        {
            return new LineupCollection {
                Map = map,
                IdLineUps = new Dictionary<int, LineUp>(),
                IdTargets = new Dictionary<int, Target>(),
           };
        }
    }
}
