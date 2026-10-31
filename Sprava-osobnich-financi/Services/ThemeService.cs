#if ANDROID
using AndroidX.Core.View;
using Microsoft.Maui.Platform;
#endif

namespace Sprava_osobnich_financi.Services
{
    public static class ThemeService
    {
        private const string PreferenceKey = "app_theme";

        // Motiv, který si uživatel naposledy vybral (výchozí = podle systému)
        public static AppTheme SavedTheme =>
            (AppTheme)Preferences.Default.Get(PreferenceKey, (int)AppTheme.Unspecified);

        // Volá se při startu aplikace – nastaví motiv uložený z minula
        public static void ApplySavedTheme()
        {
            if (Application.Current is not null)
                Application.Current.UserAppTheme = SavedTheme;
        }

        // Nastaví a uloží motiv. AppTheme.Unspecified = podle systému
        public static void SetTheme(AppTheme theme)
        {
            if (Application.Current is null)
                return;

            Application.Current.UserAppTheme = theme;
            Preferences.Default.Set(PreferenceKey, (int)theme);
            UpdateSystemBars();
        }

        // Obarví systémový stavový a navigační panel Androidu podle aktuálního motivu
        public static void UpdateSystemBars()
        {
#if ANDROID
            var window = Platform.CurrentActivity?.Window;
            if (window is null || Application.Current is null)
                return;

            var isDark = Application.Current.RequestedTheme == AppTheme.Dark;
            var statusBarColor = GetColor(isDark ? "SurfaceDark" : "SurfaceLight");
            var navigationBarColor = GetColor(isDark ? "SurfaceContainerDark" : "SurfaceContainerLight");

#pragma warning disable CA1422 // Od Androidu 15 zastaralé, na starších verzích ale stále potřeba
            window.SetStatusBarColor(statusBarColor.ToPlatform());
            window.SetNavigationBarColor(navigationBarColor.ToPlatform());
#pragma warning restore CA1422

            // Tmavé ikony (hodiny, baterie) na světlé liště a naopak
            if (WindowCompat.GetInsetsController(window, window.DecorView) is { } insetsController)
            {
                insetsController.AppearanceLightStatusBars = !isDark;
                insetsController.AppearanceLightNavigationBars = !isDark;
            }
#endif
        }

        private static Color GetColor(string key) =>
            Application.Current?.Resources.TryGetValue(key, out var value) == true && value is Color color
                ? color
                : Colors.Transparent;
    }
}
