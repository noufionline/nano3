using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Unity;
using PrismSampleApp.Services;
using PrismSampleApp.Views;
using System;
using System.Windows;
using Unity;
using Unity.Microsoft.DependencyInjection;

namespace PrismSampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterSingleton<IAlfrescoClient,AlfrescoClient>();

            IUnityContainer container = containerRegistry.GetContainer();

            var serviceCollection = new ServiceCollection();


            serviceCollection.AddHttpClient("zeon", client => client.BaseAddress = new Uri("http://192.168.30.179:8080/alfresco/api/"));
         

            serviceCollection.BuildServiceProvider(container);



        }
    }
}
