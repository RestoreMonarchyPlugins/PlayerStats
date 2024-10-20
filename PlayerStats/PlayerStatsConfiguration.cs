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
        public float SaveIntervalSeconds { get; set; }
        public Reward[] Ranks { get; set; }

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            MessageIconUrl = "https://i.imgur.com/TWjBtCA.png";
            DatabaseProvider = "json";
            JsonFilePath = "{rocket_directory}/Plugins/PlayerStats/PlayerStats.json";
            MySQLConnectionString = "Server=127.0.0.1;Port=3306;Database=unturned;Uid=root;Pwd=passw;";
            SaveIntervalSeconds = 300;
            Ranks =
            [
                new Reward
                {
                    Name = "Airdrop",
                    Command = "/airdrop",
                    Kills = 5
                }
            ];
        }
    }
}
