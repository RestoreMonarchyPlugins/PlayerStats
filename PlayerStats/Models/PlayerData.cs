﻿using System;

namespace RestoreMonarchy.PlayerStats.Models
{
    public class PlayerData
    {
        public ulong SteamId { get; set; }
        public string Name { get; set; }
        public int Kills { get; set; }
        public int Headshots { get; set; }
        public int PVPDeaths { get; set; }
        public int PVEDeaths { get; set; }
        public int Zombies { get; set; }
        public int MegaZombies { get; set; }
        public int Animals { get; set; }
        public int Resources { get; set; }
        public int Harvests { get; set; }
        public int Fish { get; set; }
        public int Structures { get; set; }
        public int Barricades { get; set; }
        public ulong Playtime  { get; set; }
        public bool UIDisabled { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
