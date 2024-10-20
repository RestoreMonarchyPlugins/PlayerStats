using RestoreMonarchy.PlayerStats.Models;
using System.Collections.Generic;

namespace RestoreMonarchy.PlayerStats.Databases
{
    public interface IDatabase
    {
        void Initialize();
        void Reload();
        void Save(IEnumerable<PlayerData> playersData);
        PlayerData GetPlayer(ulong steamId);
        void AddOrUpdatePlayer(PlayerData player);
        PlayerData GetOrAddPlayer(ulong steamId, string name);
        PlayerRanking GetPlayerRanking(ulong steamId);
    }
}
