using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using Xamarin.Forms;
using GreatQuotes.Data;
#if WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_APP
using Windows.Storage;
#endif

[assembly:Dependency(typeof(QuoteLoader))]
namespace GreatQuotes.Data
{
    public class QuoteLoader : IQuoteLoader
    {
        private static string GetFileName()
        {
            const string fileName = "quotes.xml";
#if __IOS__
            string filename = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                "..", "Library", fileName);
#elif __ANDROID__
            string filename = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                fileName);
#elif WINDOWS_PHONE
            var path = global::Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            string filename = Path.Combine(path, fileName);
#elif WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_APP
            string filename = fileName;
#endif
            return filename;
        }


        public IEnumerable<GreatQuote> Load()
        {
            XDocument doc = null;

            string filename = GetFileName();
#if WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_APP
            StorageFolder local = global::Windows.Storage.ApplicationData.Current.LocalFolder;
            try 
            {
                Stream stream = local.OpenStreamForReadAsync(filename).Result;
                doc = XDocument.Load(stream);
            }
            catch 
            {
            }
#else
            if (File.Exists(filename)) {
                try 
                {
                    doc = XDocument.Load(filename);
                }
                catch 
                {
                }
            }
#endif

            if (doc == null)
                doc = XDocument.Parse(DefaultData);

            if (doc.Root != null) {
                foreach (var entry in doc.Root.Elements("quote"))
                {
                    yield return new GreatQuote(
                        entry.Attribute("fName").Value,
                        entry.Attribute("lName").Value,
                        (Gender) Enum.Parse(typeof (Gender), entry.Attribute("gender").Value),
                        entry.Value);
                }
            }
        }

        public void Save(IEnumerable<GreatQuote> quotes)
        {
            string filename = GetFileName();
#if WINDOWS_PHONE || __IOS__ || __ANDROID__
            if (File.Exists(filename))
                File.Delete(filename);
#endif

            XDocument doc = new XDocument(
                new XElement("quotes",
                    quotes.Select(q =>
                        new XElement("quote", 
                            new XAttribute("fName",  q.FirstName),
                            new XAttribute("lName",  q.LastName),
                            new XAttribute("gender", q.Gender.ToString()))
                        {
                            Value = q.QuoteText
                        })));

#if WINDOWS_PHONE_APP || WINDOWS_UWP || WINDOWS_APP
            System.Threading.Tasks.Task.Run(async () =>
            {
                var file = await global::Windows.Storage.ApplicationData
                    .Current.LocalFolder
                    .CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

                // write the char array created from the content string into the file
                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    doc.Save(stream);
                }
            });
#else
            doc.Save(new StreamWriter(filename));
#endif
        }

#region Internal Data
        static string DefaultData =
            @"<?xml version=""1.0"" encoding=""UTF-8""?>
<quotes>
    <quote gender=""Female"" fName=""Eleanor"" lName=""Roosevelt"">Great minds discuss ideas; average minds discuss events; small minds discuss people.</quote>
    <quote gender=""Male"" fName=""William"" lName=""Shakespeare"">Some are born great, some achieve greatness, and some have greatness thrust upon them.</quote>
    <quote gender=""Male"" fName=""Winston"" lName=""Churchill"">All the great things are simple, and many can be expressed in a single word: freedom, justice, honor, duty, mercy, hope.</quote>
    <quote gender=""Male"" fName=""Ralph"" lName=""Waldo Emerson"">Our greatest glory is not in never failing, but in rising up every time we fail.</quote>
    <quote gender=""Male"" fName=""William"" lName=""Arthur Ward"">The mediocre teacher tells. The good teacher explains. The superior teacher demonstrates. The great teacher inspires.</quote>
</quotes>";
#endregion
    }
}
