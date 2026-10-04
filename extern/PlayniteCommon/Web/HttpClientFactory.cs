using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlayniteCommon.Web
{
    public class HttpClientFactory
    {
        private static Lazy<IHttpClientFactory> httpClientFactory = new Lazy<IHttpClientFactory>(InitHttpClientFactory, LazyThreadSafetyMode.PublicationOnly);


        private static IHttpClientFactory InitHttpClientFactory()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddHttpClient("HandleNoUseCookies")
                             .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseCookies = false });

            var serviceProvide = serviceCollection.BuildServiceProvider();

            return serviceProvide.GetService<IHttpClientFactory>();
        }

        public static HttpClient GetClient()
        {
            return httpClientFactory.Value.CreateClient();
        }
    }
}
