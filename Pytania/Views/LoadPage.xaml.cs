using Pytania.ViewModels;

namespace Pytania.Views;

public partial class LoadPage : ContentPage
{

    public LoadPage(LoadPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext= viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BindingContext = new LoadPageViewModel();
    }
}