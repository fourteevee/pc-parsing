using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using HtmlAgilityPack;
using Tesseract;

namespace pc_parsing
{
    /// <summary>
    /// Basic commands that don't have to do with parsing. Rules, ping, etc.
    /// </summary>
    [Description("Basic bot commands.")]
    public class BasicCommands
    {
        [Command("ping"), Description("Ping to get a response.")]
        public async Task Ping(CommandContext ctx)
        {
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

            DiscordMember bot = await ctx.Guild.GetMemberAsync(Bot.Config.BotId);
            string now = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            builder.WithFooter($"{Bot.Config.BotName} {now}", bot.AvatarUrl);
            builder.WithAuthor("Pong!", null, ctx.Member.AvatarUrl);
            builder.WithColor(new DiscordColor(70, 19, 188));

            await ctx.Message.RespondAsync(null, false, builder.Build());
        }

        [Command("raidrules"), Description("Shows the rules for raiding.")]
        public async Task RaidRules(CommandContext ctx)
        {
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

            builder.WithColor(new DiscordColor(132, 179, 201));
            builder.WithTitle("__**Raiding Rules**__");
            builder.AddField("\u200B",
                "It is your responsibility to read and understand all these rules while raiding; failing to read or understand them is not an excuse. If you think you were suspended incorrectly, please message a Security or Officer.",
                false);

            builder.AddField("Gear/Stat requirements include:", "-Maxed attack and dexterity\n"
                                                                + "-At least Tier 10 armors and weapons\n"
                                                                + "-At least Tier 4 rings and abilities\n", false);

            builder.AddField("Prohibited activities include:",
                "-Activating and killing bosses before the group can arrive\n"
                + "-Dragging mobs onto group or any other player(s)\n"
                + "-Calling server to those who aren't in the discord; this counts as crashing for both them as well as you, and may lead to an entire guild suspension as well.\n",
                false);

            builder.AddField("Fake reacting:",
                "Reacting with either a key or class and bringing neither will result in a hefty suspension.", false);
            builder.AddField("Crashing:",
                "Crashing is being in a run while suspended or not in voice chat. If you are suspended, please wait until your suspension is over. Entering the bazaar before server is called is prohibited unless you are key, scanner, leader, or permitted roles.",
                false);
            builder.AddField("Disrespect towards staff:",
                "Do not ask any staff to start a run, as they have their own lives. Pinging staff to start a run will result in you being muted. DMing staff on discord on in game to start a run will result in a suspension.",
                false);
            builder.AddField("Not being in Voice Chat:",
                "While in a run, you must be listening to the leader. You cannot leave vc, deafen your sound, or mute the raid leader. Doing so is considered crashing. If you die or nexus from the run, you are able to leave vc.",
                false);
            builder.AddField("Leeching:",
                "Going afk or repeatedly staying at the back of the group; if you need to go afk, you must tell a leader and they might acknowledge you.",
                false);

            await ctx.Message.RespondAsync(null, false, builder.Build());
            await ctx.Message.DeleteAsync();
        }

        [Command("faq"), Description("Shows the frequently asked questions.")]
        public async Task FAQ(CommandContext ctx)
        {
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

            builder.WithColor(new DiscordColor(132, 179, 201));
            builder.WithTitle("__**FaQ**__");
            builder.AddField("What is this server about?", "This server was created to do nests.", false);
            builder.AddField("What happened to the last server?",
                "The discord creator (one of the owners) sold the server without talking with the other owner, or letting someone know, or sharing the money. We are really sad about it, but we have to move on.",
                false);
            builder.AddField("Is there a raid up right now?",
                "Check #raid-status to see if there are any current runs. If the channel is empty, it means that there are no runs. Please be patient, as our leaders have their own lives as well!",
                false);
            builder.AddField("Why can't we have NSFW?",
                "NSFW Content is strictly forbidden, and under no circumstance should be posted in any public or private chat. Take it to DMs to do so.",
                false);
            builder.AddField("Why can't I post in #loots-and-oofs?",
                "Because of excessive conversation, only certain roles are allowed to post content there.", false);
            builder.AddField("Where do I apply for TRL or Security?",
                "TRL and Security applications are currently closed, but we will announce when they re-open, check #verified-announcements.",
                false);
            builder.AddField("Server Link?", "[Server Link](https://discord.gg/WWv2QHu)", false);

            await ctx.Message.RespondAsync(null, false, builder.Build());
            await ctx.Message.DeleteAsync();
        }

