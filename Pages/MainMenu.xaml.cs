using CameraTemp.Webcams;
using System.Windows;
using System.Windows.Controls;
using static CameraTemp.Webcams.WebcamsLogic;

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
            FillWebcams(Combo);
            SetFirstWebcam(Combo);
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StopWebcam();
            StartWebcam(Combo);
        }

        private void Photo_Click(object sender, RoutedEventArgs e)
        {
            PhotoButton();
        }

        private void StartRec_Click(object sender, RoutedEventArgs e)
        {
            RecordStart();
        }
        private void StopRec_Click(object sender, RoutedEventArgs e)
        {
            RecordStop();
        }
    }
}
