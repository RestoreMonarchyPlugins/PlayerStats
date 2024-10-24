using RestoreMonarchy.PlayerStats.APIs;
using RestoreMonarchy.PlayerStats.Models;
using Rocket.API;
using Rocket.Core.Commands;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using SDG.Unturned;
using Steamworks;
using System.Text;

namespace RestoreMonarchy.PlayerStatsTester
{
    public class PlayerStatsTesterPlugin : RocketPlugin<PlayerStatsTesterConfiguration>
    {
        protected override void Load()
        {
            Logger.Log($"{Name} {Assembly.GetName().Version.ToString(3)} has been loaded!");
        }

        protected override void Unload()
        {
            Logger.Log($"{Name} has been unloaded!");
        }

        [RocketCommand("playerstats-test", "Test PlayerStats API", "<steamid>", AllowedCaller.Both)]
        public void TestPlayerStatsCommand(IRocketPlayer caller, string[] parameters)
        {
            if (parameters.Length == 0)
            {
                Logger.LogError("Usage: /playerstats-test <steamid>");
                return;
            }

            if (!ulong.TryParse(parameters[0], out ulong steamId))
            {
                Logger.LogError("Invalid SteamId!");
                return;
            }

            Logger.Log("Testing PlayerStats API...");

            PlayerStatsAPI.GetPlayerRankings(10, true, (rankings) =>
            {
                StringBuilder sb = new();
                sb.AppendLine("PVP Ranking");
                foreach (var ranking in rankings)
                {
                    sb.AppendLine($"#{ranking.Rank} | SteamId: {ranking.SteamId}, Name: {ranking.Name}, Kills: {ranking.Kills}");
                }
                Logger.Log(sb.ToString());
            });

            PlayerStatsAPI.GetPlayerRankings(10, false, (rankings) =>
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("PVE Ranking");
                foreach (var ranking in rankings)
                {
                    sb.AppendLine($"#{ranking.Rank} | SteamId: {ranking.SteamId}, Name: {ranking.Name}, Zombies: {ranking.Zombies}");
                }

                Logger.Log(sb.ToString());
            });

            PlayerStatsAPI.GetPlayerRanking(steamId, true, (ranking) =>
            {
                if (ranking == null)
                {
                    Logger.Log("Player not found when getting player ranking!");
                    return;
                }

                Logger.Log($"GetPlayerRanking(steamId, true): #{ranking.Rank} | PVP Ranking | SteamId: {ranking.SteamId}, Name: {ranking.Name}, Kills: {ranking.Kills}");
            });

            PlayerStatsAPI.GetPlayerRanking(steamId, false, (ranking) =>
            {
                if (ranking == null)
                {
                    Logger.Log("Player not found when getting player ranking!");
                    return;
                }

                Logger.Log($"GetPlayerRanking(steamId, false): #{ranking.Rank} | PVE Ranking | SteamId: {ranking.SteamId}, Name: {ranking.Name}, Zombies: {ranking.Zombies}");
            });

            PlayerStatsAPI.GetPlayerStats(steamId, (playerStats) =>
            {
                if (playerStats == null)
                {
                    Logger.Log("Player not found when getting player stats!");
                    return;
                }

                Logger.Log($"GetPlayerStats(steamId): SteamId: {playerStats.SteamId}, Name: {playerStats.Name}, Kills: {playerStats.Kills}, Zombies: {playerStats.Zombies}, PVP Deaths: {playerStats.PVPDeaths}");
            });

            Player player = PlayerTool.getPlayer(new CSteamID(steamId));
            if (player != null)
            {
                PlayerStatsData stats = PlayerStatsAPI.GetPlayerStats(player);
                if (stats != null)
                {
                    Logger.Log($"GetPlayerStats(player): SteamId: {stats.SteamId}, Name: {stats.Name}, Kills: {stats.Kills}, Zombies: {stats.Zombies}, PVP Deaths: {stats.PVPDeaths}");
                } else
                {
                    Logger.Log("GetPlayerStats(player): Player not found when getting player stats!");
                }
            } else
            {
                Logger.Log("Player not found on the server when getting player stats!");
            }
        }
    }
}