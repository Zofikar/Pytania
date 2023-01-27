using Pytania.ViewModels;

namespace Pytania.Views;

public partial class GamePage : ContentPage
{
	public GamePage()
	{
		InitializeComponent();
		BindingContext= new GamePageViewModel();
    }
}