using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    public class DrawEngine
    {
        private GameEngine gameEngine;
        private readonly Random random = new Random();
        private readonly DispatcherTimer timer = new DispatcherTimer();
        public readonly Canvas canvas;
        private bool isn;
        private double speed;
        private Cell[,] matrix;
        public DrawEngine(Canvas canv, GameEngine Engine)
        {
            gameEngine = Engine;
            speed = gameEngine.GetCurrentCellSpeed();
            canvas = canv;
            matrix = gameEngine.GetMatrix();
            Draw();
            //CreateGrid();
            timer.Tick += UpdateCell;
        }

        public int GetWidth()
        {
            return gameEngine.getGameWidth();
        }

        public int GetHeight()
        {
            return gameEngine.getGameHight();
        }

        public void Start()
        {
            timer.Interval = TimeSpan.FromMilliseconds(1000 - Math.Ceiling(speed * 100));
            timer.Start();
        }

        private void UpdateCell(object sender, EventArgs e)
        {
            speed = gameEngine.GetCurrentCellSpeed();
            timer.Interval = TimeSpan.FromMilliseconds(1000 - Math.Ceiling(speed * 100));
            gameEngine.PlayGame();
            matrix = gameEngine.GetMatrix();
            Draw();
        }

        private void Draw()
        {
            canvas.Children.Clear();
            double cellWidth = canvas.ActualWidth / gameEngine.getGameWidth();
            double cellHeight = canvas.ActualHeight / gameEngine.getGameHight();

            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    if (matrix[x, y] != null)
                    {
                        var el = matrix[x, y];
                        var convetCell = CellUtils.CellToRectangle(el, this);

                        convetCell.Width = cellWidth;
                        convetCell.Height = cellHeight;

                        canvas.Children.Add(convetCell);

                        Canvas.SetLeft(convetCell, x * (cellWidth + 0.25));
                        Canvas.SetTop(convetCell, y * (cellHeight + 0.25));
                    }
                }
            }

            //CreateGrid();
            //canvas.Children.Clear();
            //double cellWidth = gameEngine.getGameWidth();
            //double cellHeight = gameEngine.getGameHight();

            //for (int x = 0; x < matrix.GetLength(0); x++)
            //{
            //    for (int y = 0; y < matrix.GetLength(1); y++)
            //    {
            //        if (matrix[x, y] != null)
            //        {
            //            var el = matrix[x, y];
            //            var convetCell = CellUtils.CellToRectangle(el, this);
            //            canvas.Children.Add(convetCell);

            //            double leftPosition = x * cellWidth;
            //            double topPosition = y * cellHeight;

            //            if (leftPosition % cellWidth < 2)
            //            {
            //                leftPosition = Math.Round(leftPosition / cellWidth) * cellWidth;
            //            }

            //            if (leftPosition % cellWidth > cellWidth - 2)
            //            {
            //                leftPosition = Math.Round(leftPosition / cellWidth) * cellWidth;
            //            }

            //            if (topPosition % cellHeight < 2)
            //            {
            //                topPosition = Math.Round(topPosition / cellHeight) * cellHeight;
            //            }

            //            if (topPosition % cellHeight > cellHeight - 2)
            //            {
            //                topPosition = Math.Round(topPosition / cellHeight) * cellHeight;
            //            }

            //            Canvas.SetLeft(convetCell, leftPosition);
            //            Canvas.SetTop(convetCell, topPosition);
            //        }
            //    }
            //}
        }

        private void CreateGrid()
        {
            var w = canvas.ActualWidth;
            var h = canvas.ActualHeight;

            canvas.Children.Clear();

            for (int x = 0; x <= w; x += gameEngine.getGameWidth())
            {
                AddLineToBackground(x, 0, x, h);
            }
            for (int y = 0; y <= h; y += gameEngine.getGameWidth())
            {
                AddLineToBackground(0, y, w, y);
            }
        }

        private void AddLineToBackground(double x1, double y1, double x2, double y2)
        {
            var line = new Line()
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                SnapsToDevicePixels = true
            };
            line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
            canvas.Children.Add(line);
        }
    }
}