using System.Windows.Shapes;

namespace WpfApp1
{
    public static class CellUtils
    {
        public static Rectangle CellToRectangle(Cell cell, DrawEngine de)
        {
            Rectangle r = new Rectangle
            {
                Width = de.GetWidth(),
                Height = de.GetHeight(),
                Fill = cell.State
            };

            return r;
        }
    }
}