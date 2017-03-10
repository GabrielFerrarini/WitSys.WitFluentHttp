using System;

namespace WitSys.WitFluentHttp
{
    public class WitHttpClientFactory
    {
        private static readonly Lazy<WitHttpClientFactory> lazy = new Lazy<WitHttpClientFactory>(() => CreateInstanceOfT());

        public static WitHttpClientFactory Instance { get { return lazy.Value; } }

        /// <summary>
        /// Creates an instance of <see cref="{T}" via reflection since T's constructor is expected to be private./>
        /// </summary>
        /// <returns>An instance of <see cref="{T}"/></returns>
        private static WitHttpClientFactory CreateInstanceOfT()
        {
            return Activator.CreateInstance(typeof(WitHttpClientFactory)) as WitHttpClientFactory;
        }

        public IAddress Create()
        {
            IAddress retVal = new WitHttpClient();
            return retVal;
        }
    }
}
