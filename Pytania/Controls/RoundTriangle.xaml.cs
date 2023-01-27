namespace Pytania.Controls;

public partial class RoundTriangle : ContentView
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
            this.WidthRequest = Radius * 2;
            this.HeightRequest = Radius * 2;
        }
    }
    public static readonly BindableProperty ItemProperty = BindableProperty.Create(nameof(Item), typeof(List<string>), typeof(Wheel), true);
    public string Item
    {
        get
        {
            return (string)GetValue(ItemProperty);
        }
        set
        {
            SetValue(ItemProperty, value);
        }
    } 

    private static Random rnd = new Random();
    public RoundTriangle(int radius, string item)
    {
        this.Radius = radius;
        this.Item = item;
        InitializeComponent();
    }
}