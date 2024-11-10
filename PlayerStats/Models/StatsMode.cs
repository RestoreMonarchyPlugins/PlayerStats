using System.Xml.Serialization;

namespace RestoreMonarchy.PlayerStats.Models
{
    public enum StatsMode
    {
        [XmlEnum("both")]
        Both,
        [XmlEnum("pvp")]
        PVP,
        [XmlEnum("pve")]
        PVE
    }
}
