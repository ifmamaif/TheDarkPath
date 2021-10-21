using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TheDarkPath
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Image player;
        bool sus = false, jos = false, dreapta = false, stanga = false;

        public MainWindow()
        {
            InitializeComponent();

            PreviewKeyDown += Window_PreviewKeyDown;
            PreviewKeyUp += Window_PreviewKeyUp;
            KeyUp += Window_PreviewKeyUp;
            Initialize();

            // Create a task but do not start it.
            Task t1 = new Task( MainLoop);
            t1.Start();
        }

        void MainLoop()
        {
            double dt = 0f;
            const double ELAPSED = 60f;
            while (true)
            {
                dt += GetDeltaTime();

                if (dt >= ELAPSED)
                {
                    dt -= ELAPSED;

                    Input();
                }
            }
        }

        void Input()
        {
            const double MISCARE = 0.005f;
            double[] newPosition = new double[2];
            if (stanga)
            {
                newPosition[0] -= MISCARE;
            }
            if (sus)
            {
                newPosition[1] -= MISCARE;
            }
            if (dreapta)
            {
                newPosition[0] += MISCARE;
            }
            if (jos)
            {
                newPosition[1] += MISCARE;
            }

            if (newPosition[0] != 0 || newPosition[1] != 0)
            {
                MovePlayer(newPosition[0], newPosition[1]);
            }
        }

        void MovePlayer(double left, double top)
        {
            Dispatcher.Invoke(() =>
            {
                player.Margin = new Thickness(player.Margin.Left + left, player.Margin.Top + top, player.Margin.Right, player.Margin.Bottom);
            });
        }

        static long lastTime = 0;
        static double GetDeltaTime()
        {
            long now = DateTime.Now.Ticks;
            double dT = (now - lastTime) / 1000;
            lastTime = now;
            Console.WriteLine(dT);
            return dT;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                    stanga = true;
                    break;
                case Key.W:
                    sus = true;
                    break;
                case Key.D:
                    dreapta = true;
                    break;
                case Key.S:
                    jos = true;
                    break;
                case Key.Space:
                    break;
                default:
                    break;
            }
            e.Handled = true;
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                    stanga = false;
                    break;
                case Key.W:
                    sus = false;
                    break;
                case Key.D:
                    dreapta = false;
                    break;
                case Key.S:
                    jos = false;
                    break;
                default:
                    break;
            }
            e.Handled = true;
        }
      

        void Initialize()
        {
            LoadResources();
            int width = 50;
            int height = 50;

            int n = 10,
                m = 10;
            double error = 1f;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Image _ = CreateSquareImage(error * i * width, error * j * height, "simple.bmp");
                }
            }

            player = CreateSquareImage(0, 0, "p.bmp");
        }

        Image CreateSquareImage(double left, double top, string textureName)
        {
            const int WIDTH = 50;
            const int HEIGHT = 50;

            Image image = new Image
            {
                Source = resources[textureName],
                Width = WIDTH,
                Height = HEIGHT,
                Margin = new Thickness(left, top, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stretch = Stretch.Fill,
                Visibility = Visibility.Visible,
            };

            gridMainWindow.Children.Add(image);

            return image;
        }

    }
}