using Discord;
using Discord.Commands;

namespace Osiris.Discord
{
    public static class CommandServConfig
    {
        public static CommandServiceConfig GetDefault()
        {
            return new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Verbose
            };
        }
    }
}