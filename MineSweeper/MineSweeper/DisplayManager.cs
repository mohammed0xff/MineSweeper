using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace MineSweeper
{
    class DisplayManager
    {
        // fonts
        private readonly Font VerdanaFont;
        private readonly Font GenericSerifFont;
        
        // brushes 
        private Dictionary<int, SolidBrush> Brushes;

        // pens 
        private readonly Pen gridPen;
        private readonly Pen redPen;
        private readonly Pen bluePen;

        // images 
        private Image MineSprite;
        Rectangle MineSrcRect;

        private Image FlagSprite;
        Rectangle FlagSrcRect;


        public DisplayManager()
        {
            Brushes = new Dictionary<int, SolidBrush> {
                    { 0, new SolidBrush(ColorTranslator.FromHtml("0xffffff")) },
                    { 1, new SolidBrush(ColorTranslator.FromHtml("0x0000FE")) },
                    { 2, new SolidBrush(ColorTranslator.FromHtml("0x186900")) },
                    { 3, new SolidBrush(ColorTranslator.FromHtml("0xAE0107")) },
                    { 4, new SolidBrush(ColorTranslator.FromHtml("0x000177")) },
                    { 5, new SolidBrush(ColorTranslator.FromHtml("0x8D0107")) },
                    { 6, new SolidBrush(ColorTranslator.FromHtml("0x007A7C")) },
                    { 7, new SolidBrush(ColorTranslator.FromHtml("0x902E90")) },
                    { 8, new SolidBrush(ColorTranslator.FromHtml("0x000000")) }
                };
            VerdanaFont = new Font("Verdana", 16f, FontStyle.Bold);
            GenericSerifFont = new Font(
                FontFamily.GenericSerif, 17.0F, FontStyle.Bold, GraphicsUnit.Pixel
                );

            redPen = new Pen(Color.Red);
            bluePen = new Pen(Color.Blue);
            gridPen = bluePen;

            MineSprite = Image.FromFile("assets/mine.png");
            MineSrcRect = new Rectangle(0, 0, MineSprite.Height, MineSprite.Width);

            FlagSprite = Image.FromFile("assets/flag.png");
            FlagSrcRect = new Rectangle(0, 0, FlagSprite.Height, FlagSprite.Width);

        }


        public void DisplayBoard(ref Board board, Graphics g)
        {
            g.Clear(Color.White);
            g.TranslateTransform(Cell.CellSize / 2, Cell.CellSize * 2);

            for (int i = 0; i < board.Height; i++)
            {
                for (int j =0;j<board.Width;j++)
                {
                    var currCell = board.Cells[i, j];
                    DrawCell(ref currCell, g);
                    g.DrawRectangle(gridPen, currCell.Rect);
                }
            }

            if (board.HoveredCell != null)
            {
                g.DrawRectangle(redPen, board.HoveredCell.Rect) ;
            }
        }

        private Brush GetBackgroundBrush(CellState state)
        {
            if(state == CellState.Opened)
            {
               return System.Drawing.Brushes.LightGray;
            }
            return System.Drawing.Brushes.DarkGray;
        }


        public void DrawCell(ref Cell cell, Graphics g)
        {
            if (cell.Opened)
            {
                if (cell.NumOfMines > 0)
                {
                    g.DrawString(cell.NumOfMines.ToString(), VerdanaFont, Brushes[cell.NumOfMines % Brushes.Count], cell.CenterPos);
                }else if (cell.IsMine)
                {
                    g.DrawImage(MineSprite, cell.Rect, MineSrcRect, GraphicsUnit.Pixel);
                }
            }
            else
            {
                if (cell.CellType == CellType.Flagged)
                {
                    g.DrawImage(FlagSprite, cell.Rect ,FlagSrcRect, GraphicsUnit.Pixel);
                }
                else
                {
                    g.FillRectangle(GetBackgroundBrush(cell.CellState), cell.Rect);
                }
            }
        }

        public void DrawString(string str, Point pos, Graphics g)
        {
            g.DrawString(str, GenericSerifFont, Brushes[5], pos);
        }
    }
}
