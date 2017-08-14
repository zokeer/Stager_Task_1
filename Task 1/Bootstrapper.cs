using System.Web.Http;

namespace Task_1
{
    public class Bootstrapper
    {

        public static void Run()
        {
            AutofacWebApiConfig.Initialize(GlobalConfiguration.Configuration);
        }

    }
}