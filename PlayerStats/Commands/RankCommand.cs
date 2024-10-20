using RestoreMonarchy.PlayerStats.Components;
using RestoreMonarchy.PlayerStats.Helpers;
using RestoreMonarchy.PlayerStats.Models;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.PlayerStats.Commands
{
    public class RankCommand : IRocketCommand
    {
        private PlayerStatsPlugin pluginInstance => PlayerStatsPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 0 && caller is ConsolePlayer)
            {
                pluginInstance.SendMessageToPlayer(caller, "RankCommandSyntax");
                return;
            }

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
                steamId = unturnedPlayer.CSteamID.m_SteamID;
            }

            ThreadHelper.RunAsynchronously(() =>
            {
                PlayerRanking playerRanking = pluginInstance.Database.GetPlayerRanking(steamId);
                ThreadHelper.RunSynchronously(() =>
                {
                    if (playerRanking == null)
                    {
                        pluginInstance.SendMessageToPlayer(caller, "PlayerNotFound", steamId);
                        return;
                    }

                    string rank = playerRanking.Rank.ToString();
                    if (pluginInstance.Configuration.Instance.PVPRanking)
                    {                        
                        string kills = playerRanking.Kills.ToString("N0");
                        if (caller.Id == playerRanking.SteamId.ToString())
                        {
                            pluginInstance.SendMessageToPlayer(caller, "YourPlayerPVPRanking", rank, kills);
                        } else
                        {
                            pluginInstance.SendMessageToPlayer(caller, "OtherPlayerPVPRanking", playerRanking.Name, rank, kills);
                        }                        
                    }
                    else
                    {
                        string zombies = playerRanking.Zombies.ToString("N0");
                        if (caller.Id == playerRanking.SteamId.ToString())
                        {
                            pluginInstance.SendMessageToPlayer(caller, "YourPlayerPVERanking", rank, zombies);
                        } else
                        {
                            pluginInstance.SendMessageToPlayer(caller, "OtherPlayerPVERanking", playerRanking.Name, rank, zombies);
                        }
                    }
                });
            });
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "rank";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => [];

        public List<string> Permissions => [];
    }
}
