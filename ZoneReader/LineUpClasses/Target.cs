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

        public static Target FromXML(XMLClasses.Target target)
        {
            var res = new Target
            {
                Id = target.Id,
                LineUpIds = target.Cat_ids.Cat_id,
                Name = target.Name,
                Purpose = target.Purpose,
                Boundaries = Boundaries.FromXML(target.RectangularBoundaries)
            };

            return res;
        }
    }
}