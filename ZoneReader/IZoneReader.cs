using ZoneReader.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZoneReader
{
    public interface IZoneReader
    {
        ZoneCollection GetZones(ZoneType type, Map map);
        LineupCollection GetLineups(LineupType lineupType, Map map);

    }
}
