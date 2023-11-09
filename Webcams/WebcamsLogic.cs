using Accord.Video;
using Accord.Video.DirectShow;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Permissions;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CameraTemp.Webcams
{
    public class WebcamsLogic
    {
        public MainWindow _MainWindow;

        private FilterInfoCollection _availableWebcams;

        private VideoCaptureDevice _webcam;

        public void FillWebcams(ComboBox comboBox)
        {
            _availableWebcams = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (var webcam in _availableWebcams)
            {
                comboBox.Items.Add(webcam.Name);
            }
        }

        public void SetFirstWebcam (ComboBox comboBox)
        {
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        public void StartWebcam(ComboBox comboBox)
        {
            _availableWebcams = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            _webcam = new VideoCaptureDevice(_availableWebcams[comboBox.SelectedIndex].MonikerString);
            _webcam.Start();
            _webcam.NewFrame += new NewFrameEventHandler(WebcamNewFrame);
        }

        public void StopWebcam()
        {
            if (_webcam != null && _webcam.IsRunning == true)
            {
                _webcam.SignalToStop();
                _webcam.NewFrame -= new NewFrameEventHandler(WebcamNewFrame);
                _webcam = null;
            }
        }

        public void WebcamNewFrame (object sender, NewFrameEventArgs newFrame)
        {
            _MainWindow.Dispatcher.Invoke(() =>
            {
                _MainWindow.Img.Source = BitmapToBitImage(newFrame.Frame);
            });
        }

        public BitmapImage BitmapToBitImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}
