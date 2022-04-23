using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Attributes
{
    public class RequireRoleAttribute : PreconditionAttribute
    {
        private readonly string roleName;
        private readonly List<string> roles;

        public RequireRoleAttribute(string roleName)
        {
            this.roleName = roleName;
        }

        private RequireRoleAttribute(List<string> roles)
        {
            this.roles = roles;
        }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.User is SocketGuildUser user)
            {

                if (!string.IsNullOrEmpty(roleName))
                {

                    bool hasSpecificRole = await HasSpecificRole(user);

                    if (hasSpecificRole)
                    {
                        return await Task.FromResult(PreconditionResult.FromSuccess());
                    }
                    else
                    {
                        return await Task.FromResult(PreconditionResult.FromError("Du hast nicht die benötigten Rechte."));
                    }
                
                }

                bool hasRole = await HasRole(user);
                
                return hasRole ? await Task.FromResult(PreconditionResult.FromSuccess()) :  await Task.FromResult(PreconditionResult.FromError("Du hast nicht die benötigten Rechte."));


            }
            return await Task.FromResult(PreconditionResult.FromError("Du musst auf einem Server sein um diesen Command auszuführen"));

        }

        private async Task<bool> HasSpecificRole(SocketGuildUser user)
        {
            if (user.Roles.Any(x => x.Name == roleName))
            {
                return true;

            }
            else
            {
                user.SendMessageAsync("**Du hast nicht die benötigten Rechte**");
                return false;
            }
        }

        private async Task<bool> HasRole(SocketGuildUser user)
        {
            bool valid = false;

            foreach (var role in roles)
            {
                if (valid)
                {
                    break;
                }

                valid = user.Roles.Any(x => x.Name == role);
            }

            return valid;
        }
    }
}