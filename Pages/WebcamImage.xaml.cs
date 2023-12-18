using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            Zoom.Value = 0;
            ZoomLogic(Zoom, scaleTransform);
        }
        private void ZoomChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ZoomLogic(sender, scaleTransform);
        }

        private void ScrollViewer_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            Point point = new Point();
            this.sfr.CenterX = point.X;
            this.sfr.CenterY = point.Y;
            if (sfr.ScaleX < 0.3 && sfr.ScaleY < 0.3 && e.Delta < 0)
            {
                return;
            }
            sfr.ScaleX += e.Delta;
            sfr.ScaleY += e.Delta;

        }
    }
}
