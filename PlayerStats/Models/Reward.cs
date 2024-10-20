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
        public string Command { get; set; }
        public bool ShouldSerializeCommand() => !string.IsNullOrEmpty(Command);
        [XmlAttribute]
        public string Permission { get; set; }
        public bool ShouldSerializePermission() => !string.IsNullOrEmpty(Permission);

        [XmlArrayItem("Command")]
        public string[] Commands { get; set; }
        public bool ShouldSerializeCommands() => Commands != null && Commands.Length > 0;
    }
}
