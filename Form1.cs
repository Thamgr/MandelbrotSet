using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        float dx1, dy1, dx2, dy2;
        float rx1, ry1, rx2, ry2;
        float rrx1, rry1;
        int flag = 0;
        int iter = 300;
        List<int> cred = new List<int>();
        List<int> cgreen = new List<int>();
        List<int> cblue = new List<int>();
        Random rnd = new Random();


        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine($"> {e.X} {e.Y}");
            float nx = e.X, ny = e.Y;
            if (flag == 0)
            {
                rrx1 = (rx2 - rx1) * nx / (dx2 - dx1) + rx1;
                rry1 = (ry2 - ry1) * ny / (dy2 - dy1) + ry1;
            }
            else
            {
                rx2 = (rx2 - rx1) * nx / (dx2 - dx1) + rx1;
                ry2 = rry1 + (rx2 - rrx1) * dy2 / dx2;
                rx1 = rrx1;
                ry1 = rry1;
            }
            flag = (flag + 1) % 2;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Draw();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dx1 = 0;
            dy1 = 0;
            dx2 = this.pictureBox1.Width;
            dy2 = this.pictureBox1.Height;
            rx1 = -4f;// -this.pictureBox1.Width / 2;
            ry1 = -2f;//-this.pictureBox1.Height / 2;
            rx2 = 4f;// this.pictureBox1.Width / 2;
            ry2 = 2f;// this.pictureBox1.Height / 2;
            for (int i = 0; i < iter; ++i)
            {
                cred.Add(rnd.Next(128, 256));
                cgreen.Add(rnd.Next(128, 256));
                cblue.Add(rnd.Next(128, 256));
            }
            cred[0] = 0;
            cgreen[0] = 0;
            cblue[0] = 0;
        }

        private int Check(Complex c)
        {
            Complex z = new Complex(0, 0);
            //Complex cst = new Complex(-0.553373439995225f, 0.5028056112224449f);
            //Complex cst = new Complex(-0.7513694319791928f, 0.12765531062124258f);
            //-0.6836339610373092f, 0.38817635270541084f
            for (int i = 0; i < iter; ++i)
            {
                if (z.Mod() > 2)
                {
                    return i;
                }
                z = z.Square() + c;// new Complex(1, 0);
                //^3  0.7  0.35
            }
            return 0;
        }

        private void Draw()
        {
            Console.WriteLine($"{rx1} {rx2}");
            Graphics graph = this.pictureBox1.CreateGraphics();
            graph.Clear(Color.Black);
            for (float x = dx1; x <= dx2; x += 1)
            {
                for (float y = dy1; y <= dy2; y += 1)
                {
                    float rx = (rx2 - rx1) * x / (dx2 - dx1) + rx1;
                    float ry = (ry2 - ry1) * y / (dy2 - dy1) + ry1;
                    Complex z = new Complex(rx, ry);
                    int res = Check(z);
                    Brush pen = new SolidBrush(Color.FromArgb(0, res * 255 / iter, 0));
                    //Console.WriteLine(Check(z));
                    graph.FillRectangle(pen, x, y, 1, 1);
                }
            }
        }
    }
    public class Complex
    {
        public float a;
        public float b;

        public Complex(float new_a, float new_b)
        {
            a = new_a;
            b = new_b;
        }

        public float Mod()
        {
            return (float)Math.Sqrt(a * a + b * b);
        }
        public Complex Square()
        {
            return new Complex(a * a - b * b, a * b + b * a);
        }

        public static Complex operator +(Complex x, Complex y)
        {
            return new Complex(x.a + y.a, x.b + y.b);
        }
        public static Complex operator -(Complex x, Complex y)
        {
            return new Complex(x.a - y.a, x.b - y.b);
        }
        public static Complex operator *(Complex x, Complex y)
        {
            return new Complex(x.a * y.a - x.b * y.b, x.a * y.b + x.b * y.a);
        }
    }

}
