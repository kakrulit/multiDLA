using System.Windows.Media;

namespace WpfApp1
{
    public class Cell
    {
        public double Speed { get; set; }
        public Direction Directions { get; set; }
        public SolidColorBrush State { get; set; }
        public Cell(Direction direct, double speed)
        {
            Directions = direct;
            Speed = speed;
        }
    }
}