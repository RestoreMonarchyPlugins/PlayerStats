using System.Xml.Serialization;

namespace RestoreMonarchy.PlayerStats.Models
{
    public class Reward
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public int Treshold { get; set; }
        [XmlAttribute]
        public string PermissionGroup { get; set; }
    }
}
