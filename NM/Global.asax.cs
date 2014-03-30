using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NM
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
           
            routes.MapRoute(
              name: "Home",
              url: "{controller}/{action}",
               defaults: new
               {
                   controller = "Home",
                   action = "index",
                   
               }
           );   
            
            // product Detail
            routes.MapRoute(
              name: "ProductDetail",
              url: "{controller}/{action}/{category_name}/{deal_name}/{id}",
               defaults: new
               {
                   controller = "Home",
                   action = "ProductDetail",
                   category_name = UrlParameter.Optional,
                   deal_name = UrlParameter.Optional,
                   id = UrlParameter.Optional
               }
           );

            routes.MapRoute
            ("ProductInCategory", "Home/ProductInCategory",
            defaults: new { 
                controller = "home",
                action= "ProductInCategory",
                category_alias = UrlParameter.Optional,
                category_aliasParrent = UrlParameter.Optional,
                id = UrlParameter.Optional
            }
            );

          

        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
    }
}