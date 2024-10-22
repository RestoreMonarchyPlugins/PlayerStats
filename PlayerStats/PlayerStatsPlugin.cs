﻿using RestoreMonarchy.PlayerStats.Components;
using RestoreMonarchy.PlayerStats.Databases;
using RestoreMonarchy.PlayerStats.Helpers;
using RestoreMonarchy.PlayerStats.Models;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Core.Steam;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Linq;

namespace RestoreMonarchy.PlayerStats
{
    public class PlayerStatsPlugin : RocketPlugin<PlayerStatsConfiguration>
    {
        public static PlayerStatsPlugin Instance { get; private set; }
        public UnityEngine.Color MessageColor { get; set; }
        public IDatabase Database { get; private set; }

        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, UnityEngine.Color.green);

            if (Configuration.Instance.DatabaseProvider.Equals("mysql", StringComparison.OrdinalIgnoreCase))
            {
                Database = new MySQLDatabase();
                Logger.Log("Database provider is set to MySQL");
            } else
            {
                Database = new JsonDatabase();
                Logger.Log("Database provider is set to JSON");
            }            
            Database.Initialize();

            U.Events.OnPlayerConnected += OnPlayerConnected;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
            PlayerLife.onPlayerDied += OnPlayerDied;
            UnturnedPlayerEvents.OnPlayerUpdateStat += OnPlayerUpdatedStat;
            StructureManager.onStructureSpawned += OnStructureSpawned;
            BarricadeManager.onBarricadeSpawned += OnBarricadeSpawned;

            foreach (Player player in PlayerTool.EnumeratePlayers())
            {
                player.gameObject.AddComponent<PlayerStatsComponent>(); 
            }

            InvokeRepeating(nameof(Save), Configuration.Instance.SaveIntervalSeconds, Configuration.Instance.SaveIntervalSeconds);

