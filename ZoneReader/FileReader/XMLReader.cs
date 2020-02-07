using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ZoneReader.XMLClasses;

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

                return LineUpClasses.Map.FromXML(read.Map);
            }
        }
    }
}

#region XML Classes
namespace ZoneReader.XMLClasses
{
    [XmlRoot(ElementName = "centroid")]
    public class ExampleNade
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
    }

    [XmlRoot(ElementName = "release_boundaries")]
    public class ReleaseBoundaries
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
    }

    public class XMLInterval
    {
        [XmlElement(ElementName = "lower")]
        public float Lower { get; set; }
        [XmlElement(ElementName = "upper")]
        public float Upper { get; set; }

        public List<float> ToList()
        {
            return new List<float> { Lower, Upper };
        }
        public Interval ToInterval()
        {
            return new Interval(Lower, Upper);
        }
    }


    [XmlRoot(ElementName = "category")]
    public class LineUp
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
    }

    [XmlRoot(ElementName = "categories")]
    public class Categories
    {
        [XmlElement(ElementName = "category")]
        public List<LineUp> Category { get; set; }
    }

    [XmlRoot(ElementName = "cat_ids")]
    public class Cat_ids
    {
        [XmlElement(ElementName = "cat_id")]
        public List<int> Cat_id { get; set; }
    }

    [XmlRoot(ElementName = "rectangular_boundaries")]
    public class RectangularBoundaries
    {
        [XmlElement(ElementName = "GrenadePosX")]
        public XMLInterval GrenadePosX { get; set; }
        [XmlElement(ElementName = "GrenadePosY")]
        public XMLInterval GrenadePosY { get; set; }
        [XmlElement(ElementName = "GrenadePosZ")]
        public XMLInterval GrenadePosZ { get; set; }
    }

    [XmlRoot(ElementName = "target")]
    public class Target
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
    }

    [XmlRoot(ElementName = "targets")]
    public class Targets
    {
        [XmlElement(ElementName = "target")]
        public List<Target> Target { get; set; }
    }

    [XmlRoot(ElementName = "map")]
    public class Map
    {
        [XmlElement(ElementName = "mapname")]
        public string Mapname { get; set; }
        [XmlElement(ElementName = "categories")]
        public Categories Categories { get; set; }
        [XmlElement(ElementName = "targets")]
        public Targets Targets { get; set; }
    }

    [XmlRoot(ElementName = "root")]
    public class Root
    {
        [XmlElement(ElementName = "map")]
        public Map Map { get; set; }
    }

}


#endregion