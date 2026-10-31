using Sprava_osobnich_financi.Models;
using Sprava_osobnich_financi.Services;
using System.Text.Json;

namespace Sprava_osobnich_financi.Pages
{
    public partial class Settings : ContentPage
    {
        // Při předvyplnění přepínače z uloženého nastavení nechceme motiv znovu ukládat
        private bool _isLoadingTheme;

        private readonly string cesta = Path.Combine(FileSystem.AppDataDirectory, "UserCredentials.json");
        private bool jeJmeno = false;
        private bool jeHeslo = false;

        public Settings()
        {
            InitializeComponent();
            SelectSavedTheme();

            CredentialsStatusLabel.Text = File.Exists(cesta) ? "Údaje jsou uložené v zařízení." : "Zatím nic neuloženo.";
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
            // CheckedChanged chodí i od odznačeného tlačítka – reaguje jen na nově vybrané
            if (_isLoadingTheme || !e.Value)
                return;

            var theme = sender == LightThemeRadio ? AppTheme.Light
                      : sender == DarkThemeRadio ? AppTheme.Dark
                      : AppTheme.Unspecified;

            ThemeService.SetTheme(theme);
            UpdateThemeStatus();
        }

        // Při volbě "Systém" se motiv může změnit i zvenku (uživatel přepne tmavý režim v telefonu)
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
            UsernameErrorLabel.IsVisible = true;
            jeJmeno = false;

            string username = UsernameEntry.Text;

            if (username.Length < 4)
                UsernameErrorLabel.Text = "Jméno musí mít alespoň 4 znaky.";
            else if (username.Any(char.IsSymbol))
                UsernameErrorLabel.Text = "Jméno nesmí obsahovat symboly";
            else
            {
                UsernameErrorLabel.IsVisible = false;
                jeJmeno = true;
            }

            UsernameField.Style = (Style)Application.Current!.Resources[UsernameErrorLabel.IsVisible ? "InputFieldError" : "InputField"];
        }

        private void OnPasswordTextChanged(object? sender, TextChangedEventArgs e)
        {
            string heslo = PasswordEntry.Text;
            jeHeslo = false;

            bool delka = heslo.Length >= 8;
            bool cislice = heslo.Any(char.IsDigit);
            bool male = heslo.Any(char.IsLower);
            bool velke = heslo.Any(char.IsUpper);
            bool specialni = heslo.Any(char.IsSymbol) || heslo.Any(char.IsPunctuation);

            OznacPodminku(CriterionLengthLabel, delka);
            OznacPodminku(CriterionDigitLabel, cislice);
            OznacPodminku(CriterionLowerLabel, male);
            OznacPodminku(CriterionUpperLabel, velke);
            OznacPodminku(CriterionSpecialLabel, specialni);

            int body = 0;
            if (delka) body++;
            if (cislice) body++;
            if (male) body++;
            if (velke) body++;
            if (specialni) body++;

            PasswordStrengthBar.Progress = body / 5.0;

            if (heslo.Length == 0)
            {
                PasswordStrengthLabel.Text = "—";
                PasswordStrengthLabel.TextColor = BarvaMotivu("OnSurface");
            }
            else if (body <= 2)
            {
                PasswordStrengthLabel.Text = "Slabé";
                PasswordStrengthLabel.TextColor = BarvaMotivu("Error");
            }
            else if (body <= 4)
            {
                PasswordStrengthLabel.Text = "Střední";
                PasswordStrengthLabel.TextColor = BarvaMotivu("Warning");
            }
            else
            {
                PasswordStrengthLabel.Text = "Silné";
                PasswordStrengthLabel.TextColor = BarvaMotivu("Income");
                jeHeslo = true;
            }

            PasswordStrengthBar.ProgressColor = PasswordStrengthLabel.TextColor;
        }

        private void OznacPodminku(Label label, bool splneno)
        {
            label.Text = splneno ? label.Text.Replace('○', '✓') : label.Text.Replace('✓', '○');
            label.Style = (Style)Application.Current!.Resources[splneno ? "SuccessText" : "HintText"];
        }