        [Command("rules"), Description("Shows the rules of the server.")]
        public async Task Rules(CommandContext ctx)
        {
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();

            builder.WithColor(new DiscordColor(132, 179, 201));
            builder.WithTitle("__**Rules**__");
            builder.AddField("1.  Toxicity/Hate Speech",
                "Excessive swears, racial slurs, threats, and deliberate attempts to instigate negative reactions will be removed. Skirting around offensive language --for example, saying \"commit rope on neck\" instead of \"kys\"-- is also prohibited. Genuinely friendly banter of this manner belongs in DMs. Avoid discussion about politics, religion, and other controversial topics when possible. Any of the above will lead to a mute/suspension/ban.",
                false);
            builder.AddField("2.  Spamming/Advertising",
                "Spamming, including ping spam, ghost pinging, emote spam, role pinging, etc. is prohibited and subject to a mute/suspension/ban. Advertising is not allowed and links will be auto-removed. Real world trading is also prohibited. Contact a higher-up for information on partnerships.",
                false);
            builder.AddField("3.  Channels",
                "Use each channel for its intended purpose (check pins or channel topics). NSFW/inappropriate content is not permitted, under no circumstance is posting such content allowed in any channel.",
                false);
            builder.AddField("4.  Bot Abuse",
                "Abusing our bot will get you banned. Please do not ask to apply too frequently, or attempt to dodge a suspension by leaving and rejoining the server. The bot keeps track, and you will be banned.",
                false);
            builder.AddField("5.  Behavior",
                "Remember to be respectful and kind to all members! This includes staff; if there is an issue and it has been addressed, abide accordingly. Ping staff only if necessary. A history of issues will be subject to mute/suspension/ban.",
                false);
            builder.AddField("6.  ToS",
                "Most importantly, do not break Discord Terms of Service. This includes any form of targeted harassment, hate speech, or illegal activities. Read more in the Official Discord ToS Page.",
                false);

            await ctx.Message.RespondAsync(null, false, builder.Build());
            await ctx.Message.DeleteAsync();
        }
    }

    [Description("Commands for parsing a list of raid participants.")]
    //[Hidden]
    //[RequirePermissions()]
    public class ParseCommands
    {
        private struct ParseData
        {
            public float Confidence;
            public string Text;

            public ParseData(float confidence, string text)
            {
                this.Confidence = confidence;
                this.Text = text;
            }
        }

        private struct FailReqData
        {
            public string Name;
            public string Requirement;
            public string Class;

            public FailReqData(string name, string requirement, string className)
            {
                Requirement = requirement;
                Name = name;
                Class = className;
            }
        }

