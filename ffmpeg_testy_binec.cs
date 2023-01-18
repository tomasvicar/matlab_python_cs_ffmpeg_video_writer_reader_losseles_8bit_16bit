using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.Video.FFMPEG;
using Newtonsoft.Json;

namespace test_ffmpeg2
{
    public partial class Form1 : Form
    {
        List<Bitmap> imgs;
        string metadata = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            ////// https://github.com/tinohager/DotNetFFmpegPipe/blob/master/src/DotNetFFmpegPipe.SimplePipe3/Program.cs



            var ffprobe = "ffprobe.exe";
            if (!File.Exists(ffprobe))
            {
                MessageBox.Show("ffprobe not avaliable", "ffprobe  error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var ffmpeg = "ffmpeg.exe";
            if (!File.Exists(ffmpeg))
            {
                MessageBox.Show("FFMPEG not avaliable", "FFMPEG error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            //VideoFileReader videoFileReader = new VideoFileReader();

            //videoFileReader.Open(textBox_pathLoad.Text);

            //imgs = new List<Bitmap>();

            //while (true)
            //    using (var videoFrame = videoFileReader.ReadVideoFrame())
            //    {
            //        if (videoFrame == null)
            //            break;

            //        imgs.Add((Bitmap)videoFrame.Clone());
            //    }

            //videoFileReader.Close();





            //var inputArgs = $"-y -i " + textBox_pathLoad.Text;
            //var outputArgs = $"-frames 100 -vcodec bmp -f image2pipe -";



            var process_meta = new Process
            {
                StartInfo =
                {
                    FileName = ffprobe,
                    Arguments = $" -v info -show_format -show_streams -print_format json " + textBox_pathLoad.Text,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                },
            };


            //var process_meta = new Process
            //{
            //    StartInfo =
            //    {
            //        FileName = ffmpeg,
            //        Arguments = $"-y -f ffmetadata -i " + textBox_pathLoad.Text,
            //        UseShellExecute = false,
            //        CreateNoWindow = true,
            //        RedirectStandardInput = true,
            //        RedirectStandardError = true,
            //        RedirectStandardOutput = true,
            //    },
            //};


            //process_meta.ErrorDataReceived += (sender_, eventArgs) => Console.Write(eventArgs.Data);
            process_meta.Start();
            //process_meta.BeginErrorReadLine();

            
            //Stream output_meta = process_meta.StandardError.BaseStream;
            Stream output_meta = process_meta.StandardOutput.BaseStream;

            var buffer_meta = new byte[500];

            while (true)
            {
                var length = output_meta.Read(buffer_meta, 0, buffer_meta.Length);
                if (length == 0)
                {
                    break;
                }
                string meta_part = Encoding.UTF8.GetString(buffer_meta.Take(length).ToArray());
                metadata = metadata + meta_part;
            }



            dynamic stuff = JsonConvert.DeserializeObject(metadata);


            int width = stuff.streams[0].width;
            int height = stuff.streams[0].height;
            string pix_fmt = stuff.streams[0].pix_fmt;
            string avg_frame_rate = stuff.streams[0].avg_frame_rate;
            int nb_frames = stuff.streams[0].nb_frames;

            float duration = stuff.streams[0].duration;
            string codec_name = stuff.streams[0].codec_name;
            int bits_per_raw_sample = stuff.streams[0].bits_per_raw_sample;


            var inputArgs = $"-y -i " + textBox_pathLoad.Text;
            var outputArgs = $"-an -f image2pipe -vcodec rawvideo -pix_fmt rgb24 -";

            var process = new Process
            {
                StartInfo =
                {
                    FileName = ffmpeg,
                    Arguments = $"{inputArgs} {outputArgs}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                },
            };



            process.ErrorDataReceived += (sender_, eventArgs) => Console.Write(eventArgs.Data);
            process.Start();
            process.BeginErrorReadLine();


            


                //Stream error = process.StandardError.BaseStream;

                //var buffer_meta = new byte[1000];

                //while (true)
                //{
                //    var length_meta = error.Read(buffer_meta, 0, buffer_meta.Length);
                //    metadata = metadata + Encoding.UTF8.GetString(buffer_meta.Take(length_meta).ToArray());
                //    if (AllIndexesOf(metadata, "Metadata:").Count() > 1)
                //        break;
                //}

                //error.Close();
                //error.Dispose();


                //System.Threading.Thread.Sleep(50);

                //Console.Write(metadata);

                //string metadata = "";
                //while (process.StandardError.Peek() > -1)
                //{

                //    metadata = metadata + process.StandardError.ReadLine();

                //}

                //Console.Write(metadata);


                //StreamReader errorStreamReader = process.StandardError;
                //string metadata = "";
                //while (true)
                //{
                //    var tmp = process.StandardError.Read();
                //    if (tmp == null)
                //    {
                //        break;
                //    }
                //    else
                //    {
                //        metadata = metadata + tmp;
                //    }
                //}


                //Stream error = process.StandardError.BaseStream;
                //Stream output = process.StandardOutput.BaseStream;

                //var buffer_meta = new byte[10000];
                //string metadata = "";
                //error.ReadTimeout = 200;
                //while (true)
                //{
                //    //var length_meta = error.Read(buffer_meta, 0, buffer_meta.Length);
                //    var length_meta = error.

                //    if (length_meta == 0)
                //    {
                //        break;
                //    }

                //    metadata = metadata + Encoding.UTF8.GetString(buffer_meta.Take(length_meta).ToArray());

                //    break
                //}


            Stream output = process.StandardOutput.BaseStream;


            var w = 1224;
            var h = 970;

            imgs = new List<Bitmap>();

            var imgData = new byte[w * h * 3];
            var buffer = new byte[32768];

            var used_values = 0;
            while (true)
            {
                var length = output.Read(buffer, 0, buffer.Length);
                var tmp_length = length;
                if ((used_values + length) > imgData.Count())
                {
                    break;
                }
                Array.Copy(buffer, 0, imgData, used_values, length);
                used_values = used_values + length;
            }
            var bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var rect = new Rectangle(0, 0, w, h);
            var bitmapData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
            var ptr = bitmapData.Scan0;
            Marshal.Copy(imgData, 0, ptr, imgData.Length);
            bmp.UnlockBits(bitmapData);
            imgs.Add((Bitmap)bmp.Clone());




            //var buffer = new byte[w * h * 3];
            //var length = output.Read(buffer, 0, buffer.Length);
            //var bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //var rect = new Rectangle(0, 0, w, h);
            //var bitmapData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
            //var ptr = bitmapData.Scan0;
            //Marshal.Copy(buffer, 0, ptr, buffer.Length);
            //bmp.UnlockBits(bitmapData);

            //imgs.Add((Bitmap)bmp.Clone());

            //var buffer = new byte[w * h * 3];
            //imgs = new List<Bitmap>();
            //while (true)
            //{
            //    var length = output.Read(buffer, 0, buffer.Length);
            //    if (length == 0)
            //    {
            //        break;
            //    }
            //    var bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //    var rect = new Rectangle(0, 0, w, h);
            //    var bitmapData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
            //    var ptr = bitmapData.Scan0;
            //    Marshal.Copy(buffer, 0, ptr, buffer.Length);

            //    bmp.UnlockBits(bitmapData);

            //    imgs.Add((Bitmap)bmp.Clone());

            //}



            //var index = 0;
            //var buffer = new byte[32768];
            //var imageData = new List<byte>();
            //byte[] imageHeader = null;
            //imgs = new List<Bitmap>();

            //while (true)
            //{
            //    var length = output.Read(buffer, 0, buffer.Length);
            //    if (length == 0)
            //    {
            //        break;
            //    }

            //    if (imageHeader == null)
            //    {
            //        imageHeader = buffer.Take(5).ToArray();
            //    }

            //    if (buffer.Take(5).SequenceEqual(imageHeader))
            //    {
            //        if (imageData.Count > 0)
            //        {
            //            //File.WriteAllBytes($"image{index}.{outputFormat}", imageData.ToArray());

            //            var bitmapBytes = imageData.ToArray();

            //            Bitmap bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            //            var rect = new Rectangle(0, 0, w, h);
            //            var bitmapData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
            //            var ptr = bitmapData.Scan0;
            //            Marshal.Copy(bitmapBytes, 0, ptr, bitmapBytes.Length);

            //            bmp.UnlockBits(bitmapData);

            //            imgs.Add((Bitmap)bmp.Clone());


            //            imageData.Clear();
            //            index++;
            //        }
            //    }

            //    imageData.AddRange(buffer.Take(length));
            //}



            //var index = 0;
            //var buffer = new byte[w * h];
            //var imageData = new List<byte>();
            //byte[] imageHeader = null;
            //imgs = new List<Bitmap>();

            //while (true)
            //{
            //    var length = output.Read(buffer, 0, buffer.Length);
            //    if (length == 0)
            //    {
            //        break;
            //    }

            //    if (imageHeader == null)
            //    {
            //        imageHeader = buffer.Take(5).ToArray();
            //    }

            //    if (imageHeader != null)
            //    {
            //        if (imageData.Count > 0)
            //        {
            //            //File.WriteAllBytes($"image{index}.{outputFormat}", imageData.ToArray());

            //            var bitmapBytes = imageData.ToArray();

            //            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            //            var rect = new Rectangle(0, 0, w, h);
            //            var bitmapData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
            //            var ptr = bitmapData.Scan0;
            //            Marshal.Copy(bitmapBytes, 0, ptr, bitmapBytes.Length);

            //            bmp.UnlockBits(bitmapData);

            //            imgs.Add((Bitmap)bmp.Clone());

            //            bmp.Dispose();
            //            imageData.Clear();
            //            index++;
            //        }
            //    }

            //    imageData.AddRange(buffer.Take(length));
            //}



            //var index = 0;
            //var buffer = new byte[32768];
            //var imageData = new List<byte>();
            //byte[] imageHeader = null;
            //imgs = new List<Bitmap>();

            //while (true)
            //{
            //    var length = output.Read(buffer, 0, buffer.Length);
            //    if (length == 0)
            //    {
            //        break;
            //    }

            //    if (imageHeader == null)
            //    {
            //        imageHeader = buffer.Take(5).ToArray();
            //    }

            //    if (imageHeader != null)
            //    {
            //        if (imageData.Count >= w*h*3)
            //        {
            //            //File.WriteAllBytes($"image{index}.{outputFormat}", imageData.ToArray());

            //            var bitmapBytes = imageData.ToArray();

            //            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            //            var rect = new Rectangle(0, 0, w, h);
            //            var bitmapData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
            //            var ptr = bitmapData.Scan0;
            //            Marshal.Copy(bitmapBytes, 0, ptr, bitmapBytes.Length);

            //            bmp.UnlockBits(bitmapData);

            //            imgs.Add((Bitmap)bmp.Clone());

            //            bmp.Dispose();
            //            imageData.Clear();
            //            index++;
            //        }
            //    }

            //    imageData.AddRange(buffer.Take(length));
            //}



            // File.WriteAllBytes($"image{index}.{outputFormat}", imageData.ToArray());


            button_show.Enabled = true;
            numericUpDown_frameNum.Maximum = imgs.Count();

            output.Close();
            output.Dispose();

            process.WaitForExit();
            process.Dispose();

        }

        private void button_save_Click(object sender, EventArgs e)
        {
            //VideoFileWriter writer = new VideoFileWriter();
            //writer.Open(textBox_pathSave.Text, imgs.ElementAt(0).Width, imgs.ElementAt(0).Height, 25, VideoCodec.FFV1);

            //for (int frame = 0; frame < 50; frame++)
            //{

            //    Bitmap b16bpp = new Bitmap(imgs.ElementAt(0).Width, imgs.ElementAt(0).Height, System.Drawing.Imaging.PixelFormat.Format16bppGrayScale);
            //    var rect = new Rectangle(0, 0, imgs.ElementAt(0).Width, imgs.ElementAt(0).Height);
            //    var bitmapData = b16bpp.LockBits(rect, ImageLockMode.WriteOnly, b16bpp.PixelFormat);

            //    var numberOfBytes = bitmapData.Stride * imgs.ElementAt(0).Height;
            //    var bitmapBytes = new short[imgs.ElementAt(0).Width * imgs.ElementAt(0).Height];

            //    var random = new Random();
            //    for (int x = 0; x < imgs.ElementAt(0).Width; x++)
            //    {
            //        for (int y = 0; y < imgs.ElementAt(0).Height; y++)
            //        {
            //            var i = ((y * imgs.ElementAt(0).Width) + x); // 16bpp

            //            // Generate the next random pixel color value.
            //            var value = (short)random.Next(5);

            //            bitmapBytes[i] = value;         // GRAY

            //        }
            //    }
            //    var ptr = bitmapData.Scan0;
            //    Marshal.Copy(bitmapBytes, 0, ptr, bitmapBytes.Length);

            //    b16bpp.UnlockBits(bitmapData);

            //    writer.WriteVideoFrame(b16bpp);
            //}
        }

        private void button_show_Click(object sender, EventArgs e)
        {
            Bitmap img = imgs.ElementAt(decimal.ToInt32(numericUpDown_frameNum.Value));


            //Bitmap img_tmp = (Bitmap)img.Clone();
            //Bitmap img_new = new Bitmap(img_tmp.Width, img_tmp.Height, PixelFormat.Format24bppRgb);
            //using (Graphics gr = Graphics.FromImage(img_new))
            //{
            //    gr.DrawImage(img_tmp, new Rectangle(0, 0, img_new.Width, img_new.Height));
            //}

            //Bitmap img_tmp = new Bitmap(img);
            //Bitmap img_new = img_tmp.Clone(new Rectangle(0, 0, img_tmp.Width, img_tmp.Height), PixelFormat.Format24bppRgb);

            //Bitmap img_new = Accord.Imaging.Clone(img, PixelFormat.Format24bppRgb);


            Bitmap img_new = img.Clone(new Rectangle(0, 0, img.Width, img.Height), PixelFormat.Format32bppArgb);

            pictureBox1.Image = img_new;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                button_show.PerformClick();
            }

        }


        public List<int> AllIndexesOf(string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
    }
}
