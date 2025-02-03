namespace TARpv23_Mobiile_App;

public partial class ValgusfoorPage : ContentPage
{
    HorizontalStackLayout hsl;
    List<string> buttons = new List<string> { "Tagasi", "Avaleht", "Edasi" };
    private readonly Dictionary<string, Color> lightColors = new()
    {
        { "Red", Colors.Red },
        { "Yellow", Colors.Yellow },
        { "Green", Colors.Green }
    };

    private bool isOn = false;
    private List<BoxView> lights = new();
    private Label statusLabel;

    public ValgusfoorPage()
    {
        Title = "Valgusfoor";

        statusLabel = new Label
        {
            Text = "Светофор выключен",
            FontSize = 24,
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center
        };

        hsl = new HorizontalStackLayout { };
        for (int i = 0; i < 3; i++)
        {
            Button nupp = new Button
            {
                Text = buttons[i],
                ZIndex = i,
                WidthRequest = DeviceDisplay.Current.MainDisplayInfo.Width / 8.3,
            };

            hsl.Add(nupp);
            nupp.Clicked += Liikumine;
        }
        
        // Контейнер для светофора
        var trafficLightStack = new VerticalStackLayout { Spacing = 10 };

        // Создаем круги светофора через цикл
        foreach (var color in lightColors.Keys)
        {
            var light = new BoxView
            {
                WidthRequest = 100,
                HeightRequest = 100,
                CornerRadius = 50,
                BackgroundColor = Colors.Gray
            };

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += Light_Tapped;
            light.GestureRecognizers.Add(tapGesture);

            lights.Add(light);
            trafficLightStack.Children.Add(light);
        }

        // Кнопки управления
        var sisseButton = new Button
        {
            Text = "Sisse",
            FontSize = 18
        };
        sisseButton.Clicked += Sisse_Clicked;

        var valjaButton = new Button
        {
            Text = "Välja",
            FontSize = 18
        };
        valjaButton.Clicked += Välja_Clicked;

        var buttonStack = new HorizontalStackLayout
        {
            Spacing = 20,
            HorizontalOptions = LayoutOptions.Center,
            Children = { sisseButton, valjaButton }
        };

        // Финальный интерфейс
        Content = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 15,
            HorizontalOptions = LayoutOptions.Center,
            Children = { statusLabel, trafficLightStack, buttonStack }
        };
    }

    private void Sisse_Clicked(object sender, EventArgs e)
    {
        isOn = true;
        statusLabel.Text = "Светофор включен";
        UpdateLights();
    }

    private void Välja_Clicked(object sender, EventArgs e)
    {
        isOn = false;
        statusLabel.Text = "Светофор выключен";
        UpdateLights();
    }

    private void UpdateLights()
    {
        int i = 0;
        foreach (var light in lights)
        {
            light.BackgroundColor = isOn ? lightColors.ElementAt(i).Value : Colors.Gray;
            i++;
        }
    }

    private void Light_Tapped(object sender, TappedEventArgs e)
    {
        if (!isOn)
        {
            statusLabel.Text = "Сначала включи светофор";
            return;
        }

        var clickedLight = (BoxView)sender;
        if (clickedLight.BackgroundColor == Colors.Red)
        {
            statusLabel.Text = "Стой!";
        }
        else if (clickedLight.BackgroundColor == Colors.Yellow)
        {
            statusLabel.Text = "Жди!";
        }
        else if (clickedLight.BackgroundColor == Colors.Green)
        {
            statusLabel.Text = "Иди!";
        }
    }
    private async void Liikumine(object? sender, EventArgs e)
    {
        Button btn = (Button)sender;
        if (btn.ZIndex == 0)
        {
            await Navigation.PushAsync(new TextPage(btn.ZIndex));
        }
        else if (btn.ZIndex == 1)
        {
            await Navigation.PushAsync(new StartPage());
        }
        else if (btn.ZIndex == 2)
        {
            await Navigation.PushAsync(new Timer_Page());
        }
        else if (btn.ZIndex == 3)
        {
            await Navigation.PushAsync(new ValgusfoorPage());
        }
        else
        {
            await Navigation.PushAsync(new FigurePage(btn.ZIndex));
        }
    }

    private async void Tagasi_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}
