using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TheDarkPath
{
    public partial class MainWindow : Window
    {
        readonly Dictionary<string, BitmapImage> resources = new Dictionary<string, BitmapImage>();

        void LoadResources()
        {
            string[] fileEntries = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Resources"));
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
    }
}
