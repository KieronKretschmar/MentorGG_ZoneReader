using System;
using System.Collections.Generic;
using ZoneReader.XMLClasses;
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


        public static Map FromXML(XMLClasses.Map map)
        {
            var lineups = new List<LineUp>();
            foreach (var category in map.Categories.Category) lineups.Add(LineUp.FromXML(category));

            var targets = new List<Target>();
            foreach (var target in map.Targets.Target) targets.Add(Target.FromXML(target));

            var res = new Map(lineups, targets, Enum.Parse<ZoneMap>(map.Mapname, true));

            return res;
        }
    }
}