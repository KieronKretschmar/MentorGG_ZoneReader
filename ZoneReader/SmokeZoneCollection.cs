using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZoneReader.Enums;
using ZoneReader.LineUpClasses;

namespace ZoneReader
{
    public class SmokeZoneCollection
    {
        public Dictionary<int, Target> IdTargets;
        public Dictionary<int, LineUp> IdLineUps;
        public ZoneMap Map;

        public SmokeZoneCollection(Map map)
        {
            this.Map = map.MapName;
            IdLineUps = map.LineUps.ToDictionary(key=>key.Id,value => value);
            IdTargets = map.Targets.ToDictionary(key => key.Id, value => value);
        }

        public static SmokeZoneCollection EmptyOnMap(ZoneMap map)
        {
           return new SmokeZoneCollection(new Map( new List<LineUp>(),new List<Target>(), map));
        }
    }
}
