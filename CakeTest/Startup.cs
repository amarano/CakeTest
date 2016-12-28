using Owin;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;

namespace CakeTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            builder.UseWebApi(config);
            var fileSystem = new PhysicalFileSystem("./wwwroot");
            var options = new FileServerOptions
            {
                FileSystem = fileSystem,
                EnableDefaultFiles = true,
                DefaultFilesOptions = { DefaultFileNames = {"site/index.html"}}
            };

            builder.UseFileServer(options);
        }
    }
}