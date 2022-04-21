using Discord.Commands;
using Discord.WebSocket;

namespace Attributes
{
    public class RequireRoleAttribute : PreconditionAttribute
    {
        private readonly string roleName;

        public RequireRoleAttribute(string roleName)
        {
            this.roleName = roleName;
        }

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            
            if(context.User is SocketGuildUser user)
            {
                if(user.Roles.Any(x => x.Name == roleName))
                {
                    return Task.FromResult(PreconditionResult.FromSuccess());
                }
                else
                {
                    return Task.FromResult(PreconditionResult.FromError("Du hast nicht die benötigten Rechte."));
                }
            }

            return Task.FromResult(PreconditionResult.FromError("Du musst auf einem Server sein um diesen Command auszuführen"));

        }
    }
}