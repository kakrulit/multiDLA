using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private GameEngine gameEngine;
        private DrawEngine drawEngine;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            gameEngine = new GameEngine(width: 25, porosity: 250);
            drawEngine = new DrawEngine(game, gameEngine);
            drawEngine.Start();
        }
    }
}