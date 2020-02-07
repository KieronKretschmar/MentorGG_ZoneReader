using System.Numerics;

namespace ZoneReader.LineUpClasses
{
    public class ExampleNade
    {
        public Vector3 PlayerPos { get; private set; }
        public Vector3 DetonationPos { get; private set; }
        public Vector2 PlayerView { get; private set; }

        public static ExampleNade FromXML(XMLClasses.ExampleNade exampleNade)
        {
            var res = new ExampleNade
            {
                PlayerPos = new Vector3(exampleNade.PlayerPosX, exampleNade.PlayerPosY, exampleNade.PlayerPosZ),
                DetonationPos = new Vector3(exampleNade.GrenadePosX, exampleNade.GrenadePosY, exampleNade.GrenadePosZ),
                PlayerView = new Vector2(exampleNade.PlayerViewX, exampleNade.PlayerViewY),
            };

            return res;
        }
    }
}