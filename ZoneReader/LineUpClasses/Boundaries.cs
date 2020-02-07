using ZoneReader.Extensions;
using NetTopologySuite.Geometries;

namespace ZoneReader.LineUpClasses
{
    public class Boundaries
    {
        public Boundaries(Polygon groundPolygon, Interval zInterval)
        {
            ZInterval = zInterval;
            GroundPolygon = groundPolygon;
        }

        public Polygon GroundPolygon { get; }
        public Interval ZInterval { get; }
    }
}