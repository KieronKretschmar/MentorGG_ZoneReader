using ZoneReader.Extensions;
using NetTopologySuite.Geometries;

namespace ZoneReader.LineUpClasses
{
    public class ReleaseBoundaries
    {
        public Boundaries PositionBoundaries { get; }
        public Polygon ViewBoundaries { get; }

        public static ReleaseBoundaries FromXML(XMLClasses.ReleaseBoundaries releaseBoundaries)
        {
            var res = new ReleaseBoundaries
            (
                PolygonFactory.FromXMLIntervals(releaseBoundaries.PlayerViewX, releaseBoundaries.PlayerViewY),
                new Boundaries(PolygonFactory.FromXMLIntervals(releaseBoundaries.PlayerPosX, releaseBoundaries.PlayerPosY),
                    new Interval(releaseBoundaries.PlayerPosZ.Lower, releaseBoundaries.PlayerPosZ.Upper))
            );

            return res;
        }



        public ReleaseBoundaries(Polygon viewBoundaries, Boundaries positionBoundaries)
        {
            ViewBoundaries = viewBoundaries;
            PositionBoundaries = positionBoundaries;
        }
    }
}