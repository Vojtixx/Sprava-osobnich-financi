namespace Sprava_osobnich_financi.Pages
{
    public partial class ExchangeRatesPage : ContentPage
    {
        private const string AllowedHost = ".cnb.cz";

        public ExchangeRatesPage()
        {
            InitializeComponent();
        }

        private async void OnWebViewNavigating(object? sender, WebNavigatingEventArgs e)
        {
            if (new Uri(e.Url).Host.Contains(AllowedHost))
            {
                LoadingPanel.IsVisible = true;
                WebLoadingIndicator.IsRunning = true;
            }
            else
            {
                e.Cancel = true;
                await DisplayAlert("Chyba", "Přístup na tuto doménu není povolen.", "OK");
            }
        }

        private void OnWebViewNavigated(object? sender, WebNavigatedEventArgs e)
        {
            LoadingPanel.IsVisible = false;
            WebLoadingIndicator.IsRunning = false;
        }

        private void OnReloadClicked(object? sender, EventArgs e)
        {
            RatesWebView.Reload();
        }
    }
}
