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
        public int Id;
        public string Name;

        public ZoneMap Map;
        public bool IsCt;

        public string VideoUrl;

        public string ParentZoneId;
        public int ZoneDepth; 

        public Interval ZInterval;

        public Polygon Geometry;

        public Boundaries GetBoundaries()
        {
            return new Boundaries(Geometry, this.ZInterval);
        }
        
        public static readonly Zone NullZoneReturningNegativeId = new Zone {  Id = -1, Geometry = null };


    }
}
