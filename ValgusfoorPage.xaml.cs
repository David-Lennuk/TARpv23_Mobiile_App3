namespace TARpv23_Mobiile_App
{
    public partial class ValgusfoorPage : ContentPage
    {
        private bool isOn = false;
        private bool isAutoMode = false;
        private bool isFlashing = false;
        private Label header;
        private List<StackLayout> lights;  // List of StackLayouts to hold square lights
        private readonly List<Color> aktiivsed = new List<Color> { Colors.Red, Colors.Yellow, Colors.Green };
        private readonly List<string> vastused = new List<string> { "Peatu", "Oota", "Mine" };
        private readonly Random rnd = new Random();
        private int? RandomIndex = null;

        // Цвета для светофора из будущего
        private readonly List<Color> futureColors = new List<Color> { Colors.Purple, Colors.Cyan, Colors.Orange };

        public ValgusfoorPage()
        {
            Title = "Valgusfoor";
            header = new Label
            {
                Text = "Valgusfoor",
                FontSize = 24,
                HorizontalOptions = LayoutOptions.Center
            };

            lights = new List<StackLayout>();
            StackLayout lightsStack = new StackLayout
            {
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start
            };

            for (int i = 0; i < 3; i++)
            {
                var lightSquare = new BoxView
                {
                    Color = Colors.Gray,
                    HeightRequest = 100,
                    WidthRequest = 100,
                };

                var lightContainer = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Children = { lightSquare }
                };

                int index = i;
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (s, e) =>
                {
                    if (!isOn || isAutoMode || isFlashing)
                        return;

                    header.Text = vastused[index];
                    AnimateLight(lightContainer);
                };
                lightContainer.GestureRecognizers.Add(tapGesture);

                lightsStack.Children.Add(lightContainer);
                lights.Add(lightContainer);
            }

            Button onButton = new Button { Text = "Sisse" };
            onButton.Clicked += (s, e) => TurnOn();

            Button offButton = new Button { Text = "Välja" };
            offButton.Clicked += (s, e) => TurnOff();

            Button autoModeButton = new Button { Text = "Automaatne režiim" };
            autoModeButton.Clicked += (s, e) => StartAutoMode();

            // Кнопка "Светофор из будущего"
            Button futureButton = new Button { Text = "Svetofor from Future" };
            futureButton.Clicked += (s, e) => ChangeToFutureColors();

            StackLayout control = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20,
                Children = { onButton, offButton, futureButton }
            };

            Button btnPeatu = new Button { Text = "Värvi määratlus" };
            btnPeatu.Clicked += (s, e) => CheckAnswer("Värvi määratlus");

            StackLayout answerButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20,
                Children = { btnPeatu }
            };

            Content = new StackLayout
            {
                Spacing = 20,
                Padding = new Thickness(20),
                VerticalOptions = LayoutOptions.Center,
                Children = { header, lightsStack, control, autoModeButton, answerButtons }
            };
        }

        private void TurnOn()
        {
            if (isAutoMode || isFlashing)
                return;

            isOn = true;
            header.Text = "Valgusfoor on sisse lülitatud. Vali režiim";
            RandomIndex = null;
            for (int i = 0; i < lights.Count; i++)
            {
                var box = (BoxView)lights[i].Children[0];
                box.Color = aktiivsed[i];
            }
        }

        private void TurnOff()
        {
            isOn = false;
            isAutoMode = false;
            isFlashing = false;
            header.Text = "Lülita esmalt valgusfoor sisse";
            RandomIndex = null;
            foreach (var light in lights)
            {
                var box = (BoxView)light.Children[0];
                box.Color = Colors.Gray;
            }
        }

        private void ActivateRandomLight()
        {
            if (!isOn || isAutoMode || isFlashing)
                return;

            int index = rnd.Next(0, 3);
            RandomIndex = index;
            header.Text = "Mis on õige vastus?";

            for (int i = 0; i < lights.Count; i++)
            {
                var box = (BoxView)lights[i].Children[0];
                box.Color = (i == index) ? aktiivsed[i] : Colors.Gray;
            }
        }

        private async void StartAutoMode()
        {
            if (!isOn)
            {
                header.Text = "Lülitage esmalt valgusfoor põlema!";
                return;
            }

            isAutoMode = true;
            isFlashing = true;
            header.Text = "Auto Mode aktiivne!";

            while (isAutoMode)
            {
                for (int i = 0; i < 3; i++)
                {
                    ((BoxView)lights[i].Children[0]).Color = Colors.Gray;
                }

                if (isFlashing)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        ((BoxView)lights[j].Children[0]).Color = aktiivsed[j];
                        await Task.Delay(500);
                        ((BoxView)lights[j].Children[0]).Color = Colors.Gray;
                    }
                }
                else
                {
                    ((BoxView)lights[1].Children[0]).Color = aktiivsed[1];
                    await Task.Delay(500);
                    ((BoxView)lights[1].Children[0]).Color = Colors.Gray;
                }

                await Task.Delay(1000);
            }

            isFlashing = false;
        }

        private void CheckAnswer(string answer)
        {
            if (!isOn || RandomIndex == null)
            {
                header.Text = "Lülita esmalt valgusfoor sisse ja vali juhuslik režiim";
                return;
            }

            header.Text = answer == vastused[RandomIndex.Value] ? "Õige!" : "Vale!";
        }

        private async void AnimateLight(StackLayout lightContainer)
        {
            var box = (BoxView)lightContainer.Children[0];
            await box.ScaleTo(1.2, 100);
            lightContainer.BackgroundColor = Colors.White;
            await Task.Delay(200);
            await box.ScaleTo(1.0, 100);
            lightContainer.BackgroundColor = Colors.Transparent;
        }

        // Метод для изменения цветов на цвета из будущего
        private void ChangeToFutureColors()
        {
            if (!isOn)
                return;

            header.Text = "valgusfoor tulevikust: Tulevikuvärvid!";
            for (int i = 0; i < lights.Count; i++)
            {
                var box = (BoxView)lights[i].Children[0];
                box.Color = futureColors[i];
            }
        }
    }
}
 
