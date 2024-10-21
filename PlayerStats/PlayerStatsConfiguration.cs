using RestoreMonarchy.PlayerStats.Models;
using Rocket.API;

namespace RestoreMonarchy.PlayerStats
{
    public class PlayerStatsConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }
        public string MessageIconUrl { get; set; }
        public string DatabaseProvider { get; set; }
        public string JsonFilePath { get; set; }
        public string MySQLConnectionString { get; set; }
        public string PlayerStatsTableName { get; set; }
        public float SaveIntervalSeconds { get; set; }
        public bool EnableUIEffect { get; set; }
        public ushort UIEffectId { get; set; }
        public bool EnablePVPStats { get; set; }
        public bool EnablePVEStats { get; set; }
        public bool PVPRanking { get; set; }
        public bool PVPRewards { get; set; }
        public int MinimumRankingTreshold { get; set; }
        public bool EnableRewards { get; set; }
        public Reward[] Rewards { get; set; }

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            MessageIconUrl = "https://i.imgur.com/TWjBtCA.png";
            DatabaseProvider = "json";
            JsonFilePath = "{rocket_directory}/Plugins/PlayerStats/PlayerStats.json";
            MySQLConnectionString = "Server=127.0.0.1;Port=3306;Database=unturned;Uid=root;Pwd=passw;";
            PlayerStatsTableName = "PlayerStats";
            SaveIntervalSeconds = 300;
            EnableUIEffect = true;
            UIEffectId = 22512;
            EnablePVPStats = true;
            EnablePVEStats = true;
            PVPRanking = true;
            PVPRewards = true;
            MinimumRankingTreshold = 25;
            EnableRewards = true;
            Rewards =
            [
                new Reward
                {
                    Name = "VIP Rank",
                    PermissionGroup = "vip",
                    Treshold = 50
                },
                new Reward
                {
                    Name = "MVP Rank",
                    PermissionGroup = "mvp",
                    Treshold = 125
                }
            ];
        }
    }
}
