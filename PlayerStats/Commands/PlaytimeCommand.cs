using RestoreMonarchy.PlayerStats.Helpers;
using Rocket.API;
using System;
using System.Collections.Generic;

namespace RestoreMonarchy.PlayerStats.Commands
{
    public class PlaytimeCommand : IRocketCommand
    {
        private PlayerStatsPlugin pluginInstance => PlayerStatsPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 0 && caller is ConsolePlayer)
            {
                pluginInstance.SendMessageToPlayer(caller, "PlaytimeCommandSyntax");
                return;
            }

            CommandHelper.GetPlayerData(caller, command, (playerData) =>
            {
                TimeSpan timespan = TimeSpan.FromSeconds(playerData.Playtime);
                string playtime = pluginInstance.FormatTimespan(timespan);
                if (playerData.SteamId.ToString() == caller.Id)
                {
                    pluginInstance.SendMessageToPlayer(caller, "YourPlaytime", playtime);
                }
                else
                {
                    pluginInstance.SendMessageToPlayer(caller, "OtherPlaytime", playerData.Name, playtime);
                }
            });
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "playtime";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new();

        public List<string> Permissions => new();
    }
}