        [Command("channels"),
         Description("Parse an image to see who is in the server but not in voice chat, and who is not in the server.")]
        public async Task Channels(CommandContext ctx)
        {
            if (!Bot.Config.Whitelist.Contains(ctx.Channel.Name))
                return;
            
            if (ctx.Message.Attachments.Count != 1)
            {
                FailParse(ctx, ctx.Message.Attachments.Count == 0 ? "No image attached." : "Too many images attached.");
                return;
            }

            string fileName = DownloadFile(ctx);

            //Parse the text in the attached image
            await ctx.TriggerTypingAsync();
            DateTime start = DateTime.Now;

            ParseData parsed = ParseImageTask(fileName);
            if (parsed.Text == null)
            {
                FailParse(ctx, "Image parse timed out.");
                return;
            }

            //Strip out "Players online (num): " and non-alphanumeric characters
            Regex nonAlphaNum = new Regex("[^a-zA-Z\\d,\\.\\s]+");
            parsed.Text = parsed.Text.Split(":")[1].Trim();
            parsed.Text = nonAlphaNum.Replace(parsed.Text, "");

            //Map a player's name to their discord name
            await ctx.TriggerTypingAsync();
            Dictionary<string, DiscordMember> playerToDiscord = GetDiscordMemberFromPlayerList(ctx, parsed.Text);

            //Get all the players in the run but not in voice chat
            List<string> notInVc = playerToDiscord.Where(pair => pair.Value != null && pair.Value.VoiceState == null)
                .Select(pair => pair.Value.Nickname ?? pair.Value.Username).ToList();
            //Get all the players in the run but not in the Discord
            List<string> notInServer =
                playerToDiscord.Where(pair => pair.Value == null).Select(pair => pair.Key).ToList();

            //Set up the response object
            await ctx.TriggerTypingAsync();
            DiscordEmbedBuilder builder = SetupBuilder(ctx, parsed.Confidence, string.Join(", ", playerToDiscord.Keys));

            //If there are players not in VC or not in Discord, display them. Otherwise, display "None"
            builder.AddField("Players in run but not in VC", notInVc.Count > 0 ? string.Join(", ", notInVc) : "None");
            builder.AddField("Players in run but not in Discord",
                notInServer.Count > 0 ? string.Join(", ", notInServer) : "None");

            //Done parsing, record the time it took.
            DateTime end = DateTime.Now;
            double timeElapsed = (end - start).TotalSeconds;
            builder.AddField("Time spent parsing",
                Math.Round(timeElapsed, 2).ToString(CultureInfo.InvariantCulture) + "s", false);

            //Send the message
            await ctx.Message.RespondAsync(null, false, builder.Build());
        }

