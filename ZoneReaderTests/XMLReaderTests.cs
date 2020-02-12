using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using ZoneReader;
using ZoneReader.Enums;

namespace ZoneReaderTests
{
    [TestClass]
    public class XMLReaderTests
    {

        [TestMethod]
        public void XMLReaderDeserializesProperly()
        {
            string path = Path.Join(TestHelper.GetZoneResourcesDirectory(), @"\smokes\de_dust2.xml");
            var reader = new XMLReader();
            var map = reader.Deserialize(path);

            Assert.IsTrue(map.LineUps.Any());
            Assert.IsTrue(map.Targets.Any());
            Assert.AreEqual(ZoneMap.De_Dust2, map.MapName);
            foreach (var lineup in map.LineUps)
            {
                Assert.IsTrue(lineup.TargetId > 0);
                Assert.IsNotNull(lineup.releaseBoundaries);
                Assert.IsNotNull(lineup.ExampleNade);

                Assert.IsNotNull(lineup.releaseBoundaries.PositionBoundaries);
                Assert.IsNotNull(lineup.releaseBoundaries.ViewBoundaries);
            }

            foreach (var target in map.Targets)
            {
                Assert.IsNotNull(target);
                Assert.IsNotNull(target.Boundaries);
                Assert.IsNotNull(target.Boundaries.GroundPolygon);
                Assert.IsNotNull(target.Boundaries.ZInterval);
            }
        }
    }
}