            Logger.Log($"{Name} {Assembly.GetName().Version.ToString(3)} has been loaded!", ConsoleColor.Yellow);
            Logger.Log($"Check out more Unturned plugins at restoremonarchy.com");
        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;
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
                Destroy(component);
            }

            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        public override TranslationList DefaultTranslations => new()
        {
            { "StatsCommandSyntax", "You must specify player name or steamID." },
            { "PlayerStatsNotLoaded", "Player stats are not loaded for [[b]]{0}.[[/b]] Please try again later." },
            { "PlayerNotFound", "Player [[b]]{0}[[/b]] not found." },
            { "YourPVPStats", "[[b]]Your[[/b]] PVP stats | Kills: [[b]]{0}[[/b]], Deaths: [[b]]{1}[[/b]], KDR: [[b]]{2}[[/b]], HS%: [[b]]{3}[[/b]]" },
            { "YourPVEStats", "[[b]]Your[[/b]] PVE stats | Zombies: [[b]]{0}[[/b]], Mega Zombies: [[b]]{1}[[/b]], Animals: [[b]]{2}[[/b]], Resources: [[b]]{3}[[/b]], Harvests: [[b]]{4}[[/b]], Fish: [[b]]{5}[[/b]]" },
            { "OtherPVPStats", "[[b]]{0}[[/b]] PVP stats | Kills: [[b]]{1}[[/b]], Deaths: [[b]]{2}[[/b]], KDR: [[b]]{3}[[/b]], HS%: [[b]]{4}[[/b]]" },
            { "OtherPVEStats", "[[b]]{0}[[/b]] PVE stats | Zombies: [[b]]{1}[[/b]], Mega Zombies: [[b]]{2}[[/b]], Animals: [[b]]{3}[[/b]], Resources: [[b]]{4}[[/b]], Harvests: [[b]]{5}[[/b]], Fish: [[b]]{6}[[/b]]" },
            { "PlaytimeCommandSyntax", "You must specify player name or steamID." },
            { "YourPlaytime", "You have played for [[b]]{0}[[/b]]" },
            { "OtherPlaytime", "[[b]]{0}[[/b]] has played for [[b]]{1}[[/b]]" },
            { "RankCommandSyntax", "You must specify player name or steamID." },
            { "YourPlayerPVPRanking", "Your rank is [[b]]#{0}[[/b]] with {1} kills" },
            { "OtherPlayerPVPRanking", "[[b]]{0}[[/b]] rank is [[b]]#{1}[[/b]] with {2} kills." },
            { "YourPlayerPVERanking", "Your rank is [[b]]#{0}[[/b]] with {1} zombie kills." },
            { "OtherPlayerPVERanking", "[[b]]{0}[[/b]] rank is [[b]]#{1}[[/b]] with {2} zombie kills." },
            { "RankingListHeaderPVP", "[[b]]Top {0} Players by Kills[[/b]]" },
            { "RankingListItemPVP", "[[b]]#{0}[[/b]] [[b]]{1}[[/b]] - {2} kills" },
            { "RankingListHeaderPVE", "[[b]]Top {0} Players by Zombie Kills[[/b]]" },
            { "RankingListItemPVE", "[[b]]#{0}[[/b]] [[b]]{1}[[/b]] - {2} zombie kills" },
            { "YouAreUnrankedPVP", "You are unranked because you have [[b]]{0}/{1}[[/b]] kills. " },
            { "OtherPlayerIsUnrankedPVP", "[[b]]{0}[[/b]] is unranked because they have [[b]]{1}/{2}[[/b]] kills." },
            { "YouAreUnrankedPVE", "You are unranked because you have [[b]]{0}/{1}[[/b]] zombie kills. " },
            { "OtherPlayerIsUnrankedPVE", "[[b]]{0}[[/b]] is unranked because they have [[b]]{1}/{2}[[/b]] zombie kills." },
            { "NoRankingPlayersFound", "There isn't any players qualified for ranking yet." },
            { "StatsUIDisabled", "Stats UI has been disabled" },
            { "StatsUIEnabled", "Stats UI has been enabled" },
            { "RewardReceivedPVP", "You received [[b]]{0}[[/b]] reward for {1} kills." },
            { "RewardReceivedPVE", "You received [[b]]{0}[[/b]] reward for {1} zombie kills." },
            { "YourPVPSessionStats", "[[b]]Your[[/b]] PVP session stats | Kills: [[b]]{0}[[/b]], Deaths: [[b]]{1}[[/b]], KDR: [[b]]{2}[[/b]], HS%: [[b]]{3}[[/b]]" },
            { "OtherPVPSessionStats", "[[b]]{0}[[/b]] PVP session stats | Kills: [[b]]{1}[[/b]], Deaths: [[b]]{2}[[/b]], KDR: [[b]]{3}[[/b]], HS%: [[b]]{4}[[/b]]" },
            { "YourPVESessionStats", "[[b]]Your[[/b]] PVE session stats | Zombies: [[b]]{0}[[/b]], Mega Zombies: [[b]]{1}[[/b]], Animals: [[b]]{2}[[/b]], Resources: [[b]]{3}[[/b]], Harvests: [[b]]{4}[[/b]], Fish: [[b]]{5}[[/b]]" },
            { "OtherPVESessionStats", "[[b]]{0}[[/b]] PVE session stats | Zombies: [[b]]{1}[[/b]], Mega Zombies: [[b]]{2}[[/b]], Animals: [[b]]{3}[[/b]], Resources: [[b]]{4}[[/b]], Harvests: [[b]]{5}[[/b]], Fish: [[b]]{6}[[/b]]" },
            { "SessionStatsCommandSyntax", "You must specify player name or steamID." },
            { "SessionPlaytimeCommandSyntax", "You must specify player name or steamID." },
            { "YourSessionPlaytime", "You have played for [[b]]{0}[[/b]] since you joined." },
            { "OtherSessionPlaytime", "[[b]]{0}[[/b]] has played for [[b]]{1}[[/b]] since they joined." },

            { "JoinMessage", "[[b]][#{0}] {1}[[/b]] joined the server." },
            { "LeaveMessage", "[[b]][#{0}] {1}[[/b]] left the server." },
            { "JoinMessageNoRank", "[[b]]{0}[[/b]] joined the server." },
            { "LeaveMessageNoRank", "[[b]]{0}[[/b]] left the server." },

            { "Day", "1 day" },
            { "Days", "{0} days" },
            { "Hour", "1 hour" },
            { "Hours", "{0} hours" },
            { "Minute", "1 minute" },
            { "Minutes", "{0} minutes" },
            { "Second", "1 second" },
            { "Seconds", "{0} seconds" },
            { "Zero", "a moment" },

            { "UI_NextReward", "Next Reward: {0}" },
            { "UI_RewardProgress", "{0}/{1} Kills" },
            { "UI_Kills", "KILLS" },
            { "UI_Deaths", "DEATHS" },
            { "UI_Headshots", "HS" },
            { "UI_Accuracy", "HS%" },
            { "UI_Rank", "RANK" },
            { "UI_KDR", "K/D" },
            { "UI_Footer", "Use /statsui to hide" },

        };

        internal string FormatTimespan(TimeSpan span)
        {
            if (span <= TimeSpan.Zero) return Translate("Zero");

            List<string> items = new();
            if (span.Days > 0)
                items.Add(span.Days == 1 ? Translate("Day") : Translate("Days", span.Days));
            if (span.Hours > 0)
                items.Add(span.Hours == 1 ? Translate("Hour") : Translate("Hours", span.Hours));
            if (items.Count < 2 && span.Minutes > 0)
                items.Add(span.Minutes == 1 ? Translate("Minute") : Translate("Minutes", span.Minutes));
            if (items.Count < 2 && span.Seconds > 0)
                items.Add(span.Seconds == 1 ? Translate("Second") : Translate("Seconds", span.Seconds));

            return string.Join(" ", items);
        }

        private void Save(bool async = true)
        {
            List<PlayerData> playersData = [];
            foreach (Player player in PlayerTool.EnumeratePlayers())
            {
                PlayerStatsComponent component = player.GetComponent<PlayerStatsComponent>();
                if (component != null && component.Loaded)
                {
                    playersData.Add(component.PlayerData);
                }
            }

            if (playersData.Count == 0)
            {
                return;
            }

            if (async)
            {
                ThreadHelper.RunAsynchronously(() => Database.Save(playersData));
            } else
            {
                Database.Save(playersData);
            }            
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            PlayerStatsComponent component = player.GetComponent<PlayerStatsComponent>();
            if (component == null)
            {
                component = player.Player.gameObject.AddComponent<PlayerStatsComponent>();

                if (Configuration.Instance.EnableJoinLeaveMessages)
                {
                    ThreadHelper.RunAsynchronously(() =>
                    {
                        PlayerRanking playerRanking = Database.GetPlayerRanking(component.SteamId);
                        ThreadHelper.RunSynchronously(() =>
                        {
                            if (playerRanking == null || playerRanking.IsUnranked())
                            {
                                SendMessageToPlayer(null, "JoinMessageNoRank", player.CharacterName);
                            }
                            else
                            {
                                SendMessageToPlayer(null, "JoinMessage", playerRanking.Rank.ToString("N0"), player.CharacterName);
                            }
                        });
                    });
                }
            }
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            PlayerStatsComponent component = player.GetComponent<PlayerStatsComponent>();
            if (component != null)
            {
                if (component.Loaded)
                {
                    ThreadHelper.RunAsynchronously(() =>
                    {
                        Database.AddOrUpdatePlayer(component.PlayerData);
                    });
                }

                if (Configuration.Instance.EnableJoinLeaveMessages)
                {
                    ThreadHelper.RunAsynchronously(() =>
                    {
                        PlayerRanking playerRanking = Database.GetPlayerRanking(component.SteamId);
                        ThreadHelper.RunSynchronously(() =>
                        {
                            if (playerRanking == null || playerRanking.IsUnranked())
                            {
                                SendMessageToPlayer(null, "LeaveMessageNoRank", player.CharacterName);
                            }
                            else
                            {
                                SendMessageToPlayer(null, "LeaveMessage", playerRanking.Rank, player.CharacterName);
                            }
                        });
                    });
                }
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

        internal void SendMessageToPlayer(IRocketPlayer player, string translationKey, params object[] placeholder)
        {
            string msg = Translate(translationKey, placeholder);
            msg = msg.Replace("[[", "<").Replace("]]", ">");
            if (player is ConsolePlayer)
            {
                msg = msg.Replace("<b>", "").Replace("</b>", "");
                Logger.Log(msg);
                return;
            }

            SteamPlayer steamPlayer = null;
            if (player != null)
            {
                UnturnedPlayer unturnedPlayer = (UnturnedPlayer)player;
                steamPlayer = unturnedPlayer.SteamPlayer();
            }
            
            ChatManager.serverSendMessage(msg, MessageColor, null, steamPlayer, EChatMode.SAY, Configuration.Instance.MessageIconUrl, true);
        }
    }
}