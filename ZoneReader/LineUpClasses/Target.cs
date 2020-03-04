using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Numerics;

namespace ZoneReader.LineUpClasses
{
    public class Target
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
        public List<int> LineUpIds { get; set; }
        public Boundaries Boundaries { get; set; }

        /// <summary>
        /// Getter for Center vector. Used for displaying targets in webapp.
        /// </summary>
        public Vector2 Center { get
            {
                return new Vector2((float)Boundaries.GroundPolygon.Centroid.Coordinate.X, (float)Boundaries.GroundPolygon.Centroid.Coordinate.Y);
            }
        } 

        public static Target NullTargetWithNegativeId => new Target {Id = -1};
    }
}