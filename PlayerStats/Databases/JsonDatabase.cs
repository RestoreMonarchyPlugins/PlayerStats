using Newtonsoft.Json;
using RestoreMonarchy.PlayerStats.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RestoreMonarchy.PlayerStats.Databases
{
    internal class JsonDatabase : IDatabase
    {
        private PlayerStatsPlugin pluginInstance => PlayerStatsPlugin.Instance;
        private PlayerStatsConfiguration configuration => pluginInstance.Configuration.Instance;

        private List<PlayerData> playersData = new();

        public IEnumerable<PlayerRanking> GetPlayerRankings(int amount)
        {
            List<PlayerRanking> rankings = new();
            if (configuration.PVPRanking)
            {
                rankings = playersData.OrderByDescending(x => x.Kills).Take(amount).Select((x, i) => new PlayerRanking
                {
                    SteamId = x.SteamId,
                    Name = x.Name,
                    Kills = x.Kills,
                    Rank = i + 1
                }).ToList();
            }
            else
            {
                rankings = playersData.OrderByDescending(x => x.Zombies).Take(amount).Select((x, i) => new PlayerRanking
                {
                    SteamId = x.SteamId,
                    Name = x.Name,
                    Zombies = x.Zombies,
                    Rank = i + 1
                }).ToList();
            }

            return rankings;
        }

        public PlayerRanking GetPlayerRanking(ulong steamId)
        {
            PlayerData player = GetPlayer(steamId);
            if (player == null)
            {
                return null;
            }

            PlayerRanking ranking = new();
            ranking.SteamId = player.SteamId;
            ranking.Name = player.Name;
            ranking.Kills = player.Kills;
            ranking.Zombies = player.Zombies;
            if (configuration.PVPRanking)
            {
                ranking.Rank = playersData.Count(x => x.Kills > player.Kills) + 1;
                
            } else
            {
                ranking.Rank = playersData.Count(x => x.Zombies > player.Zombies) + 1;
            }

            return ranking;
        }

        public void AddOrUpdatePlayer(PlayerData player)
        {
            player.LastUpdated = DateTime.UtcNow;
        }

        public PlayerData GetOrAddPlayer(ulong steamId, string name)
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

        public PlayerData GetPlayer(ulong steamId)
        {
            PlayerData player = playersData.FirstOrDefault(x => x.SteamId == steamId);

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
            foreach (PlayerData player in playersData)
            {
                player.LastUpdated = DateTime.UtcNow;
            }

            string path = configuration.JsonFilePath.Replace("{rocket_directory}", Directory.GetCurrentDirectory());
            string text = JsonConvert.SerializeObject(this.playersData, Formatting.Indented);
            File.WriteAllText(path, text);
        }
    }
}
