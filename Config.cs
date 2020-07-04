using System.Collections.Generic;
using Newtonsoft.Json;

namespace pc_parsing
{
    public struct BotConfig
    {
        [JsonProperty("discordAppName")] public string Application;
        [JsonProperty("discordKey")] public string ApiKey;
        [JsonProperty("botName")] public string BotName;
        [JsonProperty("botUserID")] public ulong BotId;
        [JsonProperty("prefix")] public string CommandPrefix;
        [JsonProperty("staffRegex")] public string StaffRegex;
        [JsonProperty("realmeyeUserAgent")] public string UserAgent;
        [JsonProperty("classStats")] public Dictionary<string, ClassInfo> Classes;
        [JsonProperty("apiWaitTime")] public int SleepTimer;
    }

    public struct ClassInfo
    {
        [JsonProperty("name")] public string Name;
        [JsonProperty("att")] public int MaxAttack;
        [JsonProperty("dex")] public int MaxDexterity;
    }
}