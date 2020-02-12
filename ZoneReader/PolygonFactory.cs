using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ZoneReader.Extensions
{
    public class PolygonFactory
    {
        public static Polygon FromFloats(List<List<float>> coords)
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            foreach(var coord in coords)
            {
                if (coord.Count != 2)
                    throw new ArgumentException("list of floats must have exactly two values per coordinate");

                var coordinate = new Coordinate(coord[0], coord[1]);
                coordinates.Add(coordinate);
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

        private static List<Point> CartesianProductOfTwoLists(List<float> firstList, List<float> secondList)
        {
            return firstList.SelectMany(x => secondList, (x, y) => new Point(x, y)).ToList();
        }
    }
}
