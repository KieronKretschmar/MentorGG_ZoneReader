using System.Collections.Generic;
using System.Linq;
using ZoneReader.Enums;

namespace ZoneReader.LineUpClasses
{
    public class Map
    {
        public Map(List<LineUp> lineUps, List<Target> targets, Enums.Map mapName)
        {
            LineUps = lineUps;
            Targets = targets;
            MapName = mapName;
        }

        public Enums.Map MapName { get; }
        public List<LineUp> LineUps { get; }
        public List<Target> Targets { get; }

        public LineupCollection ToLineupCollection()
        {
            return new LineupCollection
            {
                Map = MapName,
                IdLineUps = LineUps.ToDictionary(key => key.Id, value => value),
                IdTargets = Targets.ToDictionary(key => key.Id, value => value),
            };
        }
    }
}