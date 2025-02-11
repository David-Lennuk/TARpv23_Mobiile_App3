namespace TARpv23_Mobiile_App;

public partial class StartPage : ContentPage
{
    public List<ContentPage> lehed = new List<ContentPage>() { new TextPage(0), new FigurePage(1), new ValgusfoorPage() };
    public List<string> tekstid = new List<string> { "Tee lahti TekstPage", "Tee lahti FigurePage", "Tee lahti ValgusfoorPage" };
    ScrollView sv;
    VerticalStackLayout vsl;
    public StartPage()
    {
        Title = "Avaleht";
        vsl = new VerticalStackLayout { BackgroundColor = Color.FromArgb("#800080") };
        for (int i = 0; i < tekstid.Count; i++)
        {
            Button nupp = new Button
            {
                Text = tekstid[i],
                BackgroundColor = Color.FromArgb("#8080ff"),
                TextColor = Color.FromArgb(" #000000f"),
                BorderWidth = 10,
                ZIndex = i,
                FontFamily = "Lothares 400",
                FontSize = 30
            };
            vsl.Add(nupp);
            nupp.Clicked += Lehte_avamine;
        }
        sv = new ScrollView { Content = vsl };
        Content = sv;

    }

    private async void Lehte_avamine(object? sender, EventArgs e)
    {
        Button btn = (Button)sender;
        await Navigation.PushAsync(lehed[btn.ZIndex]);
    }

    private async void Tagasi_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}