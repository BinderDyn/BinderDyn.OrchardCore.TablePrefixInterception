using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;

namespace BinderDyn.OrchardCore.TablePrefixInterception;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddTablePrefixInterceptor(this IServiceCollection serviceCollection,
        Action<TablePrefixInterceptionOptions> configure)
    {
        var shellSettings = serviceCollection.BuildServiceProvider().GetRequiredService<ShellSettings>();

        var options = new TablePrefixInterceptionOptions();
        
        configure(options);

        serviceCollection.AddScoped<TablePrefixInterceptor>(x => new TablePrefixInterceptor(shellSettings, options.TableNamesToPrefix));

        return serviceCollection;
    }
}