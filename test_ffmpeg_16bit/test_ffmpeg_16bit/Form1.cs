using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_ffmpeg_16bit
{
    public partial class Form1 : Form
    {
        List<Bitmap> imgs;

        public Form1()
        {
            InitializeComponent();

        }

        private void button_load_Click(object sender, EventArgs e)
        {

            FfmpegVideoLoader ffmpegVideoLoader = new FfmpegVideoLoader(textBox_pathLoad.Text);

            ffmpegVideoLoader.loadMetadata();

            imgs = ffmpegVideoLoader.loadVideo("gray16", PixelFormat.Format16bppGrayScale, 2);

            //imgs = ffmpegVideoLoader.loadVideo("rgb24", PixelFormat.Format24bppRgb, 3);


        }

        private void button_save_Click(object sender, EventArgs e)
        {

        }

        private void button_show_Click(object sender, EventArgs e)
        {
            Bitmap img = imgs.ElementAt(decimal.ToInt32(numericUpDown_frameNum.Value));

            Bitmap img_new = img.Clone(new Rectangle(0, 0, img.Width, img.Height), PixelFormat.Format24bppRgb);
            pictureBox1.Image = img_new;

            //pictureBox1.Image = img;
        }

        private void numericUpDown_frameNum_ValueChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                button_show.PerformClick();
            }
        }
    }
}
