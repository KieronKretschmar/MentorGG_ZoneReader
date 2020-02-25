using System;

namespace ZoneReader.LineUpClasses
{
    public class Lineup
    {
        /// <summary>
        /// A null lineup for which target  and lineup id are negative
        /// </summary>
        public static Lineup NullLineUpNegativeIdAndTarget = new Lineup {Id = -1, TargetId = -1};

        public int Id { get; set; }
        public string Name { get; set; }
        public int TargetId { get; set; }
        public bool ViewXContainsPole { get; set; }
        public byte ThrowType { get; set; }
        public string SetposCommand { get; set; }
        public Grenade ExampleNade { get; set; }
        public ReleaseBoundaries releaseBoundaries { get; set; }
    }
}