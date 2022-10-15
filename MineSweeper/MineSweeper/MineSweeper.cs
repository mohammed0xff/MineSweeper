using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace MineSweeper
{
    enum Level
    {
        Begginer,
        Intermediate,
        Expert
    }

    class MineSweeper : Form
    {
        private Label label1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem level1ToolStripMenuItem;
        private ToolStripMenuItem levelToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem3;

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.level1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.levelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(696, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.level1ToolStripMenuItem,
            this.levelToolStripMenuItem,
            this.toolStripMenuItem3});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(96, 24);
            this.toolStripMenuItem1.Text = "New Game";
            // 
            // level1ToolStripMenuItem
            // 
            this.level1ToolStripMenuItem.Name = "level1ToolStripMenuItem";
            this.level1ToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.level1ToolStripMenuItem.Text = "Begginer";
            this.level1ToolStripMenuItem.Click += new System.EventHandler(this.Level1ToolStripMenuItem_Click);
            // 
            // levelToolStripMenuItem
            // 
            this.levelToolStripMenuItem.Name = "levelToolStripMenuItem";
            this.levelToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.levelToolStripMenuItem.Text = "Intermediate";
            this.levelToolStripMenuItem.Click += new System.EventHandler(this.LevelToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(177, 26);
            this.toolStripMenuItem3.Text = "Expert";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.ToolStripMenuItem3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mines : ";
            // 
            // MineSweeper
            // 
            this.ClientSize = new System.Drawing.Size(696, 593);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MineSweeper";
            this.Load += new System.EventHandler(this.MineSweeper_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MineSweeper_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MineSweeper_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MineSweeper_MouseMove);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Board Board;
        private readonly DisplayManager DisplayManager;

        public static int BoardHight { get; set; }
        public static int BoardWidth { get; set; }

        private int NumOfMines { get; set; }
        private bool PlayerLost { get; set; }
        private bool PlayerWon { get; set; }
        private Level CurrentLevel { get; set; }


        public MineSweeper()
        {
            InitializeComponent();
            CurrentLevel = Level.Intermediate;
            UpateBoardsize();
            ResizeScreen();
            Board = new Board(BoardHight, BoardWidth, NumOfMines);
            DisplayManager = new DisplayManager();

            DoubleBuffered = true;
        }

        private void RestartGame(Level lvl = Level.Intermediate)
        {
            PlayerLost = false;
            PlayerWon = false;
            CurrentLevel = lvl;
            UpateBoardsize();
            ResizeScreen();
            Board = new Board(BoardHight, BoardWidth, NumOfMines);
        }

        private void ResizeScreen()
        {
            this.Width = BoardHight * Cell.CellSize + 40;
            this.Height = BoardWidth * Cell.CellSize + 100;
        }

        // get the mouse state and send it to the board to update the 
        // hovered cell 

        private void UpateBoardsize()
        {
            switch (CurrentLevel)
            {
                case Level.Begginer:
                    BoardHight = 8;
                    BoardWidth = 8;
                    NumOfMines = 10;
                    break;
                case Level.Intermediate:
                    BoardHight = 16;
                    BoardWidth = 16;
                    NumOfMines = 40;
                    break;
                case Level.Expert:
                    BoardHight = 30;
                    BoardWidth = 16;
                    NumOfMines = 90;
                    break;
                default:
                    break;
            }
        }


     


        private void MineSweeper_MouseMove(object sender, MouseEventArgs e)
        {
            Board.UpdateHoveredCell(e);
            Cursor = Board.HoveredCell != null && Board.HoveredCell.Closed ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }

        private void MineSweeper_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DisplayManager.DisplayBoard(ref Board, g);

            DisplayManager.DrawString(Board.MinesLeftUnflagged.ToString(), new (50, -20), g);

        }

        private void MineSweeper_MouseClick(object sender, MouseEventArgs e)
        {
            if (PlayerLost || PlayerWon)
                return;

            if (e.Button == MouseButtons.Left)
            {
                if (Board.OpenHoveredCell()) // returns true if it's a mine.
                {
                    MessageBox.Show("Sorry, You Lost! :( ");
                    PlayerLost = true;
                }
                if (Board.CheckForWin())
                {
                    MessageBox.Show("Congrats, YOU WON!");
                    PlayerWon = true;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                Board.FlagHoveredCell();
            }
        }

        private void Level1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentLevel = Level.Begginer;
            RestartGame(CurrentLevel);
        }
        private void LevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentLevel = Level.Intermediate;
            RestartGame(CurrentLevel);
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            CurrentLevel = Level.Expert;
            RestartGame(CurrentLevel);
        }

        private void MineSweeper_Load(object sender, EventArgs e)
        {
        }
    }
}
