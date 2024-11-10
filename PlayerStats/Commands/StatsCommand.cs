using RestoreMonarchy.PlayerStats.Helpers;
using RestoreMonarchy.PlayerStats.Models;
using Rocket.API;
using System.Collections.Generic;

namespace RestoreMonarchy.PlayerStats.Commands
{
    public class StatsCommand : IRocketCommand
    {
        private PlayerStatsPlugin pluginInstance => PlayerStatsPlugin.Instance;
        private PlayerStatsConfiguration configuration => pluginInstance.Configuration.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 1 && caller is ConsolePlayer)
            {
                pluginInstance.SendMessageToPlayer(caller, "StatsCommandSyntax");
                return;
            }

            CommandHelper.GetPlayerData(caller, command, (playerData) =>
            {
                if (configuration.StatsMode == StatsMode.Both || configuration.StatsMode == StatsMode.PVP)
                {
                    string kills = playerData.Kills.ToString("N0");
                    string deaths = playerData.PVPDeaths.ToString("N0");
                    string kdr = deaths == "0" ? kills : ((decimal)playerData.Kills / playerData.PVPDeaths).ToString("N2");
                    string hsPercentage = (playerData.Kills == 0 ? "0" : (((decimal)playerData.Headshots / playerData.Kills) * 100).ToString("N0")) + "%";

                    if (caller.Id == playerData.SteamId.ToString())
                    {
                        pluginInstance.SendMessageToPlayer(caller, "YourPVPStats", kills, deaths, kdr, hsPercentage);
                    } else
                    {
                        pluginInstance.SendMessageToPlayer(caller, "OtherPVPStats", playerData.Name, kills, deaths, kdr, hsPercentage);
                    }                    
                }

                if (configuration.StatsMode == StatsMode.Both || configuration.StatsMode == StatsMode.PVE)
                {
                    string zombies = playerData.Zombies.ToString("N0");
                    string megaZombies = playerData.MegaZombies.ToString("N0");
                    string animals = playerData.Animals.ToString("N0");
                    string resources = playerData.Resources.ToString("N0");
                    string harvests = playerData.Harvests.ToString("N0");
                    string fish = playerData.Fish.ToString("N0");

                    if (caller.Id == playerData.SteamId.ToString())
                    {
                        pluginInstance.SendMessageToPlayer(caller, "YourPVEStats", zombies, megaZombies, animals, resources, harvests, fish);
                    }
                    else
                    {
                        pluginInstance.SendMessageToPlayer(caller, "OtherPVEStats", playerData.Name, zombies, megaZombies, animals, resources, harvests, fish);
                    }
                }
            });
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "stats";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new();

        public List<string> Permissions => new();
    }
}
