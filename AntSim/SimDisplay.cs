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
        private System.Drawing.Graphics g;
        private System.Drawing.Pen blackPen = new System.Drawing.Pen(Color.Black, 1F);
        private System.Drawing.Pen bluePen = new System.Drawing.Pen(Color.Blue, 1F);
        private System.Drawing.Pen redPen = new System.Drawing.Pen(Color.Red, 1F);
        private SolidBrush darkRedBrush = new SolidBrush(Color.DarkRed);
        private SolidBrush redBrush = new SolidBrush(Color.Red);
        private SolidBrush orangeRedBrush = new SolidBrush(Color.OrangeRed);

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        private int scale = 10;
        private int worldDimensions = 40;
        private int homeDimensions = 5;
        private Location homeStart;

        private Simulator sim;

        public SimDisplay()
        {
            //this.SetStyle(ControlStyles.DoubleBuffer, true); 
            //this.SetStyle(ControlStyles.ResizeRedraw, true); 
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); 
            //this.SetStyle(ControlStyles.UserPaint, true);
            InitializeComponent();
            homeStart = new Location(worldDimensions / 3, worldDimensions / 3);
            sim = new Simulator(worldDimensions, homeDimensions, homeStart);

            timer.Interval = 500;
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

            g = this.CreateGraphics();

            // draw home
            int homeSize = homeDimensions * scale;
            g.DrawRectangle(blackPen, homeStart.X * scale, homeStart.Y * scale, homeSize, homeSize);
            
            // draw each cell
            foreach(Cell c in sim.World.AllCells)
            {
                // draw available food squares
                if(c.AvailableFood > 20) 
                {
                    //g.DrawRectangle(redPen, c.Location.X * scale, c.Location.Y * scale, scale, scale);
                    g.FillRectangle(darkRedBrush, new Rectangle(c.Location.X * scale, c.Location.Y * scale, scale, scale));
                }
                else if(c.AvailableFood > 10)
                {
                    g.FillRectangle(redBrush, new Rectangle(c.Location.X * scale, c.Location.Y * scale, scale, scale));
                }
                else if(c.AvailableFood > 0)
                {
                    g.FillRectangle(orangeRedBrush, new Rectangle(c.Location.X * scale, c.Location.Y * scale, scale, scale));
                }

                // draw ant
                if(c.Ant != null)
                {
                    Console.WriteLine("Cell " + c.Location.ToString() + " has ant in it!!!!");

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


        
    }
}
