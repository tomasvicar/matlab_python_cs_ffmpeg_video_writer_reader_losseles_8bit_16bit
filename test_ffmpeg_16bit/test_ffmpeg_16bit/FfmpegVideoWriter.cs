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
    class FfmpegVideoWriter
    {

        // based on https://github.com/tinohager/DotNetFFmpegPipe/blob/master/src/DotNetFFmpegPipe.SimplePipe1/Program.cs

        string filename;
        string ffprobe;
        string ffmpeg;
        public FfmpegVideoWriter(string filename)
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

        public void writeVideo(List<Bitmap> imgs, int fps, string ffmpegPixelFormat)
        {

            //string arguments = " -y -framerate " + fps.ToString() + " -f image2pipe -i - -vcodec ffv1 -pix_fmt " + ffmpegPixelFormat + " " + filename;

            //string arguments = " -y -framerate " + fps.ToString() + " -f image2pipe -i - -vcodec ffv1 -pix_fmt " + ffmpegPixelFormat + " " + filename;

            string arguments = " -y -framerate " + fps.ToString() + " -s 1224x970 -pixel_format " + ffmpegPixelFormat + " -f rawvideo -i pipe: -vcodec ffv1 -pix_fmt " + ffmpegPixelFormat + " " + filename;

            //string arguments = " -y -framerate " + fps.ToString() + " -f image2pipe -i - -vcodec libx264 -crf 23 -pix_fmt yuv420p -preset ultrafast -r " + fps.ToString() + " out.mp4";

            var process = new Process
            {
                StartInfo =
                {
                    FileName = ffmpeg,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                },
            };
            process.ErrorDataReceived += (sender, eventArgs) => Console.Write(eventArgs.Data);
            process.Start();
            process.BeginErrorReadLine();



            var ffmpegIn = process.StandardInput.BaseStream;


            foreach (Bitmap img in imgs)
            {
                // this is for image2pipe
                //byte[] imageData = ToByteArray1(img, ImageFormat.Png);
                //byte[] imageData = ToByteArray2(img);

                byte[] imageData = ToByteArray3(img);


                //var rect = new Rectangle(0, 0, img.Width, img.Height);
                //var bitmapData = img.LockBits(rect, ImageLockMode.WriteOnly, img.PixelFormat);
                //var ptr = bitmapData.Scan0;
                //byte[] imageData = new byte[bitmapData.Stride * bitmapData.Height];
                //Marshal.Copy(ptr, imageData, 0, imageData.Length);


                //byte[] imageDataCopy = (byte[])imageData.Clone();
                //ffmpegIn.Write(imageDataCopy, 0, imageDataCopy.Length);

                //byte[] imageData = new byte[1147918];


                //var rect = new Rectangle(0, 0, img.Width, img.Height);
                //var bitmapData = img.LockBits(rect, ImageLockMode.WriteOnly, img.PixelFormat);
                //var ptr = bitmapData.Scan0;
                //byte[] imageData = new byte[1147918];
                //Marshal.Copy(ptr, imageData, 0, imageData.Length);

                //1187280 *3 = 3561840

                ffmpegIn.Write(imageData, 0, imageData.Length);

                //img.UnlockBits(bitmapData);

                
            }

            ffmpegIn.Flush();
            ffmpegIn.Close();

            process.WaitForExit();
            process.Dispose();


        }



        public byte[] ToByteArray1(Image image, ImageFormat format)
        {
            // not working with gray16
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }


        public byte[] ToByteArray2(Image img)
        {
            // not working with gray16
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }




        public byte[] ToByteArray3(Bitmap img)
        {
            var rect = new Rectangle(0, 0, img.Width, img.Height);
            var bitmapData = img.LockBits(rect, ImageLockMode.WriteOnly, img.PixelFormat);
            var ptr = bitmapData.Scan0;
            byte[] imageData = new byte[bitmapData.Stride * bitmapData.Height];
            Marshal.Copy(ptr, imageData, 0, imageData.Length);
            img.UnlockBits(bitmapData);

            return imageData;
        }


    }


}
