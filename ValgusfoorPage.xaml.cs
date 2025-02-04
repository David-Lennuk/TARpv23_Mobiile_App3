namespace TARpv23_Mobiile_App
{
    public partial class ValgusfoorPage : ContentPage
    {
        private bool isOn = false;
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

            // Liiklustulede ringide loomine
            ring = new List<Frame>();
            StackLayout lightsStack = new StackLayout
            {
                Spacing = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            // Ringide loomine tsuklis
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

                // muudab päise vastavaks sõnumiks
                int index = i;
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (s, e) =>
                {
                    if (!isOn)
                    {
                        header.Text = "Kõigepealt lülitage valgusfoorid sisse";
                    }
                    else
                    {
                        header.Text = vastused[index];
                    }
                };
                frame.GestureRecognizers.Add(tapGesture);

                lightsStack.Children.Add(frame);
                ring.Add(frame);
            }

            // Liiklusvalgustusluliti
            Button onButton = new Button 
            { 
                Text = "Sisse" 
            };
            onButton.Clicked += (s, e) => TurnOn();

            // Liiklustulede valjalulitamise nupp
            Button offButton = new Button { Text = "Välja" };
            offButton.Clicked += (s, e) => TurnOff();

            // juhuslik varvivalik 
            Button randomButton = new Button 
            { 
                Text = "Juhuslik valik" 
            };
            randomButton.Clicked += (s, e) => ActivateRandomLight();

            StackLayout control = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20,
                Children = { onButton, offButton, randomButton }
            };

            // Vastusevoimalustega nupud
            Button btnPeatu = new Button 
            { 
                Text = "Peatu" 
            };
            btnPeatu.Clicked += (s, e) => CheckAnswer("Peatu");

            Button btnOota = new Button 
            { 
                Text = "Oota" 
            };
            btnOota.Clicked += (s, e) => CheckAnswer("Oota");

            Button btnMine = new Button 
            { 
                Text = "Mine" 
            };
            btnMine.Clicked += (s, e) => CheckAnswer("Mine");

            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 20,
                Children = { btnPeatu, btnOota, btnMine }
            };

            // Lehe peamine konteiner
            Content = new StackLayout
            {
                Spacing = 20,
                Padding = new Thickness(20),
                VerticalOptions = LayoutOptions.Center,
                Children = { header, lightsStack, control, stackLayout }
            };
        }

        // Liiklustulede sisselulitamise meetod 
        private void TurnOn()
        {
            isOn = true;
            header.Text = "Valgusfoor on sisse lülitatud. Vali reþiim.";
            RandomIndex = null;
            for (int i = 0; i < ring.Count; i++)
            {
                var box = (BoxView)ring[i].Content;
                box.Color = aktiivsed[i];
            }
        }

        // Liiklustulede valjalulitamise meetod 
        private void TurnOff()
        {
            isOn = false;
            header.Text = "Lülita esmalt valgusfoor sisse";
            RandomIndex = null;
            foreach (var frame in ring)
            {
                var box = (BoxView)frame.Content;
                box.Color = Colors.Gray;
            }
        }

        // Juhuslik varvireziim 
        private void ActivateRandomLight()
        {
            if (!isOn)
            {
                header.Text = "Lülita esmalt valgusfoor sisse";
                return;
            }

            // Valime juhusliku indeksi vahemikus 0 kuni 2
            int index = rnd.Next(0, 3);
            RandomIndex = index;
            header.Text = "Mis on õige vastus?";

            //valitud värv on aktiivne, teised on hallid
            for (int i = 0; i < ring.Count; i++)
            {
                var box = (BoxView)ring[i].Content;
                box.Color = (i == index) ? aktiivsed[i] : Colors.Gray;
            }
        }

        // Valitud vastuse kontrollimine
        private void CheckAnswer(string answer)
        {
            if (!isOn || RandomIndex == null)
            {
                header.Text = "Lülita esmalt valgusfoor sisse ja vali juhuslik reþiim";
                return;
            }
            // Kui vastus on sama, mis oige vastus praegusele varvile
            if (answer == vastused[RandomIndex.Value])
            {
                header.Text = "Õige!";
            }
            else
            {
                header.Text = "Vale!";
            }
        }
    }
}
