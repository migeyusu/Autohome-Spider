using Car;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CarDisplay
{
    public class Installer:IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<MainWindow>()
                .LifestyleSingleton());
            container.Register(Component.For<MainWindowViewModel>()
                .LifestyleSingleton());
            container.Register(Component.For<CarContext>()
                .LifestyleSingleton());
        }
    }
}