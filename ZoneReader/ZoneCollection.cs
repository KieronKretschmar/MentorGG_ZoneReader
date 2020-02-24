using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZoneReader
{
    /// <summary>
    /// Collection of zones for a particular map.
    /// </summary>
    public class ZoneCollection : IEnumerable<KeyValuePair<bool, List<Zone>>>
    {
        public Dictionary<bool, List<Zone>> TeamZone { get; }

        public ZoneCollection()
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

        public List<Zone> Values()
        {
            return TeamZone.Values.SelectMany(x=> x).ToList();
        }

    }
}