using Ninject;
using RadiantBot.Logik.Domain.LoginManagement;
using RadiantBot.Logik.Domain.LoginManagement.Contract;

namespace RadiantBot.Infrastruktur.Bindings
{
    public class Mapper
    {

        public StandardKernel Initialize()
        {
            var kernel = new StandardKernel();

            kernel.Bind<ILoginManager>().To<LoginManager>();


            return kernel;
        }

    }
}