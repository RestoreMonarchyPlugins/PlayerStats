using SDG.NetTransport;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.PlayerStats.Components
{
    public partial class PlayerStatsComponent
    {
        private const short Key = 22512;
        private ITransportConnection TransportConnection => Player.channel.GetOwnerTransportConnection();

        public void SendUIEffect()
        {
            EffectManager.sendUIEffect(configuration.UIEffectId, Key, TransportConnection, true);

            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_Kills_Text", pluginInstance.Translate("UI_Kills"));
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_Deaths_Text", pluginInstance.Translate("UI_Deaths"));
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_HeadShot_Text", pluginInstance.Translate("UI_Headshots"));
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_Accuracy_Text", pluginInstance.Translate("UI_Accuracy"));
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Rank_Text", pluginInstance.Translate("UI_Rank"));
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_KD_Text", pluginInstance.Translate("UI_KDR"));

            UpdateUIEffect();
            ShowUIEffect();
        }

        public void UpdateUIEffectRank(int rank)
        {

        }

        public void UpdateUIEffect()
        {
            string kills = PlayerData.Kills.ToString("N0");
            string deaths = PlayerData.PVPDeaths.ToString("N0");
            string headshots = PlayerData.Headshots.ToString("N0");            
            string hsPercentage = PlayerData.Kills == 0 ? "0" : ((PlayerData.Headshots / PlayerData.Kills) * 100).ToString("N0");
            string kdr = PlayerData.Kills == 0 ? PlayerData.Kills.ToString("N2") : (PlayerData.Kills / PlayerData.PVPDeaths).ToString("N2");

            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_Kills_Stats_Text", kills);
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_Deaths_Stats_Text", deaths);
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_HeadShot_Stats_Text", headshots);
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_Accuracy_Stats_Text", hsPercentage);
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_KD_Stats_Text", kdr);
        }

        public void ShowUIEffect()
        {
            EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "PlayerStatsUIHolder", true);
        }

        public void HideUIEffect()
        {
            EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "PlayerStatsUIHolder", false);
        }

        public void CloseUIEffect()
        {

        }
    }
}
