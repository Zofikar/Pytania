﻿namespace Pytania;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);
		window.Width = 780;
		window.Height = 550;
		return window;
    }
}
