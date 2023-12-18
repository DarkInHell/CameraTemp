using Accord.Imaging.Filters;
using Accord.Video;
using Accord.Video.DirectShow;
using Accord.Video.FFMPEG;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static CameraTemp.Webcams.FrameManager;

namespace CameraTemp.Webcams
{
    class WebcamsLogic
    {

        public static ScaleTransform scaleTransform;

        private static FilterInfoCollection _availableWebcams;

        public static VideoCaptureDevice _webcam;

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

        public static void SetFirstWebcam(ComboBox comboBox)
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

        public static void WebcamNewFrame(object sender, NewFrameEventArgs newFrame)
        {
            var bitmap = newFrame.Frame;
            WebcamImageFrame().Dispatcher.Invoke(() =>
            {
                FirstFrameTime(newFrame);

                FiltersApply(newFrame, FiltersMenuFrame().InvertCheck, new Invert(), out bitmap);

                FiltersApply(newFrame, FiltersMenuFrame().BlackWhite, Grayscale.CommonAlgorithms.BT709, out bitmap);

                FiltersApply(bitmap, FiltersMenuFrame().RotationX);

                FiltersApply(bitmap, FiltersMenuFrame().RotationY);

                WebcamImageFrame().Img.Source = BitmapToBitImage(bitmap);
            });
        }
        public static void ZoomLogic(object sender, ScaleTransform scale)
        {
            if (_webcam != null)
            {
                Binding bindx = new Binding();
                bindx.Source = sender as Slider;
                bindx.Path = new PropertyPath("Value");
                BindingOperations.SetBinding(scale, ScaleTransform.ScaleXProperty, bindx);
                Binding bindy = new Binding();
                bindy.Source = sender as Slider;
                bindy.Path = new PropertyPath("Value");
                BindingOperations.SetBinding(scale, ScaleTransform.ScaleYProperty, bindy);
            }
        }

        public static void FirstFrameTime(NewFrameEventArgs newFrame)
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

        private static void FiltersApply(Bitmap newFrame, CheckBox checkBox)
        {
            if (checkBox.IsChecked == true && checkBox.Name.Last() == 'X')
            {
                newFrame.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            if (checkBox.IsChecked == true && checkBox.Name.Last() == 'Y')
            {
                newFrame.RotateFlip(RotateFlipType.RotateNoneFlipY);
            }

        }

        private static void FiltersApply(NewFrameEventArgs newFrame, CheckBox checkBox, IFilter baseFilter, out Bitmap bitmap)
        {
            if (checkBox.IsChecked == true)
            {
                bitmap = baseFilter.Apply(newFrame.Frame);
            }
            else
            {
                bitmap = newFrame.Frame;
            }
        }
        public static void FiltersApply(NewFrameEventArgs newFrame, CheckBox checkBox, BaseInPlacePartialFilter filter, out Bitmap bitmap)
        {
            if (checkBox.IsChecked == true)
            {
                filter.ApplyInPlace(newFrame.Frame);
                bitmap = newFrame.Frame;
            }
            else
            {
                bitmap = newFrame.Frame;
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
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)WebcamImageFrame().Img.Source));
            using (var fileStream = new FileStream(dialog.FileName, FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        public static void RecordStart()
        {
            var video = (BitmapSource)WebcamImageFrame().Img.Source;
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

        public static void RecordStop()
        {
            _recording = false;
            writer.Close();
        }

        public static void Wheel()
        {
            if (WebcamImageFrame() != null)
            {
                if (WebcamImageFrame().Cursor != null) 
                {
                }
            }
        }
    }
}
