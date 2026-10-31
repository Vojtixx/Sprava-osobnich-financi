using Microsoft.Extensions.Logging;

namespace Sprava_osobnich_financi
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            RemoveNativeInputUnderline();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        // Android kreslí pod Entry a Picker vlastní podtržení. Rámeček pole (Material 3 "outlined text field")
        // kreslí Border se stylem InputField, takže nativní podtržení schováme.
        private static void RemoveNativeInputUnderline()
        {
#if ANDROID
            var transparent = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, _) =>
                handler.PlatformView.BackgroundTintList = transparent);

            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("NoUnderline", (handler, _) =>
                handler.PlatformView.BackgroundTintList = transparent);
#endif
        }
    }
}
