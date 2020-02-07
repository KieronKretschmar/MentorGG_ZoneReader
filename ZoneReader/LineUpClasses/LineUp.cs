using System;

namespace ZoneReader.LineUpClasses
{
    public class LineUp
    {
        public static LineUp NullLineUpNegativeIdAndTarget = new LineUp {Id = -1, TargetId = -1};

        public int Id { get; set; }
        public string Name { get; set; }
        public int TargetId { get; set; }
        public bool ViewXContainsPole { get; set; }
        public byte ThrowType { get; set; }
        public string SetposCommand { get; set; }
        public ExampleNade NadeExample { get; set; }
        public ReleaseBoundaries releaseBoundaries { get; set; }

        public static LineUp FromXML(XMLClasses.LineUp category)
        {
            var res = new LineUp
            {
                NadeExample = ExampleNade.FromXML(category.ExampleNade),
                Id = category.Id,
                TargetId = category.TargetId,
                SetposCommand = category.SetposCommand,
                Name = category.Name,
                ViewXContainsPole = Convert.ToBoolean(category.Viewx_contains_pole.ToLower()),
                releaseBoundaries = ReleaseBoundaries.FromXML(category.ReleaseBoundaries),
                ThrowType = category.ThrowType
            };
            return res;
        }
    }
}