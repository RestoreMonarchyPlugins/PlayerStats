# Player Stats
> Track and display player PVP and PVE statistics with ranking, rewards and UI

---

## 📊 Overview
Player Stats is a plugin that tracks various player statistics and provides ranking for both PVP and PVE gameplay. The plugin supports permission-based rewards and includes an optional visual UI.

## ✨ Features
| Feature | Description |
|---------|-------------|
| 💾 Data Storage | Store player stats in a JSON file or MySQL database |
| 📈 Stats Display | Show player stats, playtime, ranking, and session stats via commands |
| 🏆 Rankings | Support for both PVP (Kills) and PVE (Zombie Kills) leaderboards |
| 🎁 Rewards | Permission group rewards for reaching specific kill thresholds |
| 🔄 Migration | Automatic migration from Arechi PlayerStats plugin |
| 🖥️ User Interface | Optional UI for viewing PVP/PVE stats |

## 📊 Tracked Statistics

### PVP Stats
- Kills
- Deaths
- KDR (Kill/Death Ratio)
- HS% (Headshot Percentage)

### PVE Stats
- Zombies
- Mega Zombies
- Animals
- Resources
- Harvests
- Fish

---

## 🔧 Configuration Options

### Stats Mode (`<StatsMode>Both</StatsMode>`)

| Mode | Description |
|------|-------------|
| `Both` | The `/stats` command displays both PVP and PVE stats, but ranking, rewards and UI are based on PVP stats |
| `PVP` | The `/stats` command displays only PVP stats, and ranking, rewards and UI are based on PVP stats |
| `PVE` | The `/stats` command displays only PVE stats, and ranking, rewards and UI are based on PVE stats |

---

## 🖥️ Workshop Integration (Optional)
The UI is optional and provides a visual display for PVP stats. 

- **Workshop Item**: [Player Stats UI](https://steamcommunity.com/sharedfiles/filedetails/?id=3352126593)
- **ID**: `3352126593`

> 💡 **PRO TIP**  
> Remember to set `<EnableUIEffect>true</EnableUIEffect>` in the configuration file to activate the UI.

---

## 🔑 Commands

| Command | Description |
|---------|-------------|
| `/playtime [player]` | Shows your or another player's total playtime |
| `/stats [player]` | Displays your or another player's stats |
| `/rank [player]` | Shows your or another player's ranking |
| `/sstats [player]` | Displays your or another player's session stats (since they joined) |
| `/splaytime [player]` | Shows your or another player's session playtime (since they joined) |
| `/ranking` | Displays the top players ranking |
| `/statsui` | Toggles the stats UI on/off |

---

## 🔐 Permissions

```xml
<Permission Cooldown="0">playtime</Permission>
<Permission Cooldown="0">stats</Permission>
<Permission Cooldown="0">rank</Permission>
<Permission Cooldown="0">sstats</Permission>
<Permission Cooldown="0">splaytime</Permission>
<Permission Cooldown="0">ranking</Permission>
<Permission Cooldown="0">statsui</Permission>
```

---

## ⚙️ Configuration
```xml
<?xml version="1.0" encoding="utf-8"?>
<PlayerStatsConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <MessageColor>yellow</MessageColor>
  <MessageIconUrl>https://i.imgur.com/TWjBtCA.png</MessageIconUrl>
  <DatabaseProvider>json</DatabaseProvider>
  <JsonFilePath>{rocket_directory}/Plugins/PlayerStats/PlayerStats.json</JsonFilePath>
  <MySQLConnectionString>Server=127.0.0.1;Port=3306;Database=unturned;Uid=root;Pwd=passw;</MySQLConnectionString>
  <PlayerStatsTableName>PlayerStats</PlayerStatsTableName>
  <SaveIntervalSeconds>300</SaveIntervalSeconds>
  <EnableUIEffect>false</EnableUIEffect>
  <UIEffectId>22512</UIEffectId>
  <ShowUIEffectByDefault>true</ShowUIEffectByDefault>
  <EnableJoinLeaveMessages>true</EnableJoinLeaveMessages>
  <StatsMode>Both</StatsMode>
  <ShowCombinedDeaths>true</ShowCombinedDeaths>
  <MinimumRankingTreshold>25</MinimumRankingTreshold>
  <EnableRewards>true</EnableRewards>
  <Rewards>
    <Reward Name="VIP Rank" Treshold="50" PermissionGroup="vip" />
    <Reward Name="MVP Rank" Treshold="125" PermissionGroup="mvp" />
  </Rewards>
</PlayerStatsConfiguration>
```

---

## 👨‍💻 Credits
UI made by **💪 Soer (Unbeaten)**. He also sponsored the creation of this plugin 💸.

---

## ❓ Frequently Asked Questions

1. **How do I enable the UI?**  
   Set `<EnableUIEffect>true</EnableUIEffect>` in the configuration and subscribe to the workshop item.

2. **Can I use MySQL instead of JSON?**  
   Yes, change `<DatabaseProvider>json</DatabaseProvider>` to `<DatabaseProvider>mysql</DatabaseProvider>` and configure your connection string.

3. **How do I customize rewards?**  
   Edit the `<Rewards>` section in the configuration file with your desired thresholds and permission groups.

---

*For support, bug reports, or feature requests, please write on our forum or join our Discord.*