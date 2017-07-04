using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Task_1.Startup))]
namespace Task_1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
