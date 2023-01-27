using Pytania.ViewModels;

namespace Pytania.Views;

public partial class NewGamePage : ContentPage
{
	public NewGamePage(NewGamePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext= viewModel;
	}
}