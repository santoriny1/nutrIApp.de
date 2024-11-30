using Microsoft.Extensions.Logging;
using nutrIApp.de.Services;
using nutrIApp.de.ViewModels;
using nutrIApp.de.Views;

namespace nutrIApp.de;

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
		
		builder.Services.AddSingleton<HomePageViewModel>();

		builder.Services.AddTransient<WelcomePage>();
		builder.Services.AddTransient<HomePage>();
		builder.Services.AddTransient<WorkOutPage>();
		builder.Services.AddTransient<ProgressPage>();
		builder.Services.AddTransient<ProfilePage>();
		builder.Services.AddTransient<RecipePage>();



		// builder.Services.AddTransient<MainPage>();

		OpenAIService svc = new OpenAIService();
		svc.Initialize(Constants.OpenAIKey);
		builder.Services.AddSingleton<OpenAIService>(svc);

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
