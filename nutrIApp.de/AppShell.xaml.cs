using nutrIApp.de.Views;

namespace nutrIApp.de;

public partial class AppShell : Shell
{
	public Dictionary<string, Type> Routes { get; private set; } = new Dictionary<string, Type>();

	public AppShell()
	{
		InitializeComponent();
		RegisterRoutes();
	}

	private void RegisterRoutes()
	{
		try
		{
			Routes.Add("welcomepage", typeof(WelcomePage));
			Routes.Add("recipepage", typeof(RecipePage));


			foreach (var item in Routes)
			{
				Routing.RegisterRoute(item.Key, item.Value);
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
	}
}
