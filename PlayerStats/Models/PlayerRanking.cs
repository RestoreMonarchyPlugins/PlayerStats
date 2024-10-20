namespace RestoreMonarchy.PlayerStats.Models
{
    public class PlayerRanking
    {
        public int Rank { get; set; }
        public ulong SteamId { get; set; }
        public string Name { get; set; }
        public int Kills { get; set; }
        public int Zombies { get; set; }

        public bool IsUnranked()
        {
            if (PlayerStatsPlugin.Instance.Configuration.Instance.PVPRanking)
            {
                return Kills < PlayerStatsPlugin.Instance.Configuration.Instance.MinimumRankingTreshold;
            }
            else
            {
                return Zombies < PlayerStatsPlugin.Instance.Configuration.Instance.MinimumRankingTreshold;
            }
        }
    }
}
