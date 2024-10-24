using RestoreMonarchy.PlayerStats.Components;
using RestoreMonarchy.PlayerStats.Helpers;
using RestoreMonarchy.PlayerStats.Models;
using SDG.Unturned;
using System;
using System.Collections.Generic;

namespace RestoreMonarchy.PlayerStats.APIs
{
    public static class PlayerStatsAPI
    {
        private static PlayerStatsPlugin pluginInstance => PlayerStatsPlugin.Instance;

        public static void GetPlayerRanking(ulong steamId, bool pvp, Action<PlayerRanking> callback)
        {
            if (pluginInstance == null)
            {
                throw new Exception("PlayerStats plugin is not loaded!");
            }

            ThreadHelper.RunAsynchronously(() =>
            {
                string orderBy = pvp ? "Kills" : "Zombies";
                PlayerRanking playerRanking = pluginInstance.Database.GetPlayerRanking(steamId, orderBy);

                ThreadHelper.RunSynchronously(() =>
                {
                    callback(playerRanking);
                });
            });
        }

        public static void GetPlayerRankings(int limit, bool pvp, Action<IEnumerable<PlayerRanking>> callback)
        {
            if (pluginInstance == null)
            {
                throw new Exception("PlayerStats plugin is not loaded!");
            }

            ThreadHelper.RunAsynchronously(() =>
            {
                string orderBy = pvp ? "Kills" : "Zombies";
                IEnumerable<PlayerRanking> playerRankings = pluginInstance.Database.GetPlayerRankings(limit, orderBy);

                ThreadHelper.RunSynchronously(() =>
                {
                    callback(playerRankings);
                });
            });
        }

        public static void GetPlayerStats(ulong steamId, Action<PlayerStatsData> callback)
        {
            if (pluginInstance == null)
            {
                throw new Exception("PlayerStats plugin is not loaded!");
            }

            ThreadHelper.RunAsynchronously(() =>
            {
                PlayerStatsData playerStats = pluginInstance.Database.GetPlayer(steamId);

                ThreadHelper.RunSynchronously(() =>
                {
                    callback(playerStats);
                });
            });
        }

        public static PlayerStatsData GetPlayerStats(Player player)
        {
            if (pluginInstance == null)
            {
                throw new Exception("PlayerStats plugin is not loaded!");
            }

            PlayerStatsComponent component = player.GetComponent<PlayerStatsComponent>();
            if (component == null)
            {
                throw new Exception("PlayerStats component is not attached to the player!");
            }

            return component.PlayerData;
        }
    }
}
