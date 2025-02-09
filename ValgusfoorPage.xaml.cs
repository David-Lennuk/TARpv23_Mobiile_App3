namespace TARpv23_Mobiile_App
{
    public partial class ValgusfoorPage : ContentPage
    {
        private bool isOn = false;
        private bool isAutoMode = false;
        private bool isFlashing = false;
        private Label header;
        private List<Frame> ring;
        private readonly List<Color> aktiivsed = new List<Color> { Colors.Red, Colors.Yellow, Colors.Green };
        private readonly List<string> vastused = new List<string> { "Peatu", "Oota", "Mine" };
        private readonly Random rnd = new Random();
        private int? RandomIndex = null;

        public ValgusfoorPage()
        {
            Title = "Valgusfoor";
            header = new Label
            {
                Text = "Valgusfoor",
                FontSize = 24,
                HorizontalOptions = LayoutOptions.Center
            };

            ring = new List<Frame>();
            StackLayout lightsStack = new StackLayout
            {
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start
            };

            for (int i = 0; i < 3; i++)
            {
                var box = new BoxView
                {
                    Color = Colors.Gray,
                    HeightRequest = 100,
                    WidthRequest = 100,
                    CornerRadius = 50
                };

                var frame = new Frame
                {
                    Padding = 0,
                    Content = box,
                    HasShadow = false,
                    BorderColor = Colors.Black,
                    CornerRadius = 50,
                    HeightRequest = 100,
                    WidthRequest = 100,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                };

                int index = i;
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (s, e) =>
                {
                    if (!isOn || isAutoMode || isFlashing)
                        return;

                    header.Text = vastused[index];
                    AnimateFrame(frame);
                };
                frame.GestureRecognizers.Add(tapGesture);

                lightsStack.Children.Add(frame);
                ring.Add(frame);
            }

            Button onButton = new Button { Text = "Sisse" };
            onButton.Clicked += (s, e) => TurnOn();

            Button offButton = new Button { Text = "Välja" };
            offButton.Clicked += (s, e) => TurnOff();

            Button randomButton = new Button { Text = "Juhuslik valik" };
            randomButton.Clicked += (s, e) => ActivateRandomLight();

            Button autoModeButton = new Button { Text = "Automaatne režiim" };
            autoModeButton.Clicked += (s, e) => StartAutoMode();

            StackLayout control = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20,
                Children = { onButton, offButton }
            };

            Button btnPeatu = new Button { Text = "Peatu" };
            btnPeatu.Clicked += (s, e) => CheckAnswer("Peatu");

            Button btnOota = new Button { Text = "Oota" };
            btnOota.Clicked += (s, e) => CheckAnswer("Oota");

            Button btnMine = new Button { Text = "Mine" };
            btnMine.Clicked += (s, e) => CheckAnswer("Mine");

            StackLayout answerButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20,
                Children = { btnPeatu, btnOota, btnMine }
            };

            Content = new StackLayout
            {
                Spacing = 20,
                Padding = new Thickness(20),
                VerticalOptions = LayoutOptions.Center,
                Children = { header, lightsStack, control, autoModeButton, randomButton, answerButtons }
            };
        }

        private void TurnOn()
        {
            if (isAutoMode || isFlashing)
                return;

            isOn = true;
            header.Text = "Valgusfoor on sisse lülitatud. Vali režiim.";
            RandomIndex = null;
            for (int i = 0; i < ring.Count; i++)
            {
                var box = (BoxView)ring[i].Content;
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
            foreach (var frame in ring)
            {
                var box = (BoxView)frame.Content;
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

            for (int i = 0; i < ring.Count; i++)
            {
                var box = (BoxView)ring[i].Content;
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
                    ((BoxView)ring[i].Content).Color = Colors.Gray;
                }

                if (isFlashing)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        ((BoxView)ring[j].Content).Color = aktiivsed[j];
                        await Task.Delay(500);
                        ((BoxView)ring[j].Content).Color = Colors.Gray;
                    }
                }
                else
                {
                    ((BoxView)ring[1].Content).Color = aktiivsed[1];
                    await Task.Delay(500);
                    ((BoxView)ring[1].Content).Color = Colors.Gray;
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

        private async void AnimateFrame(Frame frame)
        {
            await frame.ScaleTo(1.2, 100);
            frame.BorderColor = Colors.White;
            await Task.Delay(200);
            await frame.ScaleTo(1.0, 100);
            frame.BorderColor = Colors.Black;
        }
    }
}
