using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarsRent.Startup))]
namespace CarsRent
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
