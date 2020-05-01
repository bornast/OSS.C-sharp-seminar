using Autofac;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Application.Services;
using Module = Autofac.Module;

namespace Sindikat.Identity.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthService>().As<IAuthService>();
        }
    }
}
