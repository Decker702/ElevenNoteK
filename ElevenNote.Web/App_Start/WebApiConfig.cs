using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace ElevenNote.Web

{
    public static class WebApiConfig//static so can call the methods off it directlyu
    {
        public static void Register()
        {// Below is bolilerpoint code, can use in other projects.
            GlobalConfiguration
                .Configure(
                    x =>
                    {
                        x
                            .Formatters
                            .JsonFormatter
                            .SupportedMediaTypes
                            .Add(new MediaTypeHeaderValue("text/html"));

                        x.MapHttpAttributeRoutes();
                    }
                );

        }

    }
}