        [Command("parse"),
         Description(
             "Parses an image to verify player stats, see who is in the server but not in voice chat, and who is not in the server.")]
        public async Task Parse(CommandContext ctx)
        {
            if (!Bot.Config.Whitelist.Contains(ctx.Channel.Name))
                return;

            if (ctx.Message.Attachments.Count != 1)
            {
                FailParse(ctx, ctx.Message.Attachments.Count == 0 ? "No image attached." : "Too many images attached.");
                return;
            }

            string fileName = DownloadFile(ctx);

            //Parse the text in the attached image
            await ctx.TriggerTypingAsync();
            DateTime start = DateTime.Now;

            ParseData parsed = ParseImageTask(fileName);
            if (parsed.Text == null)
            {
                FailParse(ctx, "Image parse timed out.");
                return;
            }

            //Strip out "Players online (num): " and non-alphanumeric characters
            Regex nonAlphaNum = new Regex("[^a-zA-Z\\d,\\.\\s]+");
            parsed.Text = parsed.Text.Split(":")[1].Trim();
            parsed.Text = nonAlphaNum.Replace(parsed.Text, "");

            //Map a player's name to their discord name
            await ctx.TriggerTypingAsync();
            Dictionary<string, DiscordMember> playerToDiscord = GetDiscordMemberFromPlayerList(ctx, parsed.Text);
            List<string> playersInDiscord = playerToDiscord.Keys.Where(key => playerToDiscord[key] != null).ToList();

            //Get the list of players not meeting requirements.
            await ctx.TriggerTypingAsync();
            var notMeetingReqs = ParseRequirements(playersInDiscord);

            //Get all the players in the run but not in voice chat
            List<string> notInVc = playerToDiscord.Where(pair => pair.Value != null && pair.Value.VoiceState == null)
                .Select(pair => pair.Value.Nickname ?? pair.Value.Username).ToList();
            //Get all the players in the run but not in the Discord
            List<string> notInServer =
                playerToDiscord.Where(pair => pair.Value == null).Select(pair => pair.Key).ToList();

            //Set up the response object
            await ctx.TriggerTypingAsync();
            DiscordEmbedBuilder builder = SetupBuilder(ctx, parsed.Confidence, string.Join(", ", playerToDiscord.Keys));

            builder.AddField("Players not meeting requirements:",
                notMeetingReqs.Count == 0 ? "None" : $"{notMeetingReqs.Count}", false);

            //If too many players are failing, we'll reach the max of 25 fields and throw an exception.
            //Just print all their names if this is the case.
            if (notMeetingReqs.Count > 19)
            {
                var failing = notMeetingReqs.Select(f => f.Name).ToList();
                builder.AddField("**Most players failing requirements:**", string.Join(", ", failing), true);
            }
            else
            {
                //Add fields for every player not meeting requirements
                foreach (var fail in notMeetingReqs)
                {
                    builder.AddField($"**{fail.Name}**", $"{fail.Class}\n {fail.Requirement}\n", true);
                }
            }

            //If there are players not in VC or not in Discord, display them. Otherwise, display "None"
            builder.AddField("Players in run but not in VC", notInVc.Count > 0 ? string.Join(", ", notInVc) : "None");
            builder.AddField("Players in run but not in Discord",
                notInServer.Count > 0 ? string.Join(", ", notInServer) : "None");

            //Done parsing, record the time it took.
            DateTime end = DateTime.Now;
            double timeElapsed = (end - start).TotalSeconds;
            builder.AddField("Time spent parsing",
                Math.Round(timeElapsed, 2).ToString(CultureInfo.InvariantCulture) + "s", false);

            //Send the message
            await ctx.Message.RespondAsync(null, false, builder.Build());
        }

        /// <summary>
        /// Takes a list of players and retrieves their DiscordMember object, if available.
        /// </summary>
        /// <param name="ctx">context of command</param>
        /// <param name="replace">string of names, comma separated</param>
        /// <returns>A dictionary of player name -> DiscordMember</returns>
        private Dictionary<string, DiscordMember> GetDiscordMemberFromPlayerList(CommandContext ctx, string replace)
        {
            Dictionary<string, DiscordMember> output = new Dictionary<string, DiscordMember>();

            //Take all the names and put them in the dictionary.
            Regex splitCritera = new Regex("[\\.\\s,]+");
            var names = splitCritera.Split(replace);

            foreach (string n in names)
            {
                //Occasionally the OCR won't pick up the comma, so break it up by spaces too.
                var spaceSplit = n.Split("\\s+");
                foreach (string spl in spaceSplit)
                {
                    output.Add(spl.ToLower(), null);
                }
            }

            //If a player is in the Discord, map their DiscordMember object to their player name
            foreach (DiscordMember m in ctx.Guild.Members)
            {
                if (m.IsBot) continue;

                //Use a user's nickname, if available 
                var name = m.Nickname ?? m.Username;

                //Parse out staff markings
                Regex staff = new Regex(Bot.Config.StaffRegex);
                name = staff.Replace(name, "").ToLower();
                if (output.ContainsKey(name))
                {
                    output[name] = m;
                }
            }

            return output;
        }

        /// <summary>
        /// Does all the common set up for an embed builder
        /// </summary>
        /// <param name="ctx">context to create builder under</param>
        /// <param name="confidence">confidence of the OCR</param>
        /// <param name="players">players detected by OCR</param>
        /// <returns></returns>
        private DiscordEmbedBuilder SetupBuilder(CommandContext ctx, float confidence, string players)
        {
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();
            builder.WithAuthor($"{ctx.Message.Author.Username}'s parse", null, ctx.Message.Author.AvatarUrl);
            builder.WithThumbnailUrl(ctx.Message.Attachments[0].Url);
            builder.AddField("Parse status:", "Succeeded");
            builder.AddField("Confidence", confidence.ToString(CultureInfo.InvariantCulture), false);
            builder.AddField("Players in the run:", players, false);
            DiscordMember bot = ctx.Guild.GetMemberAsync(Bot.Config.BotId).Result;
            builder.WithFooter($"{Bot.Config.BotName} {DateTime.Now.ToString(CultureInfo.InvariantCulture)}",
                bot.AvatarUrl);
            return builder;
        }

