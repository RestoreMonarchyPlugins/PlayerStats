using RestoreMonarchy.PlayerStats.Helpers;
using RestoreMonarchy.PlayerStats.Models;
using SDG.Unturned;
using System;
using UnityEngine;

namespace RestoreMonarchy.PlayerStats.Components
{
    public class PlayerStatsComponent : MonoBehaviour
    {
        private PlayerStatsPlugin pluginInstance => PlayerStatsPlugin.Instance;
        private PlayerStatsConfiguration configuration => pluginInstance.Configuration.Instance;

        public Player Player { get; private set; }
        public PlayerData PlayerData { get; private set; }

        void Awake()
        {
            Player = gameObject.GetComponent<Player>();
            ulong steamId = Player.channel.owner.playerID.steamID.m_SteamID;
            string name = Player.channel.owner.playerID.characterName;
            ThreadHelper.RunAsynchronously(() =>
            {   
                PlayerData playerData = pluginInstance.Database.GetOrAddPlayer(steamId, name);
                ThreadHelper.RunSynchronously(() =>
                {
                    if (Player != null)
                    {
                        playerData.Name = Player.channel.owner.playerID.characterName;
                        PlayerData = playerData;
                    }                    
                });
            });
        }

        void Start()
        {
            InvokeRepeating(nameof(UpdatePlaytime), 1, 1);
        }

        void OnDestroy()
        {
            CancelInvoke(nameof(UpdatePlaytime));
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

        internal void OnPlayerDeath(Player killer, ELimb limb, EDeathCause cause)
        {
            if (killer != null)
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
                }
            } else
            {
                PlayerData.PVEDeaths++;  
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
                    break;
                case EPlayerStat.KILLS_ZOMBIES_MEGA:
                    PlayerData.MegaZombies++;
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
