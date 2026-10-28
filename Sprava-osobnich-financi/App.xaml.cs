using Sprava_osobnich_financi.Services;

namespace Sprava_osobnich_financi
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ThemeService.ApplySavedTheme();

            RequestedThemeChanged += (_, _) => MainThread.BeginInvokeOnMainThread(ThemeService.UpdateSystemBars);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override void OnStart()
        {
            base.OnStart();
            ThemeService.UpdateSystemBars();
        }

        protected override void OnResume()
        {
            base.OnResume();
            ThemeService.UpdateSystemBars();
        }
    }
}