        /// <summary>
        /// Downloads a DiscordAttachment to disk
        /// </summary>
        /// <param name="ctx">context of the message attachment</param>
        /// <returns>name of the file downloaded</returns>
        private string DownloadFile(CommandContext ctx)
        {
            DiscordAttachment attachment = ctx.Message.Attachments[0];

            string filePath = attachment.Url;
            string fileName = Path.GetFileName(filePath);

            using WebClient wc = new WebClient();
            wc.DownloadFile(new Uri(filePath), fileName);
            wc.Dispose();
            return fileName;
        }

        /// <summary>
        /// Parses the text from an image
        /// </summary>
        /// <param name="file">The file to parse</param>
        /// <returns>The data retrieved during the parsing process</returns>
        private ParseData ParseImage(string file)
        {
            using var img = Pix.LoadFromFile(file);
            using var page = Bot.Engine.Process(img);
            return new ParseData(page.GetMeanConfidence(), page.GetText());
        }

        /// <summary>
        /// Parses an image, applying a timeout
        /// </summary>
        /// <param name="file">file to parse</param>
        /// <returns>a populated ParseData object if there is data, an empty one if no data</returns>
        private ParseData ParseImageTask(string file)
        {
            Task<ParseData> parseImage = Task<ParseData>.Factory.StartNew(() => ParseImage(file));
            int index = Task.WaitAny(new Task[] {parseImage}, TimeSpan.FromMinutes(1));
            File.Delete(file);

            return index == -1 ? new ParseData() : parseImage.Result;
        }

        /// <summary>
        /// Sets up a message for when the parse fails
        /// </summary>
        /// <param name="ctx">Context under which we're sending this failure message</param>
        /// <param name="reason"></param>
        private async void FailParse(CommandContext ctx, string reason)
        {
            DiscordEmbedBuilder failBuilder = new DiscordEmbedBuilder();
            failBuilder.WithAuthor($"{ctx.Message.Author.Username}'s parse", null, ctx.Message.Author.AvatarUrl);
            failBuilder.AddField("Parse status:", "Failed");
            failBuilder.AddField("Fail reason:", reason);
            DiscordMember bot = ctx.Guild.GetMemberAsync(182991062011346945).Result;
            failBuilder.WithFooter($"{Bot.Config.BotName} " + DateTime.Now.ToString(CultureInfo.InvariantCulture),
                bot.AvatarUrl);
            await ctx.Message.RespondAsync(null, false, failBuilder.Build());
        }

