using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eLifeWEB.Startup))]
namespace eLifeWEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        
            ConfigureAuth(app);
        }
    }
}
