using Accord.Video.DirectShow;
using CameraTemp.Webcams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static CameraTemp.Webcams.WebcamsLogic;

namespace CameraTemp
{
    public partial class MainWindow : Window
    {
        private readonly WebcamsLogic WebcamsLogic = new WebcamsLogic();

        public MainWindow()
        {
            InitializeComponent();
            WebcamsLogic.FillWebcams(Combo);
            WebcamsLogic.SetFirstWebcam(Combo);
            WebcamsLogic._MainWindow = this;
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WebcamsLogic.StopWebcam();
            WebcamsLogic.StartWebcam(Combo);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            WebcamsLogic.StopWebcam();
        }

    }
}
