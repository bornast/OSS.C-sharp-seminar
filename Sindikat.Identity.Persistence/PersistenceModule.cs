using Autofac;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Domain.Entities;
using Sindikat.Identity.Persistence.Repository;
using System.Reflection;
using Module = Autofac.Module;

namespace Sindikat.Identity.Persistence
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterType<BaseRepository<Claim>>().As<IBaseRepository<Claim>>();
            builder.RegisterType<BaseRepository<User>>().As<IBaseRepository<User>>();
            builder.RegisterType<BaseRepository<RefreshToken>>().As<IBaseRepository<RefreshToken>>();
            builder.RegisterType<BaseRepository<Role>>().As<IBaseRepository<Role>>();
        }
    }
}
