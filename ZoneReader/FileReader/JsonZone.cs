using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZoneReader.Extensions;
using ZoneReader.LineUpClasses;
using NetTopologySuite.Geometries;
using ZoneReader.Enums;

namespace ZoneReader
{
    public class JsonZone
    {
        public Properties properties { get; set; }
        public JsonGeometry geometry { get; set; }



        public static implicit operator Zone(JsonZone jZone)
        {
            var props = jZone.properties;

            return new Zone
            {
                Geometry = jZone.geometry.Polygon,
                Id = props.Id,
                Map = Enum.Parse<ZoneMap>(jZone.properties.Map.ToLower(), true),
                Name = props.Name,
                ParentZoneId = int.Parse(props.ParentZoneId),
                IsCt =  props.Team == 3,
                VideoUrl = props.VideoUrl,
                ZInterval = props.ZInterval,
                ZoneDepth = props.ZoneDepth
            };
        }
    }


    public class Properties
    {
        public string Map { get; set; }
        public int Team { get; set; }
        public string VideoUrl { get; set; }
        public int ZoneDepth { get; set; }
        public string ParentZoneId { get; set; }
        public int Id { get; set; }
        public float? ZMin { get; set; }
        public float? ZMax { get; set; }
        public string Name { get; set; }


        public Interval ZInterval
        {
            get
            {
                return _zInterval ??= new Interval(ZMin, ZMax);
            }
        }

        private Interval _zInterval;
    }

    public class JsonGeometry
    {
        public List<List<List<float>>> coordinates { get; set; }

        private Polygon _polygon;

        public Polygon Polygon
        {
            get
            {
                if (_polygon == null)
                    _polygon = PolygonFactory.FromFloats(coordinates.First());

                return _polygon;
            }
        }
    }

    public class JsonZoneCollection
    {
        public List<JsonZone> features { get; set; }

        public static implicit operator List<Zone>(JsonZoneCollection collection)
        {
            var res = new List<Zone>();
            foreach (var item in collection.features)
            {
                res.Add(item);
            }
            return res;
        }
    }
}
