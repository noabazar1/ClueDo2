using ClueDo.Services;
using ClueDo.ViewModels;
using ClueDo.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace ClueDo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialSymbolsOutlined.ttf", "MaterialSymbols");
                    fonts.AddFont("Sigmar-Regular.ttf", "ClueFont");
                });
            builder.Services.AddTransient<FriendsPageVM>();
            builder.Services.AddSingleton<IContactsService, ContactsService>();
            builder.Services.AddTransient<FriendsPage>();
            builder.Services.AddSingleton<IFriendsService, FriendsService>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
