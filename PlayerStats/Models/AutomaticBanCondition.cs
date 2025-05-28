using System.Xml.Serialization;

namespace RestoreMonarchy.PlayerStats.Models
{
    public class AutomaticBanCondition
    {
        [XmlAttribute]
        public string Comparer { get; set; }
        [XmlAttribute] 
        public string Stat { get; set; }
        [XmlAttribute]
        public long Value { get; set; }
    }
}
