using RestoreMonarchy.PlayerStats.Components;
using RestoreMonarchy.PlayerStats.Databases;
using RestoreMonarchy.PlayerStats.Helpers;
using RestoreMonarchy.PlayerStats.Models;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestoreMonarchy.PlayerStats
{
    public class PlayerStatsPlugin : RocketPlugin<PlayerStatsConfiguration>
    {
        public static PlayerStatsPlugin Instance { get; private set; }
        public IDatabase Database { get; private set; }

        protected override void Load()
        {
            Instance = this;

            Database = new JsonDatabase();
            Database.Initialize();

            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
            PlayerLife.onPlayerDied += OnPlayerDied;
            UnturnedPlayerEvents.OnPlayerUpdateStat += OnPlayerUpdatedStat;
            StructureManager.onStructureSpawned += OnStructureSpawned;
            BarricadeManager.onBarricadeSpawned += OnBarricadeSpawned;

            foreach (Player player in PlayerTool.EnumeratePlayers())
            {
                PlayerStatsComponent component = player.GetComponent<PlayerStatsComponent>();
                if (component == null)
                {
                    player.gameObject.AddComponent<PlayerStatsComponent>();
                }
            }

            InvokeRepeating(nameof(Save), Configuration.Instance.SaveIntervalSeconds, Configuration.Instance.SaveIntervalSeconds);

            Logger.Log($"{Name} {Assembly.GetName().Version.ToString(3)} has been loaded!", ConsoleColor.Yellow);
            Logger.Log($"Check out more Unturned plugins at restoremonarchy.com");
        }

        protected override void Unload()
        {
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
            PlayerLife.onPlayerDied -= OnPlayerDied;
            UnturnedPlayerEvents.OnPlayerUpdateStat -= OnPlayerUpdatedStat;
            StructureManager.onStructureSpawned -= OnStructureSpawned;
            BarricadeManager.onBarricadeSpawned -= OnBarricadeSpawned;

            CancelInvoke(nameof(Save));
            Save();

            foreach (Player player in PlayerTool.EnumeratePlayers())
            {
                PlayerStatsComponent component = player.GetComponent<PlayerStatsComponent>();
                if (component == null)
                {
                    Destroy(component);
                }
            }

            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        public override TranslationList DefaultTranslations => new()
        {
            { "", "" }
        };

        private void Save(bool async = true)
        {
            List<PlayerData> playersData = [];
            foreach (Player player in PlayerTool.EnumeratePlayers())
            {
                PlayerStatsComponent component = player.GetComponent<PlayerStatsComponent>();
                if (component != null)
                {
                    playersData.Add(component.PlayerData);
                }
            }

            if (async)
            {
                ThreadHelper.RunAsynchronously(() => Database.Save(playersData));
            } else
            {
                Database.Save(playersData);
            }            
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            PlayerStatsComponent component = player.GetComponent<PlayerStatsComponent>();
            if (component != null)
            {
                ThreadHelper.RunAsynchronously(() =>
                {
                    Database.AddOrUpdatePlayer(component.PlayerData);
                });
            }
        }

        private void OnPlayerDied(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator)
        {
            PlayerStatsComponent component = sender.GetComponent<PlayerStatsComponent>();
            if (component != null)
            {
                Player killer = PlayerTool.getPlayer(instigator);
                component.OnPlayerDeath(killer, limb, cause);
            }
        }

        private readonly EPlayerStat[] stats =
        [
            EPlayerStat.FOUND_FISHES,
            EPlayerStat.KILLS_ANIMALS,
            EPlayerStat.KILLS_ZOMBIES_NORMAL,
            EPlayerStat.KILLS_ZOMBIES_MEGA,
            EPlayerStat.FOUND_RESOURCES,
            EPlayerStat.FOUND_PLANTS
        ];

        private void OnPlayerUpdatedStat(UnturnedPlayer player, EPlayerStat stat)
        {
            if (stats.Contains(stat))
            {
                PlayerStatsComponent component = player.GetComponent<PlayerStatsComponent>();
                if (component != null)
                {
                    component.OnPlayerUpdatedStat(stat);
                }
            }
        }

        private void OnBarricadeSpawned(BarricadeRegion region, BarricadeDrop drop)
        {
            BarricadeData barricadeData = drop.GetServersideData();
            if (barricadeData.owner == 0)
            {
                return;
            }

            Player player = PlayerTool.getPlayer(new CSteamID(barricadeData.owner));
            if (player == null)
            {
                return;
            }

            PlayerStatsComponent component = player.GetComponent<PlayerStatsComponent>();
            if (component != null)
            {
                component.OnBarricadeSpawned();
            }
        }

        private void OnStructureSpawned(StructureRegion region, StructureDrop drop)
        {
            StructureData structureData = drop.GetServersideData();
            if (structureData.owner == 0)
            {
                return;
            }

            Player player = PlayerTool.getPlayer(new CSteamID(structureData.owner));
            if (player == null)
            {
                return;
            }

            PlayerStatsComponent component = player.GetComponent<PlayerStatsComponent>();
            if (component != null)
            {
                component.OnStructureSpawned();
            }
        }
    }
}