using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntSim
{
    public partial class SimDisplay : Form
    {
        //private System.Drawing.Graphics g;
        private System.Drawing.Pen blackPen = new System.Drawing.Pen(Color.Black, 1F);
        private System.Drawing.Pen darkBluePen = new System.Drawing.Pen(Color.DarkBlue, 1F);
        private System.Drawing.Pen redPen = new System.Drawing.Pen(Color.Red, 1F);
        private SolidBrush darkRedBrush = new SolidBrush(Color.DarkRed);
        private SolidBrush redBrush = new SolidBrush(Color.Red);
        private SolidBrush orangeRedBrush = new SolidBrush(Color.OrangeRed);
        private SolidBrush darkBlueBrush = new SolidBrush(Color.DarkBlue);
        private SolidBrush blueBrush = new SolidBrush(Color.Blue);
        private SolidBrush lightBlueBrush = new SolidBrush(Color.LightBlue);
        private SolidBrush darkGreenBrush = new SolidBrush(Color.DarkGreen);
        private SolidBrush greenBrush = new SolidBrush(Color.Green);
        private SolidBrush lightGreenBrush = new SolidBrush(Color.LightGreen);
        private SolidBrush darkGoldenRodBrush = new SolidBrush(Color.DarkGoldenrod);
        private SolidBrush goldeRodBrush = new SolidBrush(Color.Goldenrod);
        private SolidBrush lightGoldenrodYellowBrush = new SolidBrush(Color.LightGoldenrodYellow);

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        private int scale = 10;
        private int worldDimensions = 40;
        private int homeDimensions = 5;
        private int tickIntervals = 300;
        private Location homeStart;

        private Simulator sim;

        public SimDisplay()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true); 
            //this.SetStyle(ControlStyles.ResizeRedraw, true); 
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); 
            //this.SetStyle(ControlStyles.UserPaint, true);
            InitializeComponent();

            //g = this.CreateGraphics();

            homeStart = new Location(worldDimensions / 3, worldDimensions / 3);
            sim = new Simulator(worldDimensions, homeDimensions, homeStart);

            timer.Interval = tickIntervals;
            timer.Tick += new EventHandler(TimerCallBack);

        }

        private void TimerCallBack(object sender, EventArgs e)
        {
            sim.TimeTick();
            this.Invalidate();
            this.Update(); // this is required since Invalidate repaints the form at certain intervals: Update forces refresh
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            // Call the OnPaint method of the base class.
            base.OnPaint(pe);

            Graphics g = pe.Graphics;
            
            // draw each cell
            foreach(Cell c in sim.World.AllCells)
            {
                // draw available food squares
                if (c.AvailableFood > 0)
                {
                    int red = 255;
                    int green = 240 - Math.Max(0, (c.AvailableFood > 30 ? 30 : c.AvailableFood) * 240 / 30);
                    //Console.WriteLine("Food:" + c.AvailableFood + "--calc: " + c.AvailableFood * 255 / 30);
                    int blue = green;
                    g.FillRectangle(new SolidBrush(Color.FromArgb(red, green, blue)), new Rectangle(c.Location.X * scale, c.Location.Y * scale, scale, scale));
                }
                //if(c.AvailableFood > 20) 
                //    g.FillRectangle(darkRedBrush, new Rectangle(c.Location.X * scale, c.Location.Y * scale, scale, scale));
                //else if(c.AvailableFood > 10)
                //    g.FillRectangle(redBrush, new Rectangle(c.Location.X * scale, c.Location.Y * scale, scale, scale));
                //else if(c.AvailableFood > 0)
                //    g.FillRectangle(orangeRedBrush, new Rectangle(c.Location.X * scale, c.Location.Y * scale, scale, scale));


                if(c.AvailableFood == 0)
                {
                    //if (c.FoodPheromone > 0)
                    //{
                    //    int blue = 255;
                    //    int green = 240 - (int)(c.FoodPheromone * 20 > 240 ? 240: c.FoodPheromone * 20);
                    //    //Console.WriteLine("P: " + c.FoodPheromone);
                    //    int red = green;
                    //    g.FillRectangle(new SolidBrush(Color.FromArgb(red, green, blue)), new Rectangle(c.Location.X * scale, c.Location.Y * scale, scale, scale));
                    //}

                    if (c.FoodPheromone > 20)
                        g.FillRectangle(darkGoldenRodBrush, new Rectangle(c.Location.X * scale, c.Location.Y * scale, scale / 2, scale));
                    else if (c.FoodPheromone > 10)
                        g.FillRectangle(goldeRodBrush, new Rectangle(c.Location.X * scale, c.Location.Y * scale, scale / 2, scale));
                    else if (c.FoodPheromone > 0)
                        g.FillRectangle(lightGoldenrodYellowBrush, new Rectangle(c.Location.X * scale, c.Location.Y * scale, scale / 2, scale));

                    if (c.HomePheromone > 20)
                        g.FillRectangle(darkBlueBrush, new Rectangle(c.Location.X * scale + scale / 2, c.Location.Y * scale, scale / 2, scale));
                    if (c.HomePheromone > 10)
                        g.FillRectangle(blueBrush, new Rectangle(c.Location.X * scale + scale / 2, c.Location.Y * scale, scale / 2, scale));
                    if (c.HomePheromone > 0)
                        g.FillRectangle(lightBlueBrush, new Rectangle(c.Location.X * scale + scale / 2, c.Location.Y * scale, scale / 2, scale));
                }

                // draw ant
                if(c.Ant != null)
                {
                    //Console.WriteLine("Cell " + c.Location.ToString() + " has ant in it!!!!");

                    Pen pen = c.Ant.HasFood ? redPen : blackPen;

                    switch(c.Ant.Facing)
                    {
                        case FacingDirection.N:
                        case FacingDirection.S:
                            g.DrawLine(pen, c.Location.X * scale + scale / 2, 
                                                 c.Location.Y * scale + 2, 
                                                 c.Location.X * scale + scale / 2, 
                                                 c.Location.Y * scale + (scale - 2));
                            break;
                        case FacingDirection.W:
                        case FacingDirection.E:
                            g.DrawLine(pen, c.Location.X * scale + 2,
                                                 c.Location.Y * scale + scale / 2,
                                                 c.Location.X * scale + (scale - 2),
                                                 c.Location.Y * scale + scale / 2);
                            break;
                        case FacingDirection.NW:
                        case FacingDirection.SE:
                            g.DrawLine(pen, c.Location.X * scale + 3,
                                                 c.Location.Y * scale + 3,
                                                 c.Location.X * scale + (scale - 3),
                                                 c.Location.Y * scale + (scale - 3));
                            break;
                        case FacingDirection.NE:
                        case FacingDirection.SW:
                            g.DrawLine(pen, c.Location.X * scale + 3,
                                                 c.Location.Y * scale + (scale - 3),
                                                 c.Location.X * scale + (scale - 3),
                                                 c.Location.Y * scale + 3);
                            break;
                    }
                    
                }

            }

            // draw home
            int homeSize = homeDimensions * scale;
            g.DrawRectangle(darkBluePen, homeStart.X * scale, homeStart.Y * scale, homeSize, homeSize);

        }

        private void StartStopButton_Click(object sender, EventArgs e)
        {
            if (timer.Enabled == false)
            {
                timer.Enabled = true;
                StartStopButton.Text = "Stop";
            }
            else
            {
                timer.Enabled = false;
                StartStopButton.Text = "Start";
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var newInterval = Math.Max((timer.Interval - 100), 1);
            timer.Interval = newInterval;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var newInterval = timer.Interval + 100;
            timer.Interval = newInterval;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var newInterval = timer.Interval + 500;
            timer.Interval = newInterval;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var newInterval = Math.Max((timer.Interval - 500), 1);
            timer.Interval = newInterval;
        }


        
    }
}
