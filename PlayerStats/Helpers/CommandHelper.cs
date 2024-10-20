using RestoreMonarchy.PlayerStats.Components;
using RestoreMonarchy.PlayerStats.Models;
using Rocket.API;
using Rocket.Unturned.Player;
using System;

namespace RestoreMonarchy.PlayerStats.Helpers
{
    internal static class CommandHelper
    {
        private static PlayerStatsPlugin pluginInstance => PlayerStatsPlugin.Instance;

        internal static void GetPlayerData(IRocketPlayer caller, string[] command, Action<PlayerData> callback)
        {
            PlayerData playerData = null;
            UnturnedPlayer unturnedPlayer = null;
            ulong steamId = 0;
            if (command.Length == 0)
            {
                unturnedPlayer = (UnturnedPlayer)caller;
            }
            else
            {
                unturnedPlayer = UnturnedPlayer.FromName(command[0]);
                if (unturnedPlayer == null)
                {
                    if (!ulong.TryParse(command[0], out steamId))
                    {
                        pluginInstance.SendMessageToPlayer(caller, "PlayerNotFound", command[0]);
                        return;
                    }
                }
            }

            if (unturnedPlayer != null)
            {
                PlayerStatsComponent component = unturnedPlayer.GetComponent<PlayerStatsComponent>();
                if (component == null || component.PlayerData == null)
                {
                    pluginInstance.SendMessageToPlayer(caller, "PlayerStatsNotLoaded", unturnedPlayer.DisplayName);
                    return;
                }

                playerData = component.PlayerData;
                callback.Invoke(playerData);
            }
            else
            {
                ThreadHelper.RunAsynchronously(() =>
                {
                    playerData = pluginInstance.Database.GetPlayer(steamId);
                    ThreadHelper.RunSynchronously(() =>
                    {
                        if (playerData == null)
                        {
                            pluginInstance.SendMessageToPlayer(caller, "PlayerNotFound", steamId);
                            return;
                        }

                        callback.Invoke(playerData);
                    });                    
                });
            }
        }
    }
}
