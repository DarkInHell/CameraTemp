using CameraTemp.Pages;

namespace CameraTemp.Webcams
{
    public static class FrameManager
    {
        private static MainWindow _mainWindow;

        private static MainMenu _mainMenu;

        private static WebcamImage _webcamImage;

        private static FiltersMenu _filtersMenu;

        public static MainWindow MainWindowFrame()
        {
            if (_mainWindow == null)
            {
                _mainWindow = new MainWindow();
            }
            return _mainWindow;
        }
        public static MainMenu MainMenuFrame()
        {
            if ( _mainMenu == null)
            {
                _mainMenu = new MainMenu();
            }
            return _mainMenu;
        }
        public static WebcamImage WebcamImageFrame()
        {
            if (_webcamImage == null)
            {
                _webcamImage = new WebcamImage();
            }
            return _webcamImage;
        }
        public static FiltersMenu FiltersMenuFrame()
        {
            if (_filtersMenu == null)
            {
                _filtersMenu = new FiltersMenu();
            }
            return _filtersMenu;
        }
    }
}
