using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DecisionTreeApp
{
    public partial class Form1 : Form
    {
        Bitmap bmp;

        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Draw();
            tableDraw();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Run program");
            AlgorytmC45 c45 = new AlgorytmC45();
        }

        private void tableDraw() {
            dataGridView1.RowCount = 4;
            dataGridView1.ColumnCount = 5;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = "-";

                }
            }
        }

        private void Draw() {
            //Знаходимо центр ширини
            int centerWidth = Convert.ToInt32(pictureBox1.Width / 2);

            List<String> boxs = new List<String>() { "Sunny", "Overcast", "Rain" };
            List<String> boxs2 = new List<String>() { "Hight", "Normal", "Yes", "Strong", "Weak" };
            List<String> boxs3 = new List<String>() { "No", "Yes", "", "No", "Yes" };
            renderDrawBox(boxs);
            renderDrawBox(boxs2, 2);
            renderDrawBox(boxs3, 3);
        }

        private void renderDrawBox(List<String> boxs, int row = 0) {
            int boxHeight = 50;
            int boxWidth = 200;
            int countBlock = boxs.Count;
            int airwidth = Convert.ToInt32((pictureBox1.Width - (boxWidth * countBlock)) / (countBlock + 1));


            drawBox(boxs[0], airwidth, boxHeight * row);
            for (int i = 1; i < boxs.Count; i++)
            {
                drawBox(boxs[i], (airwidth * (i+1)) + (boxWidth * i), boxHeight * row);
            }
        }

        private void drawBox(String drawString, int x, int y, int width = 100, int height = 50)
        {
            Graphics graph = Graphics.FromImage(bmp);
            Pen pen = new Pen(Color.Blue);
            //graph.DrawLine(pen, 10, 50, 150, 200);

            //Починаємо рисувати з центру
            //Draw Ellipse
            //graph.DrawEllipse(pen, new Rectangle(x - (20 / 2), 0, 20, 20));
            graph.DrawRectangle(pen, new Rectangle(x, y, width, height));

            //Рисуємо текст
            Font drawFont = new Font("Arial", 16);

            // Measure string.
            SizeF stringSize = graph.MeasureString(drawString, drawFont, width);
            // Draw rectangle representing size of string.
            graph.DrawRectangle(new Pen(Color.Red, 1), x, y, stringSize.Width, stringSize.Height);

            // Draw string to screen.
            graph.DrawString(drawString, drawFont, Brushes.Black, new PointF(x, y));

            pictureBox1.Image = bmp;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
