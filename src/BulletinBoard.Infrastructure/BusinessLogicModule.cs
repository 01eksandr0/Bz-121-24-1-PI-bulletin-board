using Autofac;
using BulletinBoard.BLL.Services;

namespace BulletinBoard.Infrastructure
{
    /// <summary>
    /// Реєстрація сервісів BLL. Усі класи *Service зі збірки BLL реєструються
    /// за своїми інтерфейсами, тому новий сервіс не потребує змін у цьому модулі.
    /// </summary>
    public class BusinessLogicModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(AdService).Assembly)
                .Where(type => type.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
