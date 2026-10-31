namespace Sprava_osobnich_financi.Services
{
    // Shell navigace přes URI adresy na jednom místě
    // Pojistka _isNavigating zabrání tomu, aby rychlé dvojkliknutí otevřelo stejnou stránku dvakrát
    public static class NavigationService
    {
        private static bool _isNavigating;

        // Relativní trasa ("AddTransactionPage") otevře stránku navrch aktuální záložky,
        // absolutní ("///SettingsTab") přepne záložku, ".." se vrátí o stránku zpět.
        public static async Task GoToAsync(string route)
        {
            if (_isNavigating || Shell.Current is null)
                return;

            _isNavigating = true;
            try
            {
                await Shell.Current.GoToAsync(route);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Chyba navigace", $"Stránku se nepodařilo otevřít.\n{ex.Message}", "OK");
            }
            finally
            {
                _isNavigating = false;
            }
        }

        public static Task GoBackAsync() => GoToAsync("..");
    }
}
