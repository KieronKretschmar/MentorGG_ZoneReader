using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ZoneReader.XMLClasses;

namespace ZoneReader.Extensions
{
    public class PolygonFactory
    {
        public static Polygon FromFloats(List<float> coords)
        {
            if (coords.Count % 2 != 0)
                throw new ArgumentException("Must be an even number of coordinate values");

            List<Coordinate> coordinates = new List<Coordinate>();
            for (int i = 0; i < coords.Count; i += 2)
            {
                var coord = new Coordinate(coords[i], coords[i + 1]);
                coordinates.Add(coord);
            }
            return new Polygon(new LinearRing(coordinates.ToArray()));
        }


        public static Polygon FromPoints(List<Point> points)
        {
            return new Polygon(new LinearRing(points.Select(x => x.Coordinate).ToArray()));
        }


        public static Polygon FromIntervals(Interval first, Interval second)
        {
            var listOfCornerPoints = CartesianProductOfTwoLists(first.ToList(), second.ToList());

            //Swap last two points to make a convex rectangle
            var tmp = listOfCornerPoints[2];
            listOfCornerPoints[2] = listOfCornerPoints[3];
            listOfCornerPoints[3] = tmp;

            //Add the first point again to make a closed ring of points
            listOfCornerPoints.Add(listOfCornerPoints[0]);
            return FromPoints(listOfCornerPoints);
        }

        public static Polygon FromXMLIntervals(XMLInterval first, XMLInterval second)
        {
            return FromIntervals(first.ToInterval(),second.ToInterval());
        }

        private static List<Point> CartesianProductOfTwoLists(List<float> firstList, List<float> secondList)
        {
            return firstList.SelectMany(x => secondList, (x, y) => new Point(x, y)).ToList();
        }
    }
}
