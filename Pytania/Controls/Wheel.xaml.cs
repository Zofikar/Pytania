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
            this.WidthRequest= Radius*2;
            this.HeightRequest= Radius*2;
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
    }

    protected void OnItemsChanged()
    {
    }
}