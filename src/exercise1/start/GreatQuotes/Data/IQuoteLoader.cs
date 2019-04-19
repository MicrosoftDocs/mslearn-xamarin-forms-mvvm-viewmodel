using System.Collections.Generic;

namespace GreatQuotes.Data
{
    public interface IQuoteLoader
    {
        IEnumerable<GreatQuote> Load();
        void Save(IEnumerable<GreatQuote> quotes);
    }
}
