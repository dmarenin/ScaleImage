using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScaleImageTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Открываем изображение.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream fs = new System.IO.FileStream(openFileDialog1.FileName, System.IO.FileMode.Open);
                System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
                fs.Close();
                pictureBox1.Image = img;
            }
        }


        static Image ScaleImage(Image source, int width, int height)
        {

            Image dest = new Bitmap(width, height);
            using (Graphics gr = Graphics.FromImage(dest))
            {
                gr.FillRectangle(Brushes.White, 0, 0, width, height);  // Очищаем экран
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                float srcwidth = source.Width;
                float srcheight = source.Height;
                float dstwidth = width;
                float dstheight = height;

                if (srcwidth <= dstwidth && srcheight <= dstheight)  // Исходное изображение меньше целевого
                {
                    int left = (width - source.Width) / 2;
                    int top = (height - source.Height) / 2;
                    gr.DrawImage(source, left, top, source.Width, source.Height);
                }
                else if (srcwidth / srcheight > dstwidth / dstheight)  // Пропорции исходного изображения более широкие
                {
                    float cy = srcheight / srcwidth * dstwidth;
                    float top = ((float)dstheight - cy) / 2.0f;
                    if (top < 1.0f) top = 0;
                    gr.DrawImage(source, 0, top, dstwidth, cy);
                }
                else  // Пропорции исходного изображения более узкие
                {
                    float cx = srcwidth / srcheight * dstheight;
                    float left = ((float)dstwidth - cx) / 2.0f;
                    if (left < 1.0f) left = 0;
                    gr.DrawImage(source, left, 0, cx, dstheight);
                }

                return dest;
            }
        }

        static Image ScaleImageMain(Image img)
        {
            int x1 = 200;
            int y1 = 200;
            int x2 = 3;
            int y2 = 3;
            if (img.Width > img.Height)
            {
                x1 = 200;
                y1 = (int)Math.Round((double)img.Height / (img.Width / 200));
                y2 = (int)Math.Round((double)((200 - y1) / 2));

            }
            else
            {
                if (img.Width < img.Height)
                {
                    y1 = 200;
                    x1 = (int)Math.Round((double)img.Width / (img.Height / 200));
                    x2 = (int)Math.Round((double)((200 - x1) / 2));
                }
            }
            img = ScaleImage(img, x1, y1);
            Image dest = new Bitmap(208, 208);
            Graphics gr = Graphics.FromImage(dest);
            // Здесь рисуем рамку.
            Pen blackPen = new Pen(Color.Black, 3);
            float x = 0.0F;
            float y = 0.0F;
            float width = 208.0F;
            float height = 208.0F;
            gr.DrawRectangle(blackPen, x, y, width, height);

            gr.DrawImage(img, x2, y2, img.Width, img.Height);

            return dest;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Изменяем картинку.
            try
            {
                pictureBox1.Image = ScaleImage(pictureBox1.Image, int.Parse(textBox1.Text), int.Parse(textBox2.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.MemoryStream mass = new System.IO.MemoryStream();

                
                System.IO.FileStream fs = new System.IO.FileStream(saveFileDialog1.FileName, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite);
                pictureBox1.Image.Save(mass, System.Drawing.Imaging.ImageFormat.Png);
                
                byte[] matric = mass.ToArray();
                fs.Write(matric, 0, matric.Length);

                mass.Close();
                fs.Close();
            }
        }
    }
}
