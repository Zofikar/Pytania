using Pytania.ViewModels;

namespace Pytania.Views;

public partial class LoadPage : ContentPage
{
    LoadPageViewModel viewModel;

    public LoadPage(LoadPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext= viewModel;
        this.viewModel= viewModel;
	}
}