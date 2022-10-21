using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Invoices.Startup))]
namespace Invoices
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
