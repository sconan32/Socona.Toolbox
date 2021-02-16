using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Socona.Toolbox.Hosting
{
    public static class HostBuilderExtensions
    {
        //public static IHostBuilder ConfigureWpf(this IHostBuilder hostBuilder, Action<IWpfContext> configureAction = null)
        //{
        //    hostBuilder.ConfigureServices((hostBuilderContext, serviceCollection) =>
        //    {
        //        if (!TryRetrieveWpfContext(hostBuilder.Properties, out var wpfContext))
        //        {
        //            serviceCollection.AddSingleton(wpfContext);
        //            serviceCollection.AddHostedService<WpfHostedService>();
        //        }
        //        configureAction?.Invoke(wpfContext);
        //    });
        //    return hostBuilder;
        //}

    }
}
