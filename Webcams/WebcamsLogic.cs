using Accord.Video;
using Accord.Video.DirectShow;
using Accord.Video.FFMPEG;
using CameraTemp.Pages;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Accord.Imaging.Filters;
using System.Threading.Tasks;

namespace CameraTemp.Webcams
{
    class WebcamsLogic
    {
        public static MainWindow _MainWindow;

        public static MainMenu _MainMenu;

        public static WebcamImage _WebcamImage;

        public static FiltersMenu _FiltersMenu;

        private static FilterInfoCollection _availableWebcams;

        private static VideoCaptureDevice _webcam;

        private static bool _recording;

        private static DateTime? _firstFrameTime;

        private static VideoFileWriter writer;

        public static void FillWebcams(ComboBox comboBox)
        {
            _availableWebcams = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                foreach (var webcam in _availableWebcams)
                {
                    comboBox.Items.Add(webcam.Name);
                }
        }

        public static void SetFirstWebcam (ComboBox comboBox)
        {
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        public static void StartWebcam(ComboBox comboBox)
        {
            _availableWebcams = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            _webcam = new VideoCaptureDevice(_availableWebcams[comboBox.SelectedIndex].MonikerString);
            _webcam.Start();
            _webcam.NewFrame += new NewFrameEventHandler(WebcamNewFrame);
        }

        public static void StopWebcam()
        {
            if (_webcam != null && _webcam.IsRunning == true)
            {
                _webcam.SignalToStop();
                _webcam.NewFrame -= new NewFrameEventHandler(WebcamNewFrame);
                _webcam = null;
            }
        }

        public static void WebcamNewFrame (object sender, NewFrameEventArgs newFrame)
        {
            _WebcamImage.Dispatcher.Invoke(() =>
            {
                FirstFrameTime(newFrame);
                FiltersInvert(ref newFrame, _FiltersMenu.InvertCheck);
                _WebcamImage.Img.Source = BitmapToBitImage(newFrame.Frame);
            });
        }

        public static void FirstFrameTime (NewFrameEventArgs newFrame)
        {
            if (_recording)
            {
                if (_firstFrameTime != null)
                {
                    writer.WriteVideoFrame(newFrame.Frame, DateTime.Now - _firstFrameTime.Value);
                }
                else
                {
                    writer.WriteVideoFrame(newFrame.Frame);
                    _firstFrameTime = DateTime.Now;
                }
            }
        }

        public static void FiltersInvert (ref NewFrameEventArgs newFrame, CheckBox checkBox)
        {
            if (checkBox.IsChecked == true)
                {
                    Invert invert = new Invert();
                    invert.ApplyInPlace(newFrame.Frame);
                }

        }

        public static BitmapImage BitmapToBitImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        public static void PhotoButton()
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = "Snapshot1";
            dialog.DefaultExt = ".png";
            var dialogResult = dialog.ShowDialog();
            if (dialogResult != true)
            {
                return;
            }
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((System.Windows.Media.Imaging.BitmapSource)_WebcamImage.Img.Source));
            using (var fileStream = new FileStream(dialog.FileName, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        public static void RecordStart ()
        {
            var video = (System.Windows.Media.Imaging.BitmapSource)_WebcamImage.Img.Source;
            var videoDialog = new SaveFileDialog
            {
                FileName = "Video1",
                DefaultExt = ".avi",
                AddExtension = true
            };
            var videoDialogResult = videoDialog.ShowDialog();
            if (videoDialogResult != true)
            {
                return;
            }
            _firstFrameTime = null;
            writer = new VideoFileWriter();
            writer.Open(videoDialog.FileName, (int)Math.Round(video.Width, 0), (int)Math.Round(video.Height, 0));
            _recording = true;
        }

        public static void RecordStop ()
        {
            _recording = false;
            writer.Close();
        }

    }
}
