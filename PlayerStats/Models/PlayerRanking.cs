namespace RestoreMonarchy.PlayerStats.Models
{
    public class PlayerRanking
    {
        public int Rank { get; set; }
        public int PVERank { get; set; }
        public ulong SteamId { get; set; }
        public string Name { get; set; }
        public int Kills { get; set; }
        public int Zombies { get; set; }

        public bool IsUnranked()
        {
            PlayerStatsConfiguration configuration = PlayerStatsPlugin.Instance.Configuration.Instance;

            if (configuration.StatsMode == StatsMode.Both || configuration.StatsMode == StatsMode.PVP)
            {
                return Kills < configuration.MinimumRankingTreshold;
            }
            else
            {
                return Zombies < configuration.MinimumRankingTreshold;
            }
        }
    }
}
