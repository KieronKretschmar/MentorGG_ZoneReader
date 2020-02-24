using ZoneReader.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZoneReader
{
    public interface IZoneReader
    {
        MapZoneCollection GetZones(MapZoneType type, Map map);
        SmokeZoneCollection GetSmokeZones(Map map);

    }
}
