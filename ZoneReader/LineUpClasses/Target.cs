using System.Collections.Generic;

namespace ZoneReader.LineUpClasses
{
    public class Target
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
        public List<int> LineUpIds { get; set; }
        public Boundaries Boundaries { get; set; }
        public static Target NullTargetWithNegativeId => new Target {Id = -1};
    }
}