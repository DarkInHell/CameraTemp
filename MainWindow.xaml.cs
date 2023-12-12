using System;
using System.Windows;
using static CameraTemp.Webcams.FrameManager;
using static CameraTemp.Webcams.WebcamsLogic;

namespace CameraTemp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //FillWebcams(MainMenuFrame().Combo);
            ImageFrame.NavigationService.Navigate(WebcamImageFrame());
            Menu.NavigationService.Navigate(MainMenuFrame());
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            StopWebcam();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Menu.NavigationService.Navigate(MainMenuFrame());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Menu.NavigationService.Navigate(FiltersMenuFrame());
        }

    }
}