        /// <summary>
        /// Parses requirements for a list of players
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        private List<FailReqData> ParseRequirements(List<string> names)
        {
            List<FailReqData> output = new List<FailReqData>();
            Regex parseStats = new Regex("\\[\\d+,\\d+,(\\d+),\\d+,\\d+,\\d+,\\d+,(\\d+)]");
            Regex statMatch = new Regex("\\d+");
            foreach (string name in names)
            {
                //Wrapping this all into a giant try/catch is generally bad, but honestly I don't think we need the granularity. Fuck it.
                try
                {
                    //Grab the webpage for the user from RealmEye
                    using var client = new HttpClient();
                    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", Bot.Config.UserAgent);
                    var result = client.GetStringAsync("https://www.realmeye.com/player/" + name);
                    result.Wait();
                    client.Dispose();

                    //Validate that we can see the user's profile and characters
                    if (result.Result.Contains("Sorry, but we either:"))
                    {
                        output.Add(new FailReqData(name, "Player's RealmEye page was not found.", null));
                        continue;
                    }
                    else if (result.Result.Contains("Characters are hidden"))
                    {
                        output.Add(new FailReqData(name, "Characters appear to be hidden.", null));
                        continue;
                    }

                    //Load the document into the HTML parser
                    var document = new HtmlDocument();
                    document.LoadHtml(result.Result);

                    //Get the first character in the characters table
                    var table = document.DocumentNode.SelectSingleNode("//tbody");
                    if (table == null)
                    {
                        output.Add(new FailReqData(name, "No characters found.", null));
                        continue;
                    }

                    var equips = table.ChildNodes[0].ChildNodes[8];
                    var stats = table.ChildNodes[0].ChildNodes[9].ChildNodes[0];
                    string failReason = "";

                    if (equips.ChildNodes.Count < 4)
                    {
                        output.Add(new FailReqData(name, "Character does not have all the required items.", null));
                        continue;
                    }

                    //Verify that the items a character has equipped meet requirements
                    bool alt = false;
                    string[] slotNames = new[] {"Weapon", "Ability", "Armor", "Ring"};
                    for (int i = 0; i < 4; i++)
                    {
                        var equipName = equips.ChildNodes[i].ChildNodes[0].ChildNodes[0].Attributes["title"].Value
                            .Split(" ");
                        string level = equipName[^1];

                        if (level != "UT" && level != "ST")
                        {
                            var match = statMatch.Match(level);
                            if (match == Match.Empty)
                            {
                                failReason += $"Item in slot {i} does not have a level, assuming is backpack.";
                                continue;
                            }

                            int numericLevel = int.Parse(match.Value);
                            int min = alt ? 4 : 10;

                            if (numericLevel < min)
                            {
                                failReason +=
                                    $"**{slotNames[i]} is not meeting requirements: is T{numericLevel}, should be at least T{min}**\n";
                            }
                        }

                        //Alternate between checking for T4 and T10 items
                        alt = !alt;
                    }

                    //Validate that we have information on this class
                    if (!Bot.Config.Classes.ContainsKey(stats.Attributes["data-class"].Value))
                    {
                        output.Add(new FailReqData(name, "Please update config: new class added", null));
                        continue;
                    }

                    //Get the information about this class
                    ClassInfo info = Bot.Config.Classes[stats.Attributes["data-class"].Value];
                    var rawStats = parseStats.Match(stats.Attributes["data-stats"].Value);
                    var bonuses = parseStats.Match(stats.Attributes["data-bonuses"].Value);

                    //Get the player character's stats
                    if (!int.TryParse(rawStats.Groups[2].Value, out var dex) ||
                        !int.TryParse(rawStats.Groups[1].Value, out var att) ||
                        !int.TryParse(bonuses.Groups[2].Value, out var bDex) ||
                        !int.TryParse(bonuses.Groups[1].Value, out var bAtt))
                    {
                        output.Add(new FailReqData(name, "Failed to parse data.", null));
                        continue;
                    }

                    if (att - bAtt < info.MaxAttack || dex - bDex < info.MaxDexterity)
                    {
                        failReason += $"**ATT: {att}/{info.MaxAttack}\nDEX: {dex}/{info.MaxDexterity}**";
                    }

                    if (failReason.Length != 0)
                    {
                        output.Add(new FailReqData(name, failReason, info.Name));
                    }
                }
                catch (Exception e)
                {
                    //Basic catch to still parse when errors have occured. 
                    output.Add(new FailReqData(name, "Failed to parse data on player.", null));
                    Console.WriteLine($"Failed on player '{name}': {e}");
                }

                //Sleep, to avoid hitting rate limits
                Thread.Sleep(Bot.Config.SleepTimer);
            }

            return output;
        }
    }
}