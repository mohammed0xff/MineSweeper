using System;
using System.Windows.Forms;

namespace MineSweeper
{
    public class Board
    {
        public Cell[,] Cells { get; private set; }

        public int Height { get; private set; }
        public int Width { get; private set; }

        private int FlaggedMines { get; set; }
        public int MinesLeftUnflagged { get; set; }

        private readonly int BoardMines;
        
        public Cell? HoveredCell { set; get; }

        public Board(int hight, int width, int numOfMines)
        {

            Height = hight;
            Width = width;
            MinesLeftUnflagged = BoardMines = numOfMines;

            Cells = new Cell[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Cells[i, j] = new Cell(i, j, CellType.Empty);
                }
            }

            AddMines();
            CalculateMines();
            FlaggedMines = 0;
        }


        public void UpdateHoveredCell(MouseEventArgs mouseArgs)
        {

            HoveredCell = null;
            var clickedX = mouseArgs.X - (Cell.CellSize / 2);
            var clickedY = mouseArgs.Y - (Cell.CellSize * 2);

            var cellX = clickedX / Cell.CellSize;
            var cellY = clickedY / Cell.CellSize;

            if ((cellX < 0 || cellX > Height - 1 || cellY < 0 || cellY > Width - 1) == false)
            {
                HoveredCell = Cells[cellX, cellY];
            }
        }


        // to win you need all mines to be flaged or 
        // all cells that are not mines opened.
        public bool CheckForWin()
        {
            if (FlaggedMines == BoardMines)
            {
                return true;
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Cells[i, j].Closed && Cells[i, j].IsMine == false)
                    {
                        return false;
                    }
                }
            }
            return true; ;
        }


        // this is called on event click
        public bool OpenHoveredCell()
        {
            if (HoveredCell != null)
            {
                switch (HoveredCell.CellType)
                {
                    case CellType.Flagged:
                        break;
                    case CellType.Empty:
                        OpenCellsRecursively(HoveredCell.Board_X, HoveredCell.Board_y);
                        HoveredCell.CellState = CellState.Opened;
                        break;
                    case CellType.Mine:
                        ShowMines();
                        return true;
                }
            }
            return false;
        }


        // keep opening nighbours till the one with mines > 0
        // also any mine doesnt open .. 
        public void OpenCellsRecursively(int x, int y)
        {
            if (x < 0 || x > Height - 1 || y < 0 || y > Width - 1)
            {
                return;
            }
            if (Cells[x, y].CellType == CellType.Mine || Cells[x, y].Opened)
            {
                return;
            }
            if (Cells[x, y].NumOfMines < 1) // an empty cell .. keep opening 
            {
                Cells[x, y].CellState = CellState.Opened;
                OpenCellsRecursively(x + 1, y);
                OpenCellsRecursively(x - 1, y);
                OpenCellsRecursively(x, y + 1);
                OpenCellsRecursively(x, y - 1);
            }
        }

        public void FlagHoveredCell()
        {
            if (HoveredCell != null && !HoveredCell.Opened)
            {
                if (HoveredCell.IsMine)
                {
                    FlaggedMines++;
                }

                if (HoveredCell.CellType == CellType.Flagged)
                {
                    HoveredCell.CellType = CellType.Empty;
                    MinesLeftUnflagged++;
                }
                else
                {
                    if (MinesLeftUnflagged == 0)
                        return;
                    MinesLeftUnflagged--;
                    HoveredCell.CellType = CellType.Flagged;
                }
            }
        }

        private void AddMines()
        {
            Random random = new Random();
            int randX;
            int randY;
            for (int i = 0; i < MinesLeftUnflagged; i++)
            {
                do
                {
                    randX = random.Next(Height);
                    randY = random.Next(Width);
                } while (Cells[randX, randY].IsMine);

                Cells[randX, randY].CellType = CellType.Mine;
            }
        }

        private void CalculateMines()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Cells[i, j].IsMine) continue;
                    Cells[i, j].NumOfMines = SumAdjacentMines(i, j);
                }
            }
        }

        private int SumAdjacentMines(int x, int y)
        {
            int mines = 0;
            int posX;
            int posY;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    posX = x + i;
                    posY = y + j;
                    if (posX < 0 || posX > Height - 1 || posY < 0 || posY > Width - 1)
                        continue;
                    if (Cells[posX, posY].IsMine)
                    {
                        mines++;

                    }
                }
            }
            return mines;
        }

        private void ShowMines()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (Cells[i, j].IsMine)
                    {
                        Cells[i, j].CellState = CellState.Opened;

                    }
                }
            }
        }

    }
}
