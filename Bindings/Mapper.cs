
using Ninject;

using RadiantBot.CrossCutting.Logging;
using RadiantBot.Logik.Domain.ClientManagement;
using RadiantBot.Logik.Domain.ClientManagement.Contract;
using RadiantBot.Logik.Domain.CommandManagement;
using RadiantBot.Logik.Domain.CommandManagement.Contract;
using RadiantBot.Logik.Domain.LoginManagement;
using RadiantBot.Logik.Domain.LoginManagement.Contract;
using RadiantBot.CrossCutting.Logging.Contract;
using RadiantBot.Logik.Domain.CommandManagement.Modules;

namespace RadiantBot.Infrastruktur.Bindings
{
    public class Mapper
    {

        public StandardKernel Initialize()
        {
            var kernel = new StandardKernel();
 

            kernel.Bind<ILoginManager>().To<LoginManager>();
            kernel.Bind<ILogger>().To<Logger>();
            kernel.Bind<IClientManager>().To<ClientManager>();
            kernel.Bind<IClientFactory>().To<ClientFactory>();
            kernel.Bind<ICommandHandler>().To<CommandHandler>();
            kernel.Bind<IChannelLogger>().To<ChannelLogger>();


            return kernel;
        }


      
    }
}