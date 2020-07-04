using System;
using System.Globalization;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Tesseract;

namespace pc_parsing
{
    public class Bot
    {
        public static BotConfig Config;
        public static TesseractEngine Engine = new TesseractEngine(@"./", "eng", EngineMode.Default);
        
        private DiscordClient _client;
        private CommandsNextModule _commands;
        
        public Bot(BotConfig config)
        {
            Config = config;
        }
        
        public async Task RunAsync()
        {
            var disConfig = new DiscordConfiguration
            {
                Token = Config.ApiKey,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            _client = new DiscordClient(disConfig);

            _client.Ready += ClientReady;
            _client.ClientErrored += ClientError;

            var comConfig = new CommandsNextConfiguration
            {
                StringPrefix = Config.CommandPrefix,
                EnableDms = true,
                EnableMentionPrefix = true
            };

            _commands = _client.UseCommandsNext(comConfig);
            _commands.RegisterCommands<BasicCommands>();
            _commands.RegisterCommands<ParseCommands>();
            _commands.CommandErrored += CommandError;

            await _client.ConnectAsync();

            Console.WriteLine("Bot started successfully");
            await Task.Delay(-1);
        }

        private Task ClientReady(ReadyEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Info, Config.Application, "Client is ready.", DateTime.Now);
            return Task.CompletedTask;
        }

        private Task ClientError(ClientErrorEventArgs e)
        {
            e.Client.DebugLogger.LogMessage(LogLevel.Error, Config.Application, $"{e.Exception.GetType()}: {e.Exception.Message}", DateTime.Now);
            return Task.CompletedTask;
        }

        private Task CommandError(CommandErrorEventArgs e)
        {
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Error, Config.Application, 
                $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);
            Console.WriteLine(
                $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}");
            FailCommand(e);
            return Task.CompletedTask;
        }

        private async void FailCommand(CommandErrorEventArgs e)
        {
            DiscordEmbedBuilder failBuilder = new DiscordEmbedBuilder();
            failBuilder.WithAuthor($"{e.Context.Message.Author.Username}'s parse", null, e.Context.Message.Author.AvatarUrl);
            failBuilder.AddField("Parse status:", "Failed");
            failBuilder.AddField("Fail reason:", $"{e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}");
            DiscordMember amelia = await e.Context.Guild.GetMemberAsync(182991062011346945);
            failBuilder.WithFooter("© AmeliaX " + DateTime.Now.ToString(CultureInfo.InvariantCulture),
                amelia.AvatarUrl);
            await e.Context.Message.RespondAsync(null, false, failBuilder.Build());
        }
    }
}