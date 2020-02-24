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

        public LineupCollection(LineUpClasses.Map map)
        {
            this.Map = map.MapName;
            IdLineUps = map.LineUps.ToDictionary(key=>key.Id,value => value);
            IdTargets = map.Targets.ToDictionary(key => key.Id, value => value);
        }

        public static LineupCollection EmptyOnMap(Enums.Map map)
        {
           return new LineupCollection(new LineUpClasses.Map( new List<LineUp>(),new List<Target>(), map));
        }
    }
}
