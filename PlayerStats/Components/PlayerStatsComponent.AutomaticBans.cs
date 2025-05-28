using RestoreMonarchy.PlayerStats.Models;
using SDG.NetTransport;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;

namespace RestoreMonarchy.PlayerStats.Components
{
    public partial class PlayerStatsComponent
    {
        internal void CheckAutomaticBan()
        {
            if (!configuration.EnableAutomaticBans)
            {
                return;
            }

            foreach (AutomaticBan ban in configuration.AutomaticBans)
            {
                if (ban.CheckConditions(PlayerData))
                {
                    ITransportConnection transportConnection = Player.channel.owner.transportConnection;
                    uint ipAddress = 0;
                    transportConnection?.TryGetIPv4Address(out ipAddress);

                    IEnumerable<byte[]> hwids = Player.channel.owner.playerID.GetHwids();
                    Provider.requestBanPlayer(CSteamID.Nil, SteamID, ipAddress, hwids, ban.Reason, uint.MaxValue);
                }
            }
        }
    }
}
