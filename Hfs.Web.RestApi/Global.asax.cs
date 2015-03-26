using System;
using System.Data.Services;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;

namespace Hfs.Web.RestApi
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// Registers the service routes for all WCF services in the project
        /// </summary>
        protected void RegisterRoutes(RouteCollection routes)
        {
            routes.Add(ServiceRouteWithTokens("user", new DataServiceHostFactory(), typeof(UserServices)));
        }

        /// <summary>
        /// Generates a service route with data tokens indicating the route endpoints
        /// </summary>
        /// <param name="routePrefix">The URL prefix for the service</param>
        /// <param name="serviceHostFactory">The host factory for the service</param>
        /// <param name="serviceType">The type of class that will handle the request</param>
        /// <returns></returns>
        protected ServiceRoute ServiceRouteWithTokens(string routePrefix, ServiceHostFactoryBase serviceHostFactory, Type serviceType)
        {
            ServiceRoute route = new ServiceRoute(routePrefix, serviceHostFactory, serviceType);
            route.DataTokens = new RouteValueDictionary();
            route.DataTokens.Add("RoutePrefix", routePrefix);
            route.DataTokens.Add("ServiceType", serviceType);
            return route;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}