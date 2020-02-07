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

        public static Boundaries FromXML(XMLClasses.RectangularBoundaries rectangularBoundaries)
        {
            var res = new Boundaries(
                PolygonFactory.FromXMLIntervals(rectangularBoundaries.GrenadePosX, rectangularBoundaries.GrenadePosY),
                new Interval(rectangularBoundaries.GrenadePosZ.Lower, rectangularBoundaries.GrenadePosZ.Upper));

            return res;
        }
    }
}