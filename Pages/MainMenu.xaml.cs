using CameraTemp.Webcams;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CameraTemp.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public Pages.WebcamImage _WebcamImage;
        public MainMenu()
        {
            InitializeComponent();
            WebcamsLogic.FillWebcams(Combo);
            WebcamsLogic.SetFirstWebcam(Combo);
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WebcamsLogic.StopWebcam();
            WebcamsLogic.StartWebcam(Combo);
        }

        private void Photo_Click(object sender, RoutedEventArgs e)
        {
            WebcamsLogic.PhotoButton();
        }

        private void StartRec_Click(object sender, RoutedEventArgs e)
        {
            WebcamsLogic.RecordStart();
        }
        private void StopRec_Click(object sender, RoutedEventArgs e)
        {
            WebcamsLogic.RecordStop();
        }
    }
}
