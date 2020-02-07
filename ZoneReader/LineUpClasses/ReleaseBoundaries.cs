using ZoneReader.Extensions;
using NetTopologySuite.Geometries;

namespace ZoneReader.LineUpClasses
{
    public class ReleaseBoundaries
    {
        public Boundaries PositionBoundaries { get; }
        public Polygon ViewBoundaries { get; }

        public ReleaseBoundaries(Polygon viewBoundaries, Boundaries positionBoundaries)
        {
            ViewBoundaries = viewBoundaries;
            PositionBoundaries = positionBoundaries;
        }
    }
}