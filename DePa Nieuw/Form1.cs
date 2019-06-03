using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace DePa_Nieuw
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Point startPt;
        private Shape s = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            this.MouseUp += new MouseEventHandler(Form1_MouseUp);
        }
        void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPt = Cursor.Position;
                s = new Shape(Color.Red);
                s.Location = new Point(e.X, e.Y);
                this.Controls.Add(s);
                s.BringToFront();
            }
        }

        void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point curPt = Cursor.Position;
                Rectangle RC = NormalizedRC(startPt, curPt);
                if (RC.Width >= Shape.HandleSize * 2 && RC.Height >= Shape.HandleSize * 2)
                {
                    s.SuspendLayout();
                    s.Location = this.PointToClient(RC.Location);
                    s.Size = RC.Size;

                    s.ResumeLayout();
                }
            }
        }

        void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                s = null;
            }
        }

        private Rectangle NormalizedRC(Point ptA, Point ptB)
        {
            return new Rectangle(new Point(Math.Min(ptA.X, ptB.X), Math.Min(ptA.Y, ptB.Y)), new Size(Math.Abs(ptB.X - ptA.X), Math.Abs(ptB.Y - ptA.Y)));
        }

         void button1_Click(object sender, EventArgs e)
        {
            Data.boolShape = 0;
        }

        public void button2_Click(object sender, EventArgs e)
        {
            Data.boolShape = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }

    public partial class Shape : UserControl
    {

        private const int Thickness = 10;
        public const int HandleSize = 6;

        private Color c;
        private Point startPt;
        private PictureBox pbNorth, pbEast, pbSouth, pbWest;

        public Shape(Color c)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();

            this.c = c;
            this.Size = new Size(HandleSize * 2, HandleSize * 2);
            this.BackColor = Color.Red;
            this.SizeChanged += new EventHandler(Shape_SizeChanged);

            pbNorth = new PictureBox();
            pbNorth.Size = new Size(HandleSize, HandleSize);
            pbNorth.BackColor = Color.Black;
            pbNorth.Cursor = Cursors.SizeNS;
            pbNorth.MouseDown += new MouseEventHandler(pbNorth_MouseDown);
            pbNorth.MouseMove += new MouseEventHandler(pbNorth_MouseMove);
            this.Controls.Add(pbNorth);

            pbEast = new PictureBox();
            pbEast.Size = new Size(HandleSize, HandleSize);
            pbEast.BackColor = Color.Black;
            pbEast.Cursor = Cursors.SizeWE;
            pbEast.MouseDown += new MouseEventHandler(pbEast_MouseDown);
            pbEast.MouseMove += new MouseEventHandler(pbEast_MouseMove);
            this.Controls.Add(pbEast);

            pbSouth = new PictureBox();
            pbSouth.Size = new Size(HandleSize, HandleSize);
            pbSouth.BackColor = Color.Black;
            pbSouth.Cursor = Cursors.SizeNS;
            pbSouth.MouseDown += new MouseEventHandler(pbSouth_MouseDown);
            pbSouth.MouseMove += new MouseEventHandler(pbSouth_MouseMove);
            this.Controls.Add(pbSouth);

            pbWest = new PictureBox();
            pbWest.Size = new Size(HandleSize, HandleSize);
            pbWest.BackColor = Color.Black;
            pbWest.Cursor = Cursors.SizeWE;
            pbWest.MouseDown += new MouseEventHandler(pbWest_MouseDown);
            pbWest.MouseMove += new MouseEventHandler(pbWest_MouseMove);
            this.Controls.Add(pbWest);

            RecomputeRegion();
        }

        void Shape_SizeChanged(object sender, EventArgs e)
        {
            RecomputeRegion();
        }

        void pbWest_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPt = Cursor.Position;
                this.BringToFront();
            }
        }

        void pbWest_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point curPt = Cursor.Position;
                int DeltaX = curPt.X - startPt.X;
                if (this.Width - DeltaX >= HandleSize * 2)
                {
                    this.SuspendLayout();
                    this.Width = this.Width - DeltaX;
                    this.Location = new Point(this.Location.X + DeltaX, this.Location.Y);
                    this.ResumeLayout();
                    RecomputeRegion();
                }
                startPt = curPt;
            }
        }

        void pbNorth_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPt = Cursor.Position;
                this.BringToFront();
            }
        }

        void pbNorth_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point curPt = Cursor.Position;
                int DeltaY = curPt.Y - startPt.Y;
                if (this.Height - DeltaY >= HandleSize * 2)
                {
                    this.SuspendLayout();
                    this.Height = this.Height - DeltaY;
                    this.Location = new Point(this.Location.X, this.Location.Y + DeltaY);
                    this.ResumeLayout();
                    RecomputeRegion();
                }
                startPt = curPt;
            }
        }

        void pbSouth_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPt = Cursor.Position;
                this.BringToFront();
            }
        }

        void pbSouth_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point curPt = Cursor.Position;
                int DeltaY = curPt.Y - startPt.Y;
                if (this.Height + DeltaY >= HandleSize * 2)
                {
                    this.Height = this.Height + DeltaY;
                    RecomputeRegion();
                }
                startPt = curPt;
            }
        }

        void pbEast_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPt = Cursor.Position;
                this.BringToFront();
            }
        }

        void pbEast_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point curPt = Cursor.Position;
                int DeltaX = curPt.X - startPt.X;
                if (this.Width + DeltaX >= HandleSize * 2)
                {
                    this.Width = this.Width + DeltaX;
                    RecomputeRegion();
                }
                startPt = curPt;
            }
        }

        private const int HTCLIENT = 0x0001;
        private const int HTCAPTION = 0x0002;
        private const int WM_NCHITTEST = 0x0084;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if ((m.Msg == WM_NCHITTEST) & (m.Result.ToInt32() == HTCLIENT))
            {
                m.Result = (IntPtr)HTCAPTION;
            }
        }

        private void RecomputeRegion()
        {
            GraphicsPath gp = new GraphicsPath();
            Rectangle RC = this.ClientRectangle;
            RC.Inflate(-Thickness, -Thickness);
            if (Data.boolShape == 0)
            {
                gp.AddEllipse(RC);
            }
            else
            { 
            gp.AddRectangle(RC);
            }

            using (Pen p = new Pen(this.c, Thickness))
            {
                gp.Widen(p);
            }

            pbNorth.Location = new Point(this.Width / 2 - HandleSize / 2, 0);
            pbEast.Location = new Point(this.Width - HandleSize, this.Height / 2 - HandleSize / 2);
            pbSouth.Location = new Point(this.Width / 2 - HandleSize / 2, this.Height - HandleSize);
            pbWest.Location = new Point(0, this.Height / 2 - HandleSize / 2);

            gp.AddRectangle(new Rectangle(pbNorth.Location, pbNorth.Size));
            gp.AddRectangle(new Rectangle(pbEast.Location, pbEast.Size));
            gp.AddRectangle(new Rectangle(pbSouth.Location, pbSouth.Size));
            gp.AddRectangle(new Rectangle(pbWest.Location, pbWest.Size));

            this.Region = new Region(gp);
            if (this.Parent != null)
            {
                this.Parent.Refresh();
            }
        }

        }
}
