using RestoreMonarchy.PlayerStats.Models;
using Rocket.Core.Logging;
using SDG.NetTransport;
using SDG.Unturned;

namespace RestoreMonarchy.PlayerStats.Components
{
    public partial class PlayerStatsComponent
    {
        private bool isOpen = false;
        private const short Key = 22512;
        private ITransportConnection TransportConnection => Player.channel.GetOwnerTransportConnection();

        public void SendUIEffect()
        {
            if (isOpen)
            {
                Logger.Log($"PlayerStats UI is already open for {Name} ({SteamId})");
                return;
            }

            isOpen = true;
            EffectManager.sendUIEffect(configuration.UIEffectId, Key, TransportConnection, true);

            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_Kills_Text", pluginInstance.Translate("UI_Kills"));
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_Deaths_Text", pluginInstance.Translate("UI_Deaths"));
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_HeadShot_Text", pluginInstance.Translate("UI_Headshots"));
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_Accuracy_Text", pluginInstance.Translate("UI_Accuracy"));
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Rank_Text", pluginInstance.Translate("UI_Rank"));
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_KD_Text", pluginInstance.Translate("UI_KDR"));
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Footer_Text", pluginInstance.Translate("UI_Footer"));

            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Rank_Stats_Text", "-");


            if (configuration.EnableRewards && configuration.PVPRewards)
            {
                EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "PlayerStats_ProgressBar", true);
            } else
            {
                EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "PlayerStats_ProgressBar", false);
            }

            UpdateUIEffect();
            UpdateUIRanking();
            ShowUIEffect();
        }

        public void UpdateUIEffectRank(PlayerRanking playerRanking)
        {
            if (!isOpen)
            {
                return;
            }

            if (!configuration.PVPRanking)
            {
                return;
            }

            string rankString;
            if (playerRanking.IsUnranked())
            {
                rankString = "-";
            } else
            {
                rankString = "#" + playerRanking.Rank.ToString("N0");
            }
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Rank_Stats_Text", rankString);
        }

        private int prevPercentageProgress = 0;

        public void UpdateUIEffect()
        {
            if (!isOpen)
            {
                return;
            }

            string kills = PlayerData.Kills.ToString("N0");
            string deaths = PlayerData.PVPDeaths.ToString("N0");
            string headshots = PlayerData.Headshots.ToString("N0");            
            string hsPercentage = (PlayerData.Kills == 0 ? "0" : (((decimal)PlayerData.Headshots / PlayerData.Kills) * 100).ToString("N0")) + "%";
            string kdr = PlayerData.PVPDeaths == 0 ? PlayerData.Kills.ToString("N2") : ((decimal)PlayerData.Kills / PlayerData.PVPDeaths).ToString("N2");

            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_Kills_Stats_Text", kills);
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_Deaths_Stats_Text", deaths);
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_HeadShot_Stats_Text", headshots);
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_Accuracy_Stats_Text", hsPercentage);
            EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Stats_KD_Stats_Text", kdr);
                        
            if (configuration.EnableRewards && configuration.PVPRewards)
            {
                Reward reward = GetNextReward();
                if (reward != null)
                {
                    string nextReward = pluginInstance.Translate("UI_NextReward", reward.Name);
                    string progress = pluginInstance.Translate("UI_RewardProgress", PlayerData.Kills.ToString("N0"), reward.Treshold.ToString("N0"));
                    int progressPercentage = (int)((PlayerData.Kills / (float)reward.Treshold) * 100);

                    EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Reward_Text", nextReward);
                    EffectManager.sendUIEffectText(Key, TransportConnection, true, "PlayerStats_Progress_Text", progress);
                                        
                    EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, $"PlayerStats_ProgressBar_Fill_{prevPercentageProgress}", false);
                    EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, $"PlayerStats_ProgressBar_Fill_{progressPercentage}", true);
                    prevPercentageProgress = progressPercentage;

                } else
                {
                    EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "PlayerStats_ProgressBar", false);
                }
            }
        }

        public void ShowUIEffect()
        {
            if (!isOpen)
            {
                return;
            }

            EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "PlayerStatsUIHolder", true);
        }

        public void HideUIEffect()
        {
            EffectManager.sendUIEffectVisibility(Key, TransportConnection, true, "PlayerStatsUIHolder", false);
        }

        public void CloseUIEffect()
        {
            EffectManager.askEffectClearByID(configuration.UIEffectId, TransportConnection);
            isOpen = false;
        }
    }
}
