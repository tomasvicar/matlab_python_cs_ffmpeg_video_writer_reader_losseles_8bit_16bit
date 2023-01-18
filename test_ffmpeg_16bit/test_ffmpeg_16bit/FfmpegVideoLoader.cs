using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_ffmpeg_16bit
{
    // requires ffmpeg ffmprobe avaliable (in foder with .exe)
    class FfmpegVideoLoader
    {
        int width;
        int height;
        string pix_fmt;
        string avg_frame_rate;
        int nb_frames;
        float duration;
        string codec_name;
        int bits_per_raw_sample;
        string ffprobe;
        string ffmpeg;

        string filename;
        public FfmpegVideoLoader(string filename)
        {
            this.filename = filename;

            ffprobe = "ffprobe.exe";
            if (!File.Exists(ffprobe))
            {
                MessageBox.Show("ffprobe not avaliable", "ffprobe  error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ffmpeg = "ffmpeg.exe";
            if (!File.Exists(ffmpeg))
            {
                MessageBox.Show("FFMPEG not avaliable", "FFMPEG error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public void loadMetadata()
        {
            var process_meta = new Process
            {
                StartInfo =
                {
                    FileName = ffprobe,
                    Arguments = $" -v info -show_format -show_streams -print_format json " + filename,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                },
            };

            process_meta.Start();

            Stream output_meta = process_meta.StandardOutput.BaseStream;

            var buffer_meta = new byte[500];
            string metadata = "";

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


            width = stuff.streams[0].width;
            height = stuff.streams[0].height;
            pix_fmt = stuff.streams[0].pix_fmt;
            avg_frame_rate = stuff.streams[0].avg_frame_rate;
            nb_frames = stuff.streams[0].nb_frames;

            duration = stuff.streams[0].duration;
            codec_name = stuff.streams[0].codec_name;
            bits_per_raw_sample = stuff.streams[0].bits_per_raw_sample;


        }
        public List<Bitmap> loadVideo(string ffmpegPixelFormat, PixelFormat bitmapPixelFormat, int bytesPersample)
        {
            //string ffmpegPixelFormat = "gray16";
            //PixelFormat bitmapPixelFormat = PixelFormat.Format16bppGrayScale;
            //int bytesPersample = 2;


            //string ffmpegPixelFormat = "rgb24";
            //PixelFormat bitmapPixelFormat = PixelFormat.Format24bppRgb;
            //int bytesPersample = 3;



            var process = new Process
            {
                StartInfo =
                {
                    FileName = ffmpeg,
                    Arguments = " -y -i " + filename + " -an -f image2pipe -vcodec rawvideo -pix_fmt "+ ffmpegPixelFormat + " -",
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

            Stream output = process.StandardOutput.BaseStream;


            List<Bitmap> imgs = new List<Bitmap>();

            var imgData = new byte[width * height * bytesPersample];
            var buffer = new byte[32768];

            var couter_used = 0;
            var img_completed = false;
            while (true)
            {
                var read_length = buffer.Length;
                if ((couter_used + buffer.Count()) > imgData.Count())
                {
                    read_length = imgData.Count() - couter_used;
                    img_completed = true;
                }


                var length = 0;
                if (read_length > 0)
                {
                    length = output.Read(buffer, 0, read_length);
                    Array.Copy(buffer, 0, imgData, couter_used, length);
                    couter_used = couter_used + length;
                }
                

                if (img_completed)
                {
                    var bmp = new Bitmap(width, height, bitmapPixelFormat);
                    var rect = new Rectangle(0, 0, width, height);
                    var bitmapData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
                    var ptr = bitmapData.Scan0;
                    Marshal.Copy(imgData, 0, ptr, imgData.Length);
                    bmp.UnlockBits(bitmapData);
                    imgs.Add((Bitmap)bmp.Clone());
                    couter_used = 0;
                    img_completed = false;
                    continue;
                }



                if (length == 0)
                {
                    break;
                }



            }
            return imgs;
        }



    }
}
