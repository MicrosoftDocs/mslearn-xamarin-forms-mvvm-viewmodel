using Xamarin.Forms;
using GreatQuotes.ViewModels;

namespace GreatQuotes
{
    public partial class QuoteListPage : ContentPage
    {
        public QuoteListPage()
        {
            BindingContext = App.MainViewModel;
            InitializeComponent();
        }

        void OnQuoteSelected(object sender, ItemTappedEventArgs e)
        {
            QuoteViewModel quote = (QuoteViewModel)e.Item;
            Navigation.PushAsync(new QuoteDetailPage(quote), true);
        }
    }
}

