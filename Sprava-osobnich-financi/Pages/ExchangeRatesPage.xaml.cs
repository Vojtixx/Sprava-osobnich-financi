namespace Sprava_osobnich_financi.Pages
{
    public partial class ExchangeRatesPage : ContentPage
    {
        private const string AllowedHost = "www.cnb.cz";

        public ExchangeRatesPage()
        {
            InitializeComponent();
        }

        private void OnWebViewNavigating(object? sender, WebNavigatingEventArgs e)
        {
        }

        private void OnWebViewNavigated(object? sender, WebNavigatedEventArgs e)
        {
        }

        private void OnReloadClicked(object? sender, EventArgs e)
        {
        }
    }
}
