using RestoreMonarchy.PlayerStats.Helpers;
using RestoreMonarchy.PlayerStats.Models;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RestoreMonarchy.PlayerStats.Components
{
    public partial class PlayerStatsComponent : MonoBehaviour
    {
        private PlayerStatsPlugin pluginInstance => PlayerStatsPlugin.Instance;
        private PlayerStatsConfiguration configuration => pluginInstance.Configuration.Instance;

        public Player Player { get; private set; }
        private string Name => Player.channel.owner.playerID.characterName;
        private ulong SteamId => Player.channel.owner.playerID.steamID.m_SteamID;
        public PlayerData PlayerData { get; private set; }

        private Reward GetCurrentReward() => configuration.Rewards.OrderByDescending(x => x.Treshold).FirstOrDefault(x => x.Treshold <= PlayerData.Kills);
        private Reward GetNextReward() => configuration.Rewards.OrderBy(x => x.Treshold).FirstOrDefault(x => x.Treshold > PlayerData.Kills);

        void Awake()
        {
            Player = gameObject.GetComponent<Player>();
            ThreadHelper.RunAsynchronously(() =>
            {   
                PlayerData playerData = pluginInstance.Database.GetOrAddPlayer(SteamId, Name);
                ThreadHelper.RunSynchronously(() =>
                {
                    if (Player != null && this != null)
                    {
                        playerData.Name = Name;
                        PlayerData = playerData;

                        if (configuration.EnableUIEffect && !playerData.UIDisabled)
                        {
                            SendUIEffect();
                        }
                    }
                });
            });
        }

        void Start()
        {
            InvokeRepeating(nameof(UpdatePlaytime), 1, 1);
            InvokeRepeating(nameof(UpdateUIRanking), 90, 90);
        }

        void OnDestroy()
        {
            CancelInvoke(nameof(UpdatePlaytime));
            CancelInvoke(nameof(UpdateUIRanking));

            CloseUIEffect();
        }

        ulong prePlaytime = 0;
        private void UpdatePlaytime()
        {
            if (PlayerData == null)
            {
                prePlaytime++;
                return;
            }
            
            if (prePlaytime > 0)
            {
                PlayerData.Playtime += prePlaytime;
                prePlaytime = 0;
            }

            PlayerData.Playtime++;
        }

        private void UpdateUIRanking()
        {
            if (!isOpen)
            {
                return;
            }

            ThreadHelper.RunAsynchronously(() =>
            {
                PlayerRanking playerRanking = pluginInstance.Database.GetPlayerRanking(SteamId);
                ThreadHelper.RunSynchronously(() =>
                {
                    UpdateUIEffectRank(playerRanking);
                });
            });
        }

        internal void OnPlayerDeath(Player killer, ELimb limb, EDeathCause cause)
        {
            if (killer != null && killer != Player)
            {
                PlayerData.PVPDeaths++;

                PlayerStatsComponent killerComponent = killer.GetComponent<PlayerStatsComponent>();
                if (killerComponent != null)
                {
                    killerComponent.PlayerData.Kills++;
                    if (limb == ELimb.SKULL)
                    {
                        killerComponent.PlayerData.Headshots++;
                    }
                    killerComponent.UpdateUIEffect();
                    killerComponent.CheckGiveReward();
                }
                UpdateUIEffect();
            } else
            {
                PlayerData.PVEDeaths++;
            }
        }

        internal void CheckGiveReward()
        {
            if (!configuration.EnableRewards)
            {
                return;
            }

            Reward reward = GetCurrentReward();
            if (reward != null)
            {
                UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromPlayer(Player);
                List<RocketPermissionsGroup> groups = R.Permissions.GetGroups(unturnedPlayer, false);
                if (!groups.Exists(x => x.Id.Equals(reward.PermissionGroup, System.StringComparison.OrdinalIgnoreCase)))
                {
                    R.Permissions.AddPlayerToGroup(reward.PermissionGroup, unturnedPlayer);
                    string treshold = reward.Treshold.ToString("N0");
                    if (configuration.PVPRewards)
                    {
                        pluginInstance.SendMessageToPlayer(unturnedPlayer, "RewardReceivedPVP", reward.Name, treshold);
                    } else
                    {
                        pluginInstance.SendMessageToPlayer(unturnedPlayer, "RewardReceivedPVE", reward.Name, treshold);
                    }
                }                
            }
        }

        internal void OnPlayerUpdatedStat(EPlayerStat stat)
        {
            switch (stat)
            {
                case EPlayerStat.KILLS_ANIMALS:
                    PlayerData.Animals++;
                    break;
                case EPlayerStat.KILLS_ZOMBIES_NORMAL:
                    PlayerData.Zombies++;
                    CheckGiveReward();
                    break;
                case EPlayerStat.KILLS_ZOMBIES_MEGA:
                    PlayerData.MegaZombies++;
                    CheckGiveReward();
                    break;
                case EPlayerStat.FOUND_RESOURCES:
                    PlayerData.Resources++;
                    break;
                case EPlayerStat.FOUND_FISHES:
                    PlayerData.Fish++;
                    break;
                case EPlayerStat.FOUND_PLANTS:
                    PlayerData.Harvests++;
                    break;
            }
        }

        internal void OnStructureSpawned()
        {
            PlayerData.Structures++;
        }

        internal void OnBarricadeSpawned()
        {
            PlayerData.Barricades++;
        }
    }
}
