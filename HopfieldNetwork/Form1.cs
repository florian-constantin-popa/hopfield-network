using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HopfieldNetwork
{
    public partial class Form1 : Form
    {
        int N = 10;
        Double [,] A;
        System.Drawing.Graphics formGraphics;
        public Form1()
        {
            InitializeComponent();
          //  A = new Double[10,10];
            formGraphics = this.CreateGraphics();
           
        }
        public void Init()
        {
            Color back = Color.FromKnownColor(KnownColor.Control);
            formGraphics.Clear(back);
            Rectangle rectangle = new Rectangle();
            A = new Double[10, 10];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    rectangle.drawRectangle(formGraphics, i * 30, j * 30, 30, 30);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Init();
           // formGraphics.Dispose();

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Brush myBrush = Brushes.Black; 
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if ((e.X > (i - 1) * 30 && e.X < i * 30) && (e.Y > (j - 1) * 30 && e.Y < j * 30))
                    {
                        A[i,j] = 1;
                        RectangleF rectangle = new RectangleF((i - 1) * 30, (j - 1) * 30, 30, 30);
                        formGraphics.FillRectangle(myBrush,rectangle);
                        
                    }
            }
                }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            
            Init();
        }
    }
}
