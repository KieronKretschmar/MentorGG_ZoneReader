using System.Collections.Generic;
using ZoneReader.Enums;

namespace ZoneReader.LineUpClasses
{
    public class Map
    {
        public Map(List<LineUp> lineUps, List<Target> targets, ZoneMap mapName)
        {
            LineUps = lineUps;
            Targets = targets;
            MapName = mapName;
        }

        public ZoneMap MapName { get; }
        public List<LineUp> LineUps { get; }
        public List<Target> Targets { get; }
    }
}