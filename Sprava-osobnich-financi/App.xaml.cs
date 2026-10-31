using Sprava_osobnich_financi.Services;

namespace Sprava_osobnich_financi
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Motiv vybraný v Nastavení (uložený v Preferences) se použije hned po spuštění
            ThemeService.ApplySavedTheme();

            // Při změně motivu (i systémového, když je zvoleno "Podle systému") přebarvíme systémové lišty
            RequestedThemeChanged += (_, _) => MainThread.BeginInvokeOnMainThread(ThemeService.UpdateSystemBars);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new AppShell());

            // Systémové lišty přebarvíme při každém vytvoření i obnovení okna –
            // i když se aplikace po tlačítku Zpět otevře znovu (nové okno ve stejném procesu)
            window.Created += (_, _) => ThemeService.UpdateSystemBars();
            window.Resumed += (_, _) => ThemeService.UpdateSystemBars();

            return window;
        }
    }
}
