using Rocket.API;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.PlayerStats.Commands
{
    public class StatsCommand : IRocketCommand
    {
        private PlayerStatsPlugin pluginInstance => PlayerStatsPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length < 0 && caller is ConsolePlayer)
            {

            }
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "stats";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new();

        public List<string> Permissions => new();
    }
}
