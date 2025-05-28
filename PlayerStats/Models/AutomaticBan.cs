using System.Xml.Serialization;

namespace RestoreMonarchy.PlayerStats.Models
{
    public class AutomaticBan
    {
        [XmlAttribute]
        public string Reason { get; set; }
        [XmlArrayItem("Condition")]
        public AutomaticBanCondition[] Conditions { get; set; }

        public bool CheckConditions(PlayerStatsData data)
        {
            if (Conditions == null || Conditions.Length == 0)
            {
                return false;
            }

            foreach (AutomaticBanCondition condition in Conditions)
            {
                long statValue = data.GetStatValue(condition.Stat);
                switch (condition.Comparer.ToLower())
                {
                    case "greater":
                        if (statValue <= condition.Value) return false;
                        break;
                    case "less":
                        if (statValue >= condition.Value) return false;
                        break;
                    case "equal":
                        if (statValue != condition.Value) return false;
                        break;
                    default:
                        return false;
                }
            }

            return true;
        }
    }
}
