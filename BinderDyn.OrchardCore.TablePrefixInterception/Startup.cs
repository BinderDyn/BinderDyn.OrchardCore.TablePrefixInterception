using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace BinderDyn.OrchardCore.TablePrefixInterception
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTablePrefixInterceptor(o =>
            {
                o.TableNamesToPrefix = new[] { "" };
            });
        }
    }
}