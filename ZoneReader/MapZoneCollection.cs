using System.Collections;
using System.Collections.Generic;

namespace ZoneReader
{
    public class MapZoneCollection : IEnumerable<KeyValuePair<bool, List<Zone>>>
    {
        public Dictionary<bool, List<Zone>> TeamZone { get; }

        public MapZoneCollection()
        {
            TeamZone = new Dictionary<bool, List<Zone>>();
        }

        public void SetTeamZones(bool team, List<Zone> zones)
        {
            TeamZone[team] = zones;
        }

        public List<Zone> this[bool team] => TeamZone[team];

        public IEnumerator<KeyValuePair<bool, List<Zone>>> GetEnumerator()
        {
            return TeamZone.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}