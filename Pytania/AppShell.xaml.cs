using Pytania.Views;

namespace Pytania;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Application.Current.UserAppTheme = AppTheme.Light;
		Routing.RegisterRoute("GamePage", typeof(GamePage));
	}
}
