using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

/*

create asp.net project with no authentification

download reveal.js from https://github.com/hakimel/reveal.js/downloads

if IIS uses friendly-url, make it sure that the css and lib location starts with "/"


    
*/
namespace WebApplication1_revealjs
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}