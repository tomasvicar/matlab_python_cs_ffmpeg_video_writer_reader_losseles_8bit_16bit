using BitMiracle.LibTiff.Classic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_ffmpeg_16bit
{
    class TiffWriter
    {


        public void writeTiff(Bitmap img, string fileName)
        {

            using (Tiff tif = Tiff.Open(fileName, "w"))
            {
                tif.SetField(TiffTag.IMAGEWIDTH, img.Width);
                tif.SetField(TiffTag.IMAGELENGTH, img.Height);
                
                tif.SetField(TiffTag.ROWSPERSTRIP, img.Height);
                tif.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG);
                tif.SetField(TiffTag.COMPRESSION, Compression.LZW);
                tif.SetField(TiffTag.FILLORDER, FillOrder.MSB2LSB);


                byte[] raster = getImageRasterBytes(img, img.PixelFormat);

                //var bits = Image.GetPixelFormatSize(img.PixelFormat)
                //var bytes = bits / 8;

                if (img.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    tif.SetField(TiffTag.SAMPLESPERPIXEL, 3);
                    tif.SetField(TiffTag.BITSPERSAMPLE, 8);
                    tif.SetField(TiffTag.PHOTOMETRIC, Photometric.RGB);
                }
                else if (img.PixelFormat == PixelFormat.Format16bppGrayScale)
                {
                    tif.SetField(TiffTag.SAMPLESPERPIXEL, 1);
                    tif.SetField(TiffTag.BITSPERSAMPLE, 16);
                    tif.SetField(TiffTag.PHOTOMETRIC, Photometric.MINISBLACK);
                }
                else
                {
                    throw new NotImplementedException("this format is not implemented");
                    // https://github.com/BitMiracle/libtiff.net/tree/master/Samples
                }


                int stride = raster.Length / img.Height;

                for (int i = 0, offset = 0; i < img.Height; i++)
                {
                    tif.WriteScanline(raster, offset, i, 0);
                    offset += stride;
                }


            }

            System.Diagnostics.Process.Start(fileName);

        }




        private static byte[] getImageRasterBytes(Bitmap bmp, PixelFormat format)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            byte[] bits = null;


            // Lock the managed memory
            BitmapData bmpdata = bmp.LockBits(rect, ImageLockMode.ReadWrite, format);

            // Declare an array to hold the bytes of the bitmap.
            bits = new byte[bmpdata.Stride * bmpdata.Height];

            // Copy the values into the array.
            System.Runtime.InteropServices.Marshal.Copy(bmpdata.Scan0, bits, 0, bits.Length);

            // Release managed memory
            bmp.UnlockBits(bmpdata);
           

            return bits;
        }




        //private static void convertSamplesRGB(byte[] data, int width, int height)
        //{
        //    /// Converts BGR samples into RGB samples
        //    int stride = data.Length / height;
        //    const int samplesPerPixel = 3;

        //    for (int y = 0; y < height; y++)
        //    {
        //        int offset = stride * y;
        //        int strideEnd = offset + width * samplesPerPixel;

        //        for (int i = offset; i < strideEnd; i += samplesPerPixel)
        //        {
        //            byte temp = data[i + 2];
        //            data[i + 2] = data[i];
        //            data[i] = temp;
        //        }
        //    }
        //}


    }
}
