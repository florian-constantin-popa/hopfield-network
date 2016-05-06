using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HopfieldNetwork
{
    class Rectangle
    {
       public int x;
       public int y;
       public int width;
       public int height;
       public int N;
       System.Drawing.Pen pen;

        public Rectangle(int N, int x, int y, int width, int height) {
            this.N = N;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }
        public Rectangle()
        {
        }

        public void drawRectangle(System.Drawing.Graphics formGraphics, int x, int y, int width, int height)
        {
            pen = new System.Drawing.Pen(System.Drawing.Color.Black);
            formGraphics.DrawRectangle(pen, x, y, width, height);
            pen.Dispose();
        }
        public void colorRectangle(System.Drawing.Graphics formGraphics, int x, int y, int width, int height)
        {
            
        }

    }
}
