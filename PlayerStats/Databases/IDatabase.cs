using RestoreMonarchy.PlayerStats.Models;
using System.Collections.Generic;

namespace RestoreMonarchy.PlayerStats.Databases
{
    public interface IDatabase
    {
        void Initialize();
        void Reload();
        void Save(IEnumerable<PlayerStatsData> playersData);
        PlayerStatsData GetPlayer(ulong steamId);
        void AddOrUpdatePlayer(PlayerStatsData player);
        PlayerStatsData GetOrAddPlayer(ulong steamId, string name);
        PlayerRanking GetPlayerRanking(ulong steamId, string orderBy = null);
        IEnumerable<PlayerRanking> GetPlayerRankings(int amount, string orderBy = null);
    }
}
