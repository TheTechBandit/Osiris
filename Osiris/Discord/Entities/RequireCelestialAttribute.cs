using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Osiris;

// Inherit from PreconditionAttribute
public class RequireCelestialAttribute : PreconditionAttribute
{
    // Override the CheckPermissions method
    public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        if(UserHandler.GetUser(context.User.Id).Celestial)
        {
            return Task.FromResult(PreconditionResult.FromSuccess());
        }
        else
            return Task.FromResult(PreconditionResult.FromError("You must be a Celestial to use this command."));
    }
}