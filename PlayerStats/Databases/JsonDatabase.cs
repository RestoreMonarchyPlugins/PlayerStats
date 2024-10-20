using Newtonsoft.Json;
using RestoreMonarchy.PlayerStats.Models;
using Rocket.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.PlayerStats.Databases
{
    internal class JsonDatabase : IDatabase
    {
        private PlayerStatsPlugin pluginInstance => PlayerStatsPlugin.Instance;
        private PlayerStatsConfiguration configuration => pluginInstance.Configuration.Instance;

        private List<PlayerData> playersData = new();

        public PlayerRanking GetPlayerRankingByKills(ulong steamId)
        {
            PlayerData player = GetPlayer(steamId);

            PlayerRanking ranking = new();
            ranking.Rank = playersData.OrderByDescending(x => x.Kills).ToList().FindIndex(x => x.SteamId == steamId) + 1;
            ranking.SteamId = steamId;
            ranking.Kills = player.Kills;
            ranking.Deaths = player.PVPDeaths;
            ranking.Headshots = player.Headshots;

            return ranking;
        }

        public void AddOrUpdatePlayer(PlayerData player)
        {
            player.LastUpdated = DateTime.UtcNow;
            if (!playersData.Contains(player))
            {
                playersData.Add(player);
            }
        }

        public PlayerData GetPlayer(ulong steamId, string name = null)
        {
            PlayerData player = playersData.FirstOrDefault(x => x.SteamId == steamId);
            if (player == null)
            {
                player = new PlayerData
                {
                    SteamId = steamId,
                    Name = name
                };
                playersData.Add(player);
            }

            return player;
        }

        public void Initialize()
        {
            Reload();
        }

        public void Reload()
        {
            string path = configuration.JsonFilePath.Replace("{rocket_directory}", Directory.GetCurrentDirectory());

            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);
                playersData = JsonConvert.DeserializeObject<List<PlayerData>>(text);
            }
            else
            {
                playersData = new List<PlayerData>();
                string text = JsonConvert.SerializeObject(playersData, Formatting.Indented);
                File.WriteAllText(path, text);
            }
        }

        public void Save(IEnumerable<PlayerData> playersData)
        {
            string path = configuration.JsonFilePath.Replace("{rocket_directory}", Directory.GetCurrentDirectory());
            string text = JsonConvert.SerializeObject(playersData, Formatting.Indented);
            File.WriteAllText(path, text);
        }
    }
}
