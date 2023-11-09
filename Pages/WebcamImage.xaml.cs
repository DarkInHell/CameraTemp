using CameraTemp.Webcams;
using System.Windows.Controls;

namespace CameraTemp.Pages
{
    /// <summary>
    /// Логика взаимодействия для WebcamImage.xaml
    /// </summary>
    public partial class WebcamImage : Page
    {
        public WebcamImage()
        {
            InitializeComponent();
            WebcamsLogic._WebcamImage = this;
        }
    }
}
