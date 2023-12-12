using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static CameraTemp.Webcams.WebcamsLogic;

namespace CameraTemp.Pages
{
    /// <summary>
    /// Логика взаимодействия для WebcamImageFrame.xaml
    /// </summary>
    public partial class WebcamImage : Page
    {
        public WebcamImage()
        {
            InitializeComponent();
            Img = new Image();
            Img.Margin = new Thickness(10);
            scrollViewer.Content = Img;
            scaleTransform = new ScaleTransform();
            Img.LayoutTransform = scaleTransform;
        }
        private void ZoomChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ZoomLogic(sender, scaleTransform);
        }

        private void Zoom_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {

        }
    }
}
