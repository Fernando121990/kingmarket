using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MarketASP.Startup))]
namespace MarketASP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //QuestPDF.Settings.License = LicenseType.Community;
        }
    }
}
