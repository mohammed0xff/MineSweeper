using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace MineSweeper
{
    public enum CellType
    {
        Empty, Mine, Flagged
    }
    public enum CellState
    {
        Opened, Closed
    }

    public class Cell
    {

        public static readonly int CellSize = 25;
        // position in board [x][y]
        public int Board_X { get; }
        public int Board_y { get; }
        // drawing position on screen
        public int Screen_x { get; }
        public int Screen_y { get; }

        public int NumOfMines { get; set; }


        public CellState CellState { get; set; }
        public CellType CellType { get; set; }
        
        public Point CenterPos;
        // rectangle to draw the outside Box
        public readonly Rectangle Rect;

        // using lambda expersion to get the CellState
        public bool Opened => CellState == CellState.Opened;
        public bool Closed => CellState == CellState.Closed;
        public bool IsMine => CellType == CellType.Mine;

        public Cell(int x, int y, CellType celltype)
        {
            this.Board_X = x;
            this.Board_y = y;
            this.Screen_x = x * CellSize;
            this.Screen_y = y * CellSize;

            this.CenterPos = new Point(Screen_x + (CellSize / 2 - 10), Screen_y + (CellSize / 2 - 10));

            this.Rect = new Rectangle(Screen_x, Screen_y, CellSize, CellSize);

            this.CellState = CellState.Closed;
            this.CellType = celltype;
        }
    }
}
