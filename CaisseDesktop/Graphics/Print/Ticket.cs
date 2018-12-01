using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using PrinterUtility.EscPosEpsonCommands;

namespace CaisseDesktop.Graphics.Print
{
    public abstract class Ticket
    {
        public byte[] BytesValue { get; set; }
        public EscPosEpson EscPosEpson { get; set; }
        public string Port { get; set; }

        protected Ticket(string port)
        {
            EscPosEpson = new EscPosEpson();
            BytesValue = Encoding.ASCII.GetBytes(string.Empty);
            Port = port;
        }

        public byte[] CutPage() => new[]
            {Convert.ToByte(Convert.ToChar(0x1D)), Convert.ToByte('V'), (byte) 66, (byte) 3};

        public void Print()
        {
            PrinterUtility.PrintExtensions.Print(BytesValue, Port);
        }

        public byte[] GetLogo(string logoPath)
        {
            var byteList = new List<byte>();
            if (!File.Exists(logoPath))
                return null;
            var data = GetBitmapData(logoPath);
            var dots = data.Dots;
            var width = BitConverter.GetBytes(data.Width);

            var offset = 0;
            //var stream = new MemoryStream();
            // BinaryWriter bw = new BinaryWriter(stream);
            byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
            //bw.Write((char));
            byteList.Add(Convert.ToByte('@'));
            //bw.Write('@');
            byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
            // bw.Write((char)0x1B);
            byteList.Add(Convert.ToByte('3'));
            //bw.Write('3');
            //bw.Write((byte)24);
            byteList.Add(24);
            while (offset < data.Height)
            {
                byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
                byteList.Add(Convert.ToByte('*'));
                //bw.Write((char)0x1B);
                //bw.Write('*');         // bit-image mode
                byteList.Add(33);
                //bw.Write((byte)33);    // 24-dot double-density
                byteList.Add(width[0]);
                byteList.Add(width[1]);
                //bw.Write(width[0]);  // width low byte
                //bw.Write(width[1]);  // width high byte

                for (var x = 0; x < data.Width; ++x)
                for (var k = 0; k < 3; ++k)
                {
                    byte slice = 0;
                    for (var b = 0; b < 8; ++b)
                    {
                        var y = (offset / 8 + k) * 8 + b;
                        // Calculate the location of the pixel we want in the bit array.
                        // It'll be at (y * width) + x.
                        var i = y * data.Width + x;

                        // If the image is shorter than 24 dots, pad with zero.
                        var v = false;
                        if (i < dots.Length) v = dots[i];
                        slice |= (byte) ((v ? 1 : 0) << (7 - b));
                    }

                    byteList.Add(slice);
                    //bw.Write(slice);
                }

                offset += 24;
                byteList.Add(Convert.ToByte(0x0A));
                //bw.Write((char));
            }

            // Restore the line spacing to the default of 30 dots.
            byteList.Add(Convert.ToByte(0x1B));
            byteList.Add(Convert.ToByte('3'));
            //bw.Write('3');
            byteList.Add(30);
            return byteList.ToArray();
            //bw.Flush();
            //byte[] bytes = stream.ToArray();
            //return logo + Encoding.Default.GetString(bytes);
        }

        public BitmapData GetBitmapData(string bmpFileName)
        {
            using (var bitmap = (Bitmap) Image.FromFile(bmpFileName))
            {
                var threshold = 127;
                var index = 0;
                double multiplier = 570; // this depends on your printer model. for Beiyang you should use 1000
                var scale = multiplier / bitmap.Width;
                var xheight = (int) (bitmap.Height * scale);
                var xwidth = (int) (bitmap.Width * scale);
                var dimensions = xwidth * xheight;
                var dots = new BitArray(dimensions);

                for (var y = 0; y < xheight; y++)
                for (var x = 0; x < xwidth; x++)
                {
                    var _x = (int) (x / scale);
                    var _y = (int) (y / scale);
                    var color = bitmap.GetPixel(_x, _y);
                    var luminance = (int) (color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                    dots[index] = luminance < threshold;
                    index++;
                }

                return new BitmapData
                {
                    Dots = dots,
                    Height = (int) (bitmap.Height * scale),
                    Width = (int) (bitmap.Width * scale)
                };
            }
        }

        public class BitmapData
        {
            public BitArray Dots { get; set; }

            public int Height { get; set; }

            public int Width { get; set; }
        }

        /**
         *  Generate content
         */

        public abstract void Generate();
    }
}