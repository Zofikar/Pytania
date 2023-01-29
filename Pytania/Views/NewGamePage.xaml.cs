using Pytania.ViewModels;

namespace Pytania.Views;

public partial class NewGamePage : ContentPage
{
	public NewGamePage(NewGamePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext= viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new NewGamePageViewModel();
    }
}