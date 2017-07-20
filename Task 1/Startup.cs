using Microsoft.Owin;
using Owin;
using Task_1;

[assembly: OwinStartup(typeof(Startup))]
namespace Task_1
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
