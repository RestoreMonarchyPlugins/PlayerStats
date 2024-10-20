using RestoreMonarchy.PlayerStats.Models;
using System.Collections;
using System.Collections.Generic;

namespace RestoreMonarchy.PlayerStats.Databases
{
    public interface IDatabase
    {
        void Initialize();
        void Reload();
        void Save(IEnumerable<PlayerData> playersData);
        PlayerData GetPlayer(ulong steamId, string name = null);
        void AddOrUpdatePlayer(PlayerData player);
    }
}
