using RestoreMonarchy.PlayerStats.Models;
using Rocket.API;

namespace RestoreMonarchy.PlayerStats
{
    public class PlayerStatsConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }
        public string MessageIconUrl { get; set; }

        // Database Settings
        public string DatabaseProvider { get; set; }
        public string JsonFilePath { get; set; }
        public string MySQLConnectionString { get; set; }
        public string PlayerStatsTableName { get; set; }
        public float SaveIntervalSeconds { get; set; }

        // UI Settings
        public bool EnableUIEffect { get; set; }
        public ushort UIEffectId { get; set; }
        public bool ShowUIEffectByDefault { get; set; }

        // Feature Toggles
        public bool EnableJoinLeaveMessages { get; set; }

        // Legacy Stats Settings
        public bool EnablePVPStats { get; set; }
        public bool EnablePVEStats { get; set; }
        public bool PVPRanking { get; set; }
        public bool PVPRewards { get; set; }
        public bool PVPUI { get; set; }

        // New Stats Settings
        public StatsMode? StatsMode { get; set; }

        // Ranking & Rewards
        public int MinimumRankingTreshold { get; set; }
        public bool EnableRewards { get; set; }
        public Reward[] Rewards { get; set; }

        // Only hide legacy properties when using new StatsMode
        public bool ShouldSerializeEnablePVPStats() => !StatsMode.HasValue;
        public bool ShouldSerializeEnablePVEStats() => !StatsMode.HasValue;
        public bool ShouldSerializePVPRanking() => !StatsMode.HasValue;
        public bool ShouldSerializePVPRewards() => !StatsMode.HasValue;
        public bool ShouldSerializePVPUI() => !StatsMode.HasValue;

        public StatsMode ActualStatsMode
        {
            get
            {
                if (StatsMode.HasValue)
                    return StatsMode.Value;

                if (!EnablePVPStats && !EnablePVEStats)
                    return Models.StatsMode.Both;
                if (EnablePVPStats && !EnablePVEStats)
                    return Models.StatsMode.PVP;
                if (!EnablePVPStats && EnablePVEStats)
                    return Models.StatsMode.PVE;

                return Models.StatsMode.Both;
            }
        }

        public bool IsRankingEnabled => StatsMode.HasValue ?
            (ActualStatsMode == Models.StatsMode.PVP || ActualStatsMode == Models.StatsMode.Both) && EnableRewards
            : PVPRanking && PVPRewards;

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            MessageIconUrl = "https://i.imgur.com/TWjBtCA.png";

            DatabaseProvider = "json";
            JsonFilePath = "{rocket_directory}/Plugins/PlayerStats/PlayerStats.json";
            MySQLConnectionString = "Server=127.0.0.1;Port=3306;Database=unturned;Uid=root;Pwd=passw;";
            PlayerStatsTableName = "PlayerStats";
            SaveIntervalSeconds = 300;

            EnableUIEffect = false;
            UIEffectId = 22512;
            ShowUIEffectByDefault = true;

            EnableJoinLeaveMessages = true;

            // Legacy defaults
            EnablePVPStats = true;
            EnablePVEStats = true;
            PVPRanking = true;
            PVPRewards = true;
            PVPUI = true;

            // New system defaults to null to use legacy system
            StatsMode = null;

            MinimumRankingTreshold = 25;
            EnableRewards = true;
            Rewards = [
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