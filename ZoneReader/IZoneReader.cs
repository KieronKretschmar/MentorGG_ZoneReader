using ZoneReader.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZoneReader
{
    public interface IZoneReader
    {
        ZoneCollection GetZones(ZoneType type, Map map);
        ZoneCollection GetZones(ZoneType type);
        LineupCollection GetLineups(LineupType lineupType, Map map);

    }
}
