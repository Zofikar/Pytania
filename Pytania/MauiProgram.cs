using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Pytania.Views;
using Pytania.ViewModels;
using Pytania.Services;

namespace Pytania;

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
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		//Views
		builder.Services.AddSingleton<NewGamePage>();
		builder.Services.AddTransient<LoadPage>();
        //ViewModels
        builder.Services.AddSingleton<NewGamePageViewModel>();
        builder.Services.AddTransient<LoadPageViewModel>();


        return builder.Build();
	}
}
