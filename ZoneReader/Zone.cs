using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using ZoneReader.Enums;
using ZoneReader.LineUpClasses;

namespace ZoneReader
{
    public class Zone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ZoneMap Map { get; set; }
        public bool IsCt { get; set; }
        public string VideoUrl { get; set; }
        public string ParentZoneId { get; set; }
        public int ZoneDepth { get; set; }
        public Interval ZInterval { get; set; }
        public Polygon Geometry { get; set; }

        public Boundaries GetBoundaries()
        {
            return new Boundaries(Geometry, this.ZInterval);
        }
        
        public static readonly Zone NullZoneReturningNegativeId = new Zone {  Id = -1, Geometry = null };
    }
}
