using Rocket.API;
using System.Collections.Generic;

namespace RestoreMonarchy.PlayerStats.Commands
{
    public class RankingCommand : IRocketCommand
    {
        private PlayerStatsPlugin pluginInstance => PlayerStatsPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {

        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "ranking";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => [];

        public List<string> Permissions => [];
    }
}