        private static Color BarvaMotivu(string nazev)
        {
            string motiv = Application.Current!.RequestedTheme == AppTheme.Dark ? "Dark" : "Light";
            return (Color)Application.Current.Resources[nazev + motiv];
        }

        private void OnShowPasswordToggled(object? sender, ToggledEventArgs e)
        {
            PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        }

        private async void OnSaveCredentialsClicked(object? sender, EventArgs e)
        {
            SaveCredentialsButton.IsEnabled = false;

            if (jeJmeno && jeHeslo)
            {
                CredentialsActivityIndicator.IsVisible = true;
                CredentialsActivityIndicator.IsRunning = true;
                CredentialsStatusLabel.Text = "Ukládám…";

                try
                {
                    var udaje = new UserCredentials
                    {
                        Username = UsernameEntry.Text,
                        Password = PasswordEntry.Text,
                        SavedAt = DateTime.Now
                    };

                    string json = JsonSerializer.Serialize(udaje);
                    await File.WriteAllTextAsync(cesta, json);
                    await Task.Delay(1000);

                    CredentialsStatusLabel.Text = $"Uloženo {udaje.SavedAt:d. M. yyyy} v {udaje.SavedAt:HH:mm}.";
                    await DisplayAlert("Uloženo", "Přihlašovací údaje byly uloženy.", "OK");
                }
                catch (Exception ex)
                {
                    CredentialsStatusLabel.Text = "Uložení se nezdařilo.";
                    await DisplayAlert("Chyba", $"Nepodařilo se uložit údaje: {ex.Message}", "OK");
                }
                finally
                {
                    CredentialsActivityIndicator.IsRunning = false;
                    CredentialsActivityIndicator.IsVisible = false;
                }
            }
            else 
                await DisplayAlert("Chyba", "Zadejte platné jméno a heslo.", "OK");

            SaveCredentialsButton.IsEnabled = true;
        }

        private async void OnLoadCredentialsClicked(object? sender, EventArgs e)
        {
            LoadCredentialsButton.IsEnabled = false;

            try
            {
                if (!File.Exists(cesta))
                {
                    await DisplayAlert("Chyba", "Soubor nebyl nalezen", "OK");
                    return;
                }

                string json = await File.ReadAllTextAsync(cesta);
                UserCredentials? udaje = JsonSerializer.Deserialize<UserCredentials>(json);

                if (udaje == null)
                {
                    await DisplayAlert("Chyba", "Soubor s údaji je prázdný nebo poškozený.", "OK");
                    return;
                }

                UsernameEntry.Text = udaje.Username;
                PasswordEntry.Text = udaje.Password;
                CredentialsStatusLabel.Text = $"Načteno, uloženo {udaje.SavedAt:d. M. yyyy} v {udaje.SavedAt:HH:mm}.";

                await DisplayAlert("Načteno",
                        $"Jméno: {udaje.Username}\nHeslo: {udaje.Password}\nUloženo: {udaje.SavedAt:d. M. yyyy} v {udaje.SavedAt:HH:mm}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Chyba", $"Nepodařilo se načíst údaje: {ex.Message}", "OK");
            }
            finally
            {
                LoadCredentialsButton.IsEnabled = true;
            }
        }

        private async void OnDeleteCredentialsClicked(object? sender, EventArgs e)
        {
            DeleteCredentialsButton.IsEnabled = false;

            try
            {
                if (!File.Exists(cesta))
                {
                    await DisplayAlert("Chyba", "Soubor nebyl nalezen", "OK");
                    return;
                }

                bool smazat = await DisplayAlert("Smazat údaje?", "Uložené přihlašovací údaje budou nevratně smazány.", "Smazat", "Zrušit");
                if (!smazat)
                    return;

                File.Delete(cesta);
                CredentialsStatusLabel.Text = "Zatím nic neuloženo.";
                await DisplayAlert("Smazáno", "Přihlašovací údaje byly smazány.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Chyba", $"Nepodařilo se smazat údaje: {ex.Message}", "OK");
            }
            finally
            {
                DeleteCredentialsButton.IsEnabled = true;
            }
        }
    }
}
