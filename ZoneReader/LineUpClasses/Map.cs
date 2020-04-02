using System.Collections.Generic;
using System.Linq;
using ZoneReader.Enums;

namespace ZoneReader.LineUpClasses
{
    public class Map
    {
        public Map(List<Lineup> lineups, List<Target> targets, Enums.Map mapName)
        {
            LineUps = lineups;
            Targets = targets;
            MapName = mapName;
        }

        public Enums.Map MapName { get; }
        public List<Lineup> LineUps { get; }
        public List<Target> Targets { get; }

        public LineupCollection ToLineupCollection()
        {
            return new LineupCollection
            {
                Lineups = LineUps.ToDictionary(key => key.LineupId, value => value),
                Targets = Targets.ToDictionary(key => key.TargetId, value => value),
            };
        }
    }
}