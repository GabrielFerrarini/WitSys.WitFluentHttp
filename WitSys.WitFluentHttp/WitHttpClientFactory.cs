using System;

namespace WitSys.WitFluentHttp
{
    /// <summary>
    /// A factory class for the WitHttpClient class, returned as an IAddress interface
    /// </summary>
    public class WitHttpClientFactory
    {
        private static readonly Lazy<WitHttpClientFactory> lazy = new Lazy<WitHttpClientFactory>(() => CreateInstanceOfT());

        /// <summary>
        /// The static object to be used by all callers
        /// </summary>
        public static WitHttpClientFactory Instance { get { return lazy.Value; } }

        /// <summary>
        /// Creates an instance of WitHttpClientFactory via reflection since WitHttpClientFactory's constructor is expected to be private./>
        /// </summary>
        /// <returns>An instance of WitHttpClientFactory</returns>
        private static WitHttpClientFactory CreateInstanceOfT()
        {
            return Activator.CreateInstance(typeof(WitHttpClientFactory), true) as WitHttpClientFactory;
        }

        private WitHttpClientFactory() { }

        /// <summary>
        /// Creates an instance of WitHttpClient
        /// </summary>
        /// <returns></returns>
        public IAddress Create()
        {
            IAddress retVal = new WitHttpClient();
            return retVal;
        }
    }
}
