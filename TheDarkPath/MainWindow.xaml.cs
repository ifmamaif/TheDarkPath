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
        readonly Dictionary<string, BitmapImage> resources = new Dictionary<string, BitmapImage>();
        bool sus = false, jos = false, dreapta = false, stanga = false;
        bool taste = false;

        public MainWindow()
        {
            InitializeComponent();

            PreviewKeyDown += Window_PreviewKeyDown;
            PreviewKeyUp += Window_PreviewKeyUp;
            KeyUp += Window_KeyUp;
            Initialize();

            // Create a task but do not start it.
            Task t1 = new Task( MainLoop);
            t1.Start();
        }

        void MainLoop()
        {
            double miscare = 0.005f;
            double dt = 0f;
            double elapsed = 60f;
            while (true)
            {
                dt += GetDeltaTime();

                if (dt >= elapsed)
                {
                    dt -= elapsed;


                    if (taste)
                    {
                        continue;
                    }

                    if (stanga)
                    {
                        MovePlayer(-miscare, 0);
                    }
                    if (sus)
                    {
                        MovePlayer(0, -miscare);
                    }
                    if (dreapta)
                    {
                        MovePlayer(miscare, 0);
                    }
                    if (jos)
                    {
                        MovePlayer(0, miscare);
                    }
                }

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

        void ClearKeyBoard()
        {
            sus = jos = dreapta = stanga = false;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            taste = true;
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

            if (e.Key == Key.Enter)
            {
                //Process user input
                e.Handled = true;
            }
            e.Handled = true;
            taste = false;
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            taste = true;
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
            taste = false;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            taste = true;
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
            taste = false;
        }


        void LoadResources()
        {
            string[] fileEntries = Directory.GetFiles(System.IO.Path.Combine(Environment.CurrentDirectory, "Resources"));
            foreach (string fileName in fileEntries)
            {
                AddResource(fileName);
            }
        }

        void AddResource(string path)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            Uri pathToFile = new Uri(path);
            bitmap.UriSource = pathToFile;
            string fileName = pathToFile.Segments[^1];
            bitmap.EndInit();
            resources.Add(fileName, bitmap);
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
                    Image image = new Image
                    {
                        Source = resources["simple.bmp"],
                        Width = width,
                        Height = height,
                        Margin = new Thickness(error * i * width, error * j * height, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                    image.Stretch = Stretch.Fill;
                    image.Visibility = Visibility.Visible;
                    gridMainWindow.Children.Add(image);
                }
            }

            player = new Image
            {
                Source = resources["p.bmp"],
                Width = width,
                Height = height,
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Stretch = Stretch.Fill,
                Visibility = Visibility.Visible,
            };

            gridMainWindow.Children.Add(player);
        }


    }
}