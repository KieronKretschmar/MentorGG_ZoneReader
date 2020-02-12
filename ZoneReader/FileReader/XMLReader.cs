using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Xml.Serialization;
using ZoneReader.Enums;
using ZoneReader.Extensions;

namespace ZoneReader
{
    public class XMLReader
    {
        public LineUpClasses.Map Deserialize(string path)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Root));
            using (StreamReader reader = new StreamReader(path))
            {
                var read = (Root) ser.Deserialize(reader);

                return read.Map;
            }
        }



        #region XML Classes

        [XmlRoot(ElementName = "centroid")]
        internal class ExampleNade
        {
            [XmlElement(ElementName = "PlayerPosX")]
            public float PlayerPosX { get; set; }
            [XmlElement(ElementName = "PlayerPosY")]
            public float PlayerPosY { get; set; }
            [XmlElement(ElementName = "PlayerPosZ")]
            public float PlayerPosZ { get; set; }
            [XmlElement(ElementName = "PlayerViewX")]
            public float PlayerViewX { get; set; }
            [XmlElement(ElementName = "PlayerViewY")]
            public float PlayerViewY { get; set; }
            [XmlElement(ElementName = "GrenadePosX")]
            public float GrenadePosX { get; set; }
            [XmlElement(ElementName = "GrenadePosY")]
            public float GrenadePosY { get; set; }
            [XmlElement(ElementName = "GrenadePosZ")]
            public float GrenadePosZ { get; set; }

            public static implicit operator LineUpClasses.Grenade(ExampleNade exampleNade)
            {
                var res = new LineUpClasses.Grenade
                {
                    PlayerPos = new Vector3(exampleNade.PlayerPosX, exampleNade.PlayerPosY, exampleNade.PlayerPosZ),
                    DetonationPos = new Vector3(exampleNade.GrenadePosX, exampleNade.GrenadePosY, exampleNade.GrenadePosZ),
                    PlayerView = new Vector2(exampleNade.PlayerViewX, exampleNade.PlayerViewY),
                };

                return res;
            }
        }

        [XmlRoot(ElementName = "release_boundaries")]
        internal class ReleaseBoundaries
        {
            [XmlElement(ElementName = "PlayerPosX")]
            public XMLInterval PlayerPosX { get; set; }
            [XmlElement(ElementName = "PlayerPosY")]
            public XMLInterval PlayerPosY { get; set; }
            [XmlElement(ElementName = "PlayerPosZ")]
            public XMLInterval PlayerPosZ { get; set; }
            [XmlElement(ElementName = "PlayerViewX")]
            public XMLInterval PlayerViewX { get; set; }
            [XmlElement(ElementName = "PlayerViewY")]
            public XMLInterval PlayerViewY { get; set; }

            public static implicit operator LineUpClasses.ReleaseBoundaries(ReleaseBoundaries releaseBoundaries)
            {
                var res = new LineUpClasses.ReleaseBoundaries
                (
                    PolygonFactory.FromIntervals(releaseBoundaries.PlayerViewX, releaseBoundaries.PlayerViewY),
                    new LineUpClasses.Boundaries(PolygonFactory.FromIntervals(releaseBoundaries.PlayerPosX, releaseBoundaries.PlayerPosY),
                        new Interval(releaseBoundaries.PlayerPosZ.Lower, releaseBoundaries.PlayerPosZ.Upper))
                );

                return res;
            }
        }

        internal class XMLInterval
        {
            [XmlElement(ElementName = "lower")]
            public float Lower { get; set; }
            [XmlElement(ElementName = "upper")]
            public float Upper { get; set; }

            public List<float> ToList()
            {
                return new List<float> { Lower, Upper };
            }
            public static implicit operator Interval(XMLInterval interval) 
            {
                return new Interval(interval.Lower, interval.Upper);
            }
        }


        [XmlRoot(ElementName = "category")]
        internal class LineUp
        {
            [XmlElement(ElementName = "id")]
            public int Id { get; set; }
            [XmlElement(ElementName = "name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "target_id")]
            public int TargetId { get; set; }
            [XmlElement(ElementName = "viewx_contains_pole")]
            public string Viewx_contains_pole { get; set; }
            [XmlElement(ElementName = "throwtype")]
            public byte ThrowType { get; set; }
            [XmlElement(ElementName = "setpos")]
            public string SetposCommand { get; set; }
            [XmlElement(ElementName = "centroid")]
            public ExampleNade ExampleNade { get; set; }
            [XmlElement(ElementName = "release_boundaries")]
            public ReleaseBoundaries ReleaseBoundaries { get; set; }

            public static implicit operator LineUpClasses.LineUp(LineUp lineup)
            {
                var res = new LineUpClasses.LineUp
                {
                    ExampleNade = lineup.ExampleNade,
                    Id = lineup.Id,
                    TargetId = lineup.TargetId,
                    SetposCommand = lineup.SetposCommand,
                    Name = lineup.Name,
                    ViewXContainsPole = Convert.ToBoolean(lineup.Viewx_contains_pole.ToLower()),
                    releaseBoundaries = lineup.ReleaseBoundaries,
                    ThrowType = lineup.ThrowType

                };
                return res;
            }
        }

        [XmlRoot(ElementName = "categories")]
        internal class Categories
        {
            [XmlElement(ElementName = "category")]
            public List<LineUp> Category { get; set; }

            public static implicit operator List<LineUpClasses.LineUp>(Categories lineups)
            {
                var res = new List<LineUpClasses.LineUp>();
                foreach (var item in lineups.Category)
                {
                    res.Add(item);
                }
                return res;
            }
        }

        [XmlRoot(ElementName = "cat_ids")]
        internal class Cat_ids
        {
            [XmlElement(ElementName = "cat_id")]
            public List<int> Cat_id { get; set; }
        }

        [XmlRoot(ElementName = "rectangular_boundaries")]
        internal class RectangularBoundaries
        {
            [XmlElement(ElementName = "GrenadePosX")]
            public XMLInterval GrenadePosX { get; set; }
            [XmlElement(ElementName = "GrenadePosY")]
            public XMLInterval GrenadePosY { get; set; }
            [XmlElement(ElementName = "GrenadePosZ")]
            public XMLInterval GrenadePosZ { get; set; }

            public static implicit operator LineUpClasses.Boundaries(RectangularBoundaries bounds)
            {
                var res = new LineUpClasses.Boundaries(
                    PolygonFactory.FromIntervals(bounds.GrenadePosX, bounds.GrenadePosY),
                    new Interval(bounds.GrenadePosZ.Lower, bounds.GrenadePosZ.Upper));

                return res;
            }
        }

        [XmlRoot(ElementName = "target")]
        internal class Target
        {
            [XmlElement(ElementName = "id")]
            public int Id { get; set; }
            [XmlElement(ElementName = "name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "purpose")]
            public string Purpose { get; set; }
            [XmlElement(ElementName = "mapname")]
            public string Mapname { get; set; }

            [XmlElement(ElementName = "cat_ids")]
            public Cat_ids Cat_ids { get; set; }

            [XmlElement(ElementName = "rectangular_boundaries")]
            public RectangularBoundaries RectangularBoundaries { get; set; }

            public static implicit operator LineUpClasses.Target(Target target)
            {
                var res = new LineUpClasses.Target
                {
                    Id = target.Id,
                    LineUpIds = target.Cat_ids.Cat_id,
                    Name = target.Name,
                    Purpose = target.Purpose,
                    Boundaries = target.RectangularBoundaries
                };

                return res;
            }
        }

        [XmlRoot(ElementName = "targets")]
        internal class Targets
        {
            [XmlElement(ElementName = "target")]
            public List<Target> Target { get; set; }

            public static implicit operator List<LineUpClasses.Target>(Targets targets)
            {
                var res = new List<LineUpClasses.Target>();
                foreach (var item in targets.Target)
                {
                    res.Add(item);
                }
                return res;
            }

        }

        [XmlRoot(ElementName = "map")]
        internal class Map
        {
            [XmlElement(ElementName = "mapname")]
            public string Mapname { get; set; }
            [XmlElement(ElementName = "categories")]
            public Categories Categories { get; set; }
            [XmlElement(ElementName = "targets")]
            public Targets Targets { get; set; }

            public static implicit operator LineUpClasses.Map(Map map)
            {
                var mapName = Enum.Parse<ZoneMap>(map.Mapname, true);
                var res = new LineUpClasses.Map(map.Categories, map.Targets, mapName);
                return res;
            }
        }

        [XmlRoot(ElementName = "root")]
        internal class Root
        {
            [XmlElement(ElementName = "map")]
            public Map Map { get; set; }
        }
    }

}


#endregion