using RestoreMonarchy.PlayerStats.Components;
using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace RestoreMonarchy.PlayerStats.Commands
{
    public class StatsUICommand : IRocketCommand
    {
        private PlayerStatsPlugin pluginInstance => PlayerStatsPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            PlayerStatsComponent component = player.GetComponent<PlayerStatsComponent>();

            if (component == null)
            {
                pluginInstance.SendMessageToPlayer(player, "PlayerStatsNotLoaded");
                return;
            }

            bool flag = component.PlayerData.UIDisabled;
            component.PlayerData.UIDisabled = !component.PlayerData.UIDisabled;
            if (flag)
            {
                component.SendUIEffect();
            } else
            {
                component.CloseUIEffect();
            }

            if (flag)
            {
                pluginInstance.SendMessageToPlayer(player, "StatsUIDisabled");
            }
            else
            {
                pluginInstance.SendMessageToPlayer(player, "StatsUIEnabled");
            }

        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "statsui";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => [];

        public List<string> Permissions => [];
    }
}
