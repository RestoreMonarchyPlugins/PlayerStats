namespace RestoreMonarchy.PlayerStats.Models
{
    public class PlayerRanking
    {
        public int Rank { get; set; }
        public ulong SteamId { get; set; }
        public string Name { get; set; }
        public int Kills { get; set; }
        public int Headshots { get; set; }
        public int Deaths { get; set; }        
    }
}
