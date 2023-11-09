using Accord;
using CameraTemp.Pages;
using CameraTemp.Webcams;
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace CameraTemp
{
    public partial class MainWindow : Window
    {
        Uri uriMenu = new Uri("Pages/MainMenu.xaml", UriKind.RelativeOrAbsolute);
        Uri uriFilters = new Uri("Pages/FiltersMenu.xaml", UriKind.RelativeOrAbsolute);
        Uri uriImage = new Uri("Pages/WebcamImage.xaml", UriKind.RelativeOrAbsolute);
        public MainWindow()
        {
            InitializeComponent();
            WebcamsLogic._MainWindow = this;
            Menu.Navigate(uriFilters);
            Menu.Navigate(uriMenu);
            ImageFrame.Source = uriImage;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            WebcamsLogic.StopWebcam();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Menu.Navigate(uriMenu);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Menu.Navigate(uriFilters);
        }
    }
}
