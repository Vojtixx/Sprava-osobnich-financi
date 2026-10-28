using Sprava_osobnich_financi.Services;

namespace Sprava_osobnich_financi.Pages
{
    public partial class Settings : ContentPage
    {
        private bool _isLoadingTheme;

        public Settings()
        {
            InitializeComponent();
            SelectSavedTheme();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Application.Current is not null)
                Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;

            UpdateThemeStatus();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (Application.Current is not null)
                Application.Current.RequestedThemeChanged -= OnRequestedThemeChanged;
        }

        private void SelectSavedTheme()
        {
            _isLoadingTheme = true;

            switch (ThemeService.SavedTheme)
            {
                case AppTheme.Light:
                    LightThemeRadio.IsChecked = true;
                    break;
                case AppTheme.Dark:
                    DarkThemeRadio.IsChecked = true;
                    break;
                default:
                    SystemThemeRadio.IsChecked = true;
                    break;
            }

            _isLoadingTheme = false;
            UpdateThemeStatus();
        }

        private void OnThemeCheckedChanged(object? sender, CheckedChangedEventArgs e)
        {
            if (_isLoadingTheme || !e.Value)
                return;

            var theme = sender == LightThemeRadio ? AppTheme.Light
                      : sender == DarkThemeRadio ? AppTheme.Dark
                      : AppTheme.Unspecified;

            ThemeService.SetTheme(theme);
            UpdateThemeStatus();
        }

        private void OnRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e) =>
            MainThread.BeginInvokeOnMainThread(UpdateThemeStatus);

        private void UpdateThemeStatus()
        {
            var current = Application.Current?.RequestedTheme == AppTheme.Dark ? "tmavý" : "světlý";

            ThemeStatusLabel.Text = ThemeService.SavedTheme == AppTheme.Unspecified
                ? $"Řídí se nastavením telefonu – právě je aktivní {current} motiv."
                : $"Aktivní je {current} motiv. Volba zůstane uložená i po restartu aplikace.";
        }

        private void OnUsernameTextChanged(object? sender, TextChangedEventArgs e)
        {
        }

        private void OnPasswordTextChanged(object? sender, TextChangedEventArgs e)
        {
        }

        private void OnShowPasswordToggled(object? sender, ToggledEventArgs e)
        {
        }

        private void OnSaveCredentialsClicked(object? sender, EventArgs e)
        {
        }

        private void OnLoadCredentialsClicked(object? sender, EventArgs e)
        {
        }

        private void OnDeleteCredentialsClicked(object? sender, EventArgs e)
        {
        }
    }
}
