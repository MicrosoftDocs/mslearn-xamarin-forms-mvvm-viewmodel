using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GreatQuotes.Data
{
    public static class QuoteManager
    {
        public static IEnumerable<GreatQuote> Load()
        {
            IQuoteLoader loader = DependencyService.Get<IQuoteLoader>();
            if (loader == null)
                throw new Exception("Missing quote loader from platform.");

            return loader.Load();
        }

        public static void Save(IEnumerable<GreatQuote> quotes)
        {
            IQuoteLoader loader = DependencyService.Get<IQuoteLoader>();
            if (loader == null)
                throw new Exception("Missing quote loader from platform.");

            loader.Save(quotes);
        }
    }
}
