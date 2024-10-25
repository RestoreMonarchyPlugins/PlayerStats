# Player Stats
Player PVP and PVE stats with ranking and rewards.

## Features
* Stores player stats in a JSON file or MySQL database
* Displays player stats, playtime, ranking, and session stats using commands
* Supports PVP (Kills) and PVE (Zombie Kills) ranking
* Permission group rewards for reaching a certain number of kills
* Automatic migration from Arechi PlayerStats plugin
* UI for PVP stats

**PVP Stats:** Kills, Deaths, KDR, HS%  
**PVE Stats:** Zombies, Mega Zombies, Animals, Resources, Harvests, Fish

## Credits
UI made by **Soer (Unbeaten)**. He also sponsored the creation of this plugin.

## Workshop (optional)
The UI is optional and only for PVP stats. You can use the following workshop item to display the stats in-game.  
[Player Stats UI](https://steamcommunity.com/sharedfiles/filedetails/?id=3352126593) - `3352126593`

## Commands
* `/playtime [player]` - Displays your or other player's playtime
* `/stats [player]` - Displays your or other player's stats
* `/rank [player]` - Displays your or other player's ranking
* `/sstats [player]` - Displays your or other player's session stats (since they joined)
* `/splaytime [player]` - Displays your or other player's session playtime (since they joined)
* `/ranking` - Displays the top players ranking
* `/statsui` - Toggles the stats UI

## Configuration
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
  <EnablePVPStats>true</EnablePVPStats>
  <EnablePVEStats>true</EnablePVEStats>
  <PVPRanking>true</PVPRanking>
  <PVPRewards>true</PVPRewards>
  <MinimumRankingTreshold>25</MinimumRankingTreshold>
  <EnableRewards>true</EnableRewards>
  <Rewards>
    <Reward Name="VIP Rank" Treshold="50" PermissionGroup="vip" />
    <Reward Name="MVP Rank" Treshold="125" PermissionGroup="mvp" />
  </Rewards>
</PlayerStatsConfiguration>
```

## Translations
```xml
<?xml version="1.0" encoding="utf-8"?>
<Translations xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Translation Id="StatsCommandSyntax" Value="You must specify player name or steamID." />
  <Translation Id="PlayerStatsNotLoaded" Value="Player stats are not loaded for [[b]]{0}.[[/b]] Please try again later." />
  <Translation Id="PlayerNotFound" Value="Player [[b]]{0}[[/b]] not found." />
  <Translation Id="YourPVPStats" Value="[[b]]Your[[/b]] PVP stats | Kills: [[b]]{0}[[/b]], Deaths: [[b]]{1}[[/b]], KDR: [[b]]{2}[[/b]], HS%: [[b]]{3}[[/b]]" />
  <Translation Id="YourPVEStats" Value="[[b]]Your[[/b]] PVE stats | Zombies: [[b]]{0}[[/b]], Mega Zombies: [[b]]{1}[[/b]], Animals: [[b]]{2}[[/b]], Resources: [[b]]{3}[[/b]], Harvests: [[b]]{4}[[/b]], Fish: [[b]]{5}[[/b]]" />
  <Translation Id="OtherPVPStats" Value="[[b]]{0}[[/b]] PVP stats | Kills: [[b]]{1}[[/b]], Deaths: [[b]]{2}[[/b]], KDR: [[b]]{3}[[/b]], HS%: [[b]]{4}[[/b]]" />
  <Translation Id="OtherPVEStats" Value="[[b]]{0}[[/b]] PVE stats | Zombies: [[b]]{1}[[/b]], Mega Zombies: [[b]]{2}[[/b]], Animals: [[b]]{3}[[/b]], Resources: [[b]]{4}[[/b]], Harvests: [[b]]{5}[[/b]], Fish: [[b]]{6}[[/b]]" />
  <Translation Id="PlaytimeCommandSyntax" Value="You must specify player name or steamID." />
  <Translation Id="YourPlaytime" Value="You have played for [[b]]{0}[[/b]]" />
  <Translation Id="OtherPlaytime" Value="[[b]]{0}[[/b]] has played for [[b]]{1}[[/b]]" />
  <Translation Id="RankCommandSyntax" Value="You must specify player name or steamID." />
  <Translation Id="YourPlayerPVPRanking" Value="Your rank is [[b]]#{0}[[/b]] with {1} kills" />
  <Translation Id="OtherPlayerPVPRanking" Value="[[b]]{0}[[/b]] rank is [[b]]#{1}[[/b]] with {2} kills." />
  <Translation Id="YourPlayerPVERanking" Value="Your rank is [[b]]#{0}[[/b]] with {1} zombie kills." />
  <Translation Id="OtherPlayerPVERanking" Value="[[b]]{0}[[/b]] rank is [[b]]#{1}[[/b]] with {2} zombie kills." />
  <Translation Id="RankingListHeaderPVP" Value="[[b]]Top {0} Players by Kills[[/b]]" />
  <Translation Id="RankingListItemPVP" Value="[[b]]#{0}[[/b]] [[b]]{1}[[/b]] - {2} kills" />
  <Translation Id="RankingListHeaderPVE" Value="[[b]]Top {0} Players by Zombie Kills[[/b]]" />
  <Translation Id="RankingListItemPVE" Value="[[b]]#{0}[[/b]] [[b]]{1}[[/b]] - {2} zombie kills" />
  <Translation Id="YouAreUnrankedPVP" Value="You are unranked because you have [[b]]{0}/{1}[[/b]] kills. " />
  <Translation Id="OtherPlayerIsUnrankedPVP" Value="[[b]]{0}[[/b]] is unranked because they have [[b]]{1}/{2}[[/b]] kills." />
  <Translation Id="YouAreUnrankedPVE" Value="You are unranked because you have [[b]]{0}/{1}[[/b]] zombie kills. " />
  <Translation Id="OtherPlayerIsUnrankedPVE" Value="[[b]]{0}[[/b]] is unranked because they have [[b]]{1}/{2}[[/b]] zombie kills." />
  <Translation Id="NoRankingPlayersFound" Value="There isn't any players qualified for ranking yet." />
  <Translation Id="StatsUIDisabled" Value="Stats UI has been disabled" />
  <Translation Id="StatsUIEnabled" Value="Stats UI has been enabled" />
  <Translation Id="RewardReceivedPVP" Value="You received [[b]]{0}[[/b]] reward for {1} kills." />
  <Translation Id="RewardReceivedPVE" Value="You received [[b]]{0}[[/b]] reward for {1} zombie kills." />
  <Translation Id="YourPVPSessionStats" Value="[[b]]Your[[/b]] PVP session stats | Kills: [[b]]{0}[[/b]], Deaths: [[b]]{1}[[/b]], KDR: [[b]]{2}[[/b]], HS%: [[b]]{3}[[/b]]" />
  <Translation Id="OtherPVPSessionStats" Value="[[b]]{0}[[/b]] PVP session stats | Kills: [[b]]{1}[[/b]], Deaths: [[b]]{2}[[/b]], KDR: [[b]]{3}[[/b]], HS%: [[b]]{4}[[/b]]" />
  <Translation Id="YourPVESessionStats" Value="[[b]]Your[[/b]] PVE session stats | Zombies: [[b]]{0}[[/b]], Mega Zombies: [[b]]{1}[[/b]], Animals: [[b]]{2}[[/b]], Resources: [[b]]{3}[[/b]], Harvests: [[b]]{4}[[/b]], Fish: [[b]]{5}[[/b]]" />
  <Translation Id="OtherPVESessionStats" Value="[[b]]{0}[[/b]] PVE session stats | Zombies: [[b]]{1}[[/b]], Mega Zombies: [[b]]{2}[[/b]], Animals: [[b]]{3}[[/b]], Resources: [[b]]{4}[[/b]], Harvests: [[b]]{5}[[/b]], Fish: [[b]]{6}[[/b]]" />
  <Translation Id="SessionStatsCommandSyntax" Value="You must specify player name or steamID." />
  <Translation Id="SessionPlaytimeCommandSyntax" Value="You must specify player name or steamID." />
  <Translation Id="YourSessionPlaytime" Value="You have played for [[b]]{0}[[/b]] since you joined." />
  <Translation Id="OtherSessionPlaytime" Value="[[b]]{0}[[/b]] has played for [[b]]{1}[[/b]] since they joined." />
  <Translation Id="JoinMessage" Value="[[b]][#{0}] {1}[[/b]] joined the server." />
  <Translation Id="LeaveMessage" Value="[[b]][#{0}] {1}[[/b]] left the server." />
  <Translation Id="JoinMessageNoRank" Value="[[b]]{0}[[/b]] joined the server." />
  <Translation Id="LeaveMessageNoRank" Value="[[b]]{0}[[/b]] left the server." />
  <Translation Id="Day" Value="1 day" />
  <Translation Id="Days" Value="{0} days" />
  <Translation Id="Hour" Value="1 hour" />
  <Translation Id="Hours" Value="{0} hours" />
  <Translation Id="Minute" Value="1 minute" />
  <Translation Id="Minutes" Value="{0} minutes" />
  <Translation Id="Second" Value="1 second" />
  <Translation Id="Seconds" Value="{0} seconds" />
  <Translation Id="Zero" Value="a moment" />
  <Translation Id="UI_NextReward" Value="Next Reward: {0}" />
  <Translation Id="UI_RewardProgress" Value="{0}/{1} Kills" />
  <Translation Id="UI_Kills" Value="KILLS" />
  <Translation Id="UI_Deaths" Value="DEATHS" />
  <Translation Id="UI_Headshots" Value="HS" />
  <Translation Id="UI_Accuracy" Value="HS%" />
  <Translation Id="UI_Rank" Value="RANK" />
  <Translation Id="UI_KDR" Value="K/D" />
  <Translation Id="UI_Footer" Value="Use /statsui to hide" />
</Translations>
```