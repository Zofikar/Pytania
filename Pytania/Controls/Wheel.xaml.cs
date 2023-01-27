using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;

namespace Pytania.Controls;

public partial class Wheel : ContentView
{
	public static readonly BindableProperty RadiusProperty = BindableProperty.Create(nameof(Radius), typeof(int), typeof(Wheel), true);
    public int Radius
	{
		get
		{
			return (int)GetValue(RadiusProperty);
		}
		set
		{
			SetValue(RadiusProperty, value);
		}
    }
    public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(List<string>), typeof(Wheel), true);
    public List<string> Items
    {
        get
        {
            return (List<string>)GetValue(ItemsProperty);
        }
        set
        {
            SetValue(ItemsProperty, value);
            OnItemsChanged();
        }
    }
    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(Wheel), true);
    public string SelectedItem
    {
        get
        {
            return (string)GetValue(SelectedItemProperty);
        }
        set
        {
            SetValue(SelectedItemProperty, value);
        }
    }

    private static Random rnd = new Random();
    public Wheel(int radius)
	{
		this.Radius = radius;
        rnd.Next();
		InitializeComponent();
		HeightRequest= this.Radius *2;
		WidthRequest= this.Radius *2;
	}

    protected void OnItemsChanged()
    {
        Microsoft.Maui.Controls.Shapes.Path ItemView= new Microsoft.Maui.Controls.Shapes.Path();
        ItemView.Stroke = Brush.Black; ItemView.StrokeThickness = 1;
        ItemView.Fill = (Brush)(typeof(Brush).GetProperties()[rnd.Next(typeof(Brush).GetProperties().Length)].GetValue(null,null));
    }
}