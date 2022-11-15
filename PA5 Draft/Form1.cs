using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PA5_Draft
{
    public partial class MainForm : Form
    {
        private int Step = 1;
        private int applesEaten;
        private readonly SnakeGame Game;
        private int NumberOfApples;
        private static int MAX_APPLE_SIZE = 20;
        private static int MIN_APPLE_SIZE = 10;
        private double PrevAppleSize = MAX_APPLE_SIZE;
        private bool ShrinkAppleSize = true;
        private double AppleSize = MAX_APPLE_SIZE;

        public MainForm()
        {
            InitializeComponent();
            Form2 f2 = new Form2();           
            if(f2.ShowDialog() == DialogResult.OK)
            {
                NumberOfApples = f2.numberOfApples;
                Game = new SnakeGame(new System.Drawing.Point((Field.Width - 20) / 2, Field.Height / 2), 40, NumberOfApples, Field.Size);
                Field.Image = new Bitmap(Field.Width, Field.Height);
                Game.EatAndGrow += Game_EatAndGrow;
                Game.HitWallAndLose += Game_HitWallAndLose;
                Game.HitSnakeAndLose += Game_HitSnakeAndLose;
                this.mainTimer.Enabled = true;
            }
            
        }

        private void Game_HitWallAndLose()
        {
            mainTimer.Stop();
            Field.Refresh();
            MessageBox.Show("You ate " + applesEaten + " apple/s");
        }
        private void Game_HitSnakeAndLose()
        {
            mainTimer.Stop();
            Field.Refresh();
            MessageBox.Show("You ate " + applesEaten + " apple/s");
        }

        private void Game_EatAndGrow()
        {
            applesEaten += 1;
            if(applesEaten % 10 == 0)
            {
                Step += 1;
            }
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            Game.Move(Step);
            Field.Invalidate();           
        }

        private void Field_Paint(object sender, PaintEventArgs e)
        {
            Pen PenForObstacles = new Pen(Color.FromArgb(40,40,40),2);
            Pen PenForSnake = new Pen(Color.FromArgb(100, 100, 100), 2);
            Brush BackGroundBrush = new SolidBrush(Color.FromArgb(150, 250, 150));
            Brush AppleBrush = new SolidBrush(Color.FromArgb(250, 50, 50));
            using (Graphics g = Graphics.FromImage(Field.Image))
            {
                g.FillRectangle(BackGroundBrush,new Rectangle(0,0,Field.Width,Field.Height));
                if (ShrinkAppleSize)
                {
                    AppleSize -= 1;
                    if (AppleSize <= MIN_APPLE_SIZE)
                        ShrinkAppleSize = false;
                }
                else
                {
                    AppleSize += 1;
                    if (AppleSize >= MAX_APPLE_SIZE)
                        ShrinkAppleSize = true;
                }


                foreach (Point Apple in Game.Apples)
                    g.FillEllipse(AppleBrush, new Rectangle(Apple.X - (int)AppleSize / 2, Apple.Y - (int)AppleSize / 2,
                         (int)AppleSize, (int)AppleSize));
                foreach (LineSeg Obstacle in Game.Obstacles)
                    g.DrawLine(PenForObstacles, new System.Drawing.Point(Obstacle.Start.X, Obstacle.Start.Y)
                        , new System.Drawing.Point(Obstacle.End.X, Obstacle.End.Y));
                foreach (LineSeg Body in Game.SnakeBody)
                    g.DrawLine(PenForSnake, new System.Drawing.Point((int)Body.Start.X, (int)Body.Start.Y)
                        , new System.Drawing.Point((int)Body.End.X, (int)Body.End.Y));

                

            }
        }


        private void Snakes_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    Game.Move(Step, Direction.UP);
                    break;
                case Keys.Down:
                    Game.Move(Step, Direction.DOWN);
                    break;
                case Keys.Left:
                    Game.Move(Step, Direction.LEFT);
                    break;
                case Keys.Right:
                    Game.Move(Step, Direction.RIGHT);
                    break;
            }
        }
    }
}
