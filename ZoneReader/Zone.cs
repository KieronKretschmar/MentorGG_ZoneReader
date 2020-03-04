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
        public int ZoneId { get; set; }
        public string Name { get; set; }
        public Enums.Map Map { get; set; }
        public string MapString => Map.ToString();
        public bool IsCt { get; set; }
        public string VideoUrl { get; set; }
        public int ParentZoneId { get; set; }
        public int ZoneDepth { get; set; }
        public Interval ZInterval { get; set; }
        public Polygon Geometry { get; set; }

        public Boundaries GetBoundaries()
        {
            return new Boundaries(Geometry, this.ZInterval);
        }
        
        /// <summary>
        /// A zone with negative id and default values everywhere else
        /// </summary>
        public static readonly Zone NullZoneReturningNegativeId = new Zone {  ZoneId = -1};
    }
}
