using SharpShell.SharpPreviewHandler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ZLIB;

namespace IEBAMPreview
{
    public partial class BamPreviewHandlerControl : PreviewHandlerControl
    {
        public BamPreviewHandlerControl()
        {
            InitializeComponent();
        }

        private void AddIconsToControl()
        {
            BackColor = Color.White;

            DoubleBuffered = true;

            int yPos = 12;
            foreach (var iconImage in iconImages)
            {
                var descriptionLabel = new Label
                {
                    Location = new Point(12, yPos),
                    Text = string.Format("{0}x{1}",
                    iconImage.Size.Width, iconImage.Size.Height),
                    AutoSize = true,
                    BackColor = Color.White
                };
                yPos += 20;

                var pictureBox = new PictureBox
                {
                    Location = new Point(12, yPos),
                    Image = iconImage,
                    Width = iconImage.Size.Width,
                    Height = iconImage.Size.Height
                };
                yPos += iconImage.Size.Height + 20;
                panelImages.Controls.Add(descriptionLabel);
                panelImages.Controls.Add(pictureBox);

                generatedLabels.Add(descriptionLabel);
            }
        }

        /// <summary>
        /// Does the preview.
        /// </summary>
        /// <param name="selectedFilePath">The selected file path.</param>
        public void DoPreview(string selectedFilePath)
        {
            //  Load the icons.
            try
            {
                var images = LoadBam(selectedFilePath);

                foreach (var image in images)
                {
                    iconImages.Add(image);
                }

                AddIconsToControl();
            }
            catch
            {
                //  Maybe we could show something to the user in the preview
                //  window, but for now we'll just ignore any exceptions.
            }
        }

        // Sets the color of the background, if possible, to coordinate with the windows color scheme.
        protected override void SetVisualsBackgroundColor(Color color)
        {
            //  Set the background color.
            BackColor = color;
            generatedLabels.ForEach(gl => gl.BackColor = color);
        }

        // Sets the color of the text, if possible, to coordinate with the windows color scheme.
        protected override void SetVisualsTextColor(Color color)
        {
            generatedLabels.ForEach(gl => gl.ForeColor = color);
        }

        // Sets the font, if possible, to coordinate with the windows color scheme.
        protected override void SetVisualsFont(Font font)
        {
            generatedLabels.ForEach(gl => gl.Font = font);
        }

        private readonly List<Label> generatedLabels = new List<Label>();

        private readonly List<Bitmap> iconImages = new List<Bitmap>();

        public List<Bitmap> LoadBam(string filename)
        {
            File.AppendAllLines(@"D:\out\aa.txt", new List<string>() { "0" });

            using (var s = new MemoryStream())
            {
                using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                using (var br = new BinaryReader(fs))
                {
                    var signature = string.Join("", br.ReadChars(4));
                    var version = string.Join("", br.ReadChars(4));

                    if (version == "V1  " && signature == "BAM ")
                    {
                        fs.Position = 0;
                        fs.CopyTo(s);
                    }

                    if (version == "V1  " && signature == "BAMC")
                    {
                        var fi = new FileInfo(filename);

                        var uncompressedDataLength = br.ReadInt32();
                        var bytes = br.ReadBytes((int)fi.Length - 12);

                        var x = new MemoryStream(uncompressedDataLength);
                        x.Write(bytes, 0, bytes.Length);
                        x.Position = 0;
                        using (ZLIBStream zs = new ZLIBStream(x, CompressionMode.Decompress, true))
                        {
                            int bytesLeidos = 0;
                            byte[] buffer = new byte[1024];

                            while ((bytesLeidos = zs.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                s.Write(buffer, 0, bytesLeidos);
                            }
                        }
                    }
                }

                s.Position = 0;
                using (var br = new BinaryReader(s))
                {
                    return HandleBam(br);
                }
            }
        }

        private List<Bitmap> HandleBam(BinaryReader br)
        {
            File.AppendAllLines(@"D:\out\aa.txt", new List<string>() { "6" });

            var signature = string.Join("", br.ReadChars(4));
            var version = string.Join("", br.ReadChars(4));

            if (version == "V1  " && signature == "BAM ")
            {
                File.AppendAllLines(@"D:\out\aa.txt", new List<string>() { "5" });

                List<BamFrameEntryBinary> frameEntries = new List<BamFrameEntryBinary>();
                List<BamCycleEntryBinary> cycles = new List<BamCycleEntryBinary>();
                List<RGBA> palette = new List<RGBA>();
                List<ushort> frameLookups = new List<ushort>();
                List<byte[]> frames = new List<byte[]>();

                // header
                var frameEntryCount = br.ReadInt16();
                var cycleCount = br.ReadByte();
                var rleColourIndex = br.ReadByte();
                var frameEntryOffset = br.ReadInt32();
                var paletteOffset = br.ReadInt32();
                var frameLookupTableOffset = br.ReadInt32();

                br.BaseStream.Seek(frameEntryOffset, SeekOrigin.Begin);
                for (int i = 0; i < frameEntryCount; i++)
                {
                    var width = br.ReadInt16();
                    var height = br.ReadInt16();
                    var xCentre = br.ReadInt16();
                    var yCentre = br.ReadInt16();
                    var frameDataOffset = br.ReadInt32();

                    var frameEntry = new BamFrameEntryBinary
                    {
                        Width = width,
                        Height = height,
                        XCentre = xCentre,
                        YCentre = yCentre,
                        FrameDataOffset = frameDataOffset
                    };

                    frameEntries.Add(frameEntry);
                }

                // We're now in the right position to read cycles
                for (int i = 0; i < cycleCount; i++)
                {

                    var frameIndexCount = br.ReadInt16();
                    var frameIndexOffset = br.ReadInt16();

                    var cycle = new BamCycleEntryBinary()
                    {
                        FrameIndexCount = frameIndexCount,
                        FrameIndexOffset = frameIndexOffset
                    };

                    cycles.Add(cycle);
                }

                br.BaseStream.Seek(paletteOffset, SeekOrigin.Begin);
                for (int i = 0; i < 256; i++)
                {
                    var blue = br.ReadByte();
                    var green = br.ReadByte();
                    var red = br.ReadByte();
                    var alpha = br.ReadByte();

                    var paletteEntry = new RGBA() { Red = red, Green = green, Blue = blue, Alpha = alpha };

                    palette.Add(paletteEntry);
                }

                // We need to infer the number of framelookups by looking at the number referenced by cycles
                var count = 0;
                for (int i = 0; i < cycles.Count; i++)
                {
                    var tmp = cycles[i].FrameIndexOffset + cycles[i].FrameIndexCount;
                    if (tmp > count)
                    {
                        count = tmp;
                    }
                }

                br.BaseStream.Seek(frameLookupTableOffset, SeekOrigin.Begin);
                for (int i = 0; i < frameLookupTableOffset; i++)
                {
                    var flt = br.ReadUInt16();
                    frameLookups.Add(flt);
                }

                File.AppendAllLines(@"D:\out\aa.txt", new List<string>() { "7" });

                var result = new List<Bitmap>();
                for (int i = 0; i < frameEntries.Count; i++)
                {
                    br.BaseStream.Seek(frameEntries[i].FrameDataOffset & 0x7FFFFFFF, SeekOrigin.Begin);
                    ulong pixelCount = (ulong)(frameEntries[i].Height * frameEntries[i].Width);
                    var rleCompressed = (frameEntries[i].FrameDataOffset & 0x80000000) == 0;
                    byte[] pixels;
                    if (rleCompressed)
                    {
                        pixels = new byte[pixelCount];
                    }
                    else
                    {
                        pixels = new byte[pixelCount];
                    }

                    File.AppendAllLines(@"D:\out\aa.txt", new List<string>() { "8" });

                    br.BaseStream.Read(pixels, 0, (int)pixelCount);
                    if (rleCompressed)
                    {
                        var decodedPixels = new List<byte>();
                        for (int m = 0; m < pixels.Length; m++)
                        {
                            if (pixels[m] == rleColourIndex)
                            {
                                for (var runLength = 0; runLength <= (byte)pixels[m + 1 >= pixels.Length ? pixels.Length - 1 : m + 1]; runLength++)
                                {
                                    decodedPixels.Add(rleColourIndex);
                                }
                                m++;
                            }
                            else
                            {
                                decodedPixels.Add(pixels[m]);
                            }
                        }
                        pixels = decodedPixels.ToArray();
                    }

                    var data = new List<RGBA>();
                    foreach (var p in pixels)
                    {
                        data.Add(palette[p]);
                    }

                    File.AppendAllLines(@"D:\out\aa.txt", new List<string>() { "9" });

                    var size = frameEntries[i].Width * 4 * frameEntries[i].Height;
                    var xdata = new byte[size];
                    var cnt = 0;
                    for (int y = 0; y < frameEntries[i].Height; y++)
                    {
                        for (int x = 0; x < frameEntries[i].Width; x++)
                        {
                            xdata[cnt] = data[(y * frameEntries[i].Width) + x].Blue;
                            xdata[cnt + 1] = data[(y * frameEntries[i].Width) + x].Green;
                            xdata[cnt + 2] = data[(y * frameEntries[i].Width) + x].Red;
                            xdata[cnt + 3] = data[(y * frameEntries[i].Width) + x].Alpha;
                            cnt = cnt + 4;
                        }
                    }

                    File.AppendAllLines(@"D:\out\aa.txt", new List<string>() { "10" });

                    var img = new Bitmap(frameEntries[i].Width, frameEntries[i].Height, frameEntries[i].Width * 4, PixelFormat.Format32bppArgb, Marshal.UnsafeAddrOfPinnedArrayElement(xdata, 0));

                    File.AppendAllLines(@"D:\out\aa.txt", new List<string>() { "11" });

                    //Bitmap bitmap = new Bitmap(frameEntries[i].Width, frameEntries[i].Height);
                    //var bData = bitmap.LockBits(new Rectangle(0, 0, frameEntries[i].Width, frameEntries[i].Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                    //var size = bData.Stride * bData.Height;
                    //var xdata = new byte[size];
                    //Marshal.Copy(bData.Scan0, xdata, 0, size);
                    //var cnt = 0;
                    //for (int y = 0; y < frameEntries[i].Height; y++)
                    //{
                    //    for (int x = 0; x < frameEntries[i].Width; x++)
                    //    {
                    //        xdata[cnt] = data[(y * frameEntries[i].Width) + x].Blue;
                    //        xdata[cnt + 1] = data[(y * frameEntries[i].Width) + x].Green;
                    //        xdata[cnt + 2] = data[(y * frameEntries[i].Width) + x].Red;
                    //        xdata[cnt + 3] = data[(y * frameEntries[i].Width) + x].Alpha;
                    //        cnt = cnt + 4;
                    //    }
                    //}
                    //Marshal.Copy(xdata, 0, bData.Scan0, xdata.Length);
                    //bitmap.UnlockBits(bData);

                    //bitmap.Save(String.Format(@"D:\x\out_{0}.bmp", i), ImageFormat.Bmp); //TODO: temp

                    img.Save(String.Format(@"D:\x\out_{0}.bmp", i), ImageFormat.Bmp); //TODO: temp

                    File.AppendAllLines(@"D:\out\aa.txt", new List<string>() { "12" });

                    result.Add(img);
                }

                File.AppendAllLines(@"D:\out\aa.txt", new List<string>() { "xx" });

                return result;
            }
            return new List<Bitmap>() { new Bitmap(1, 1) };
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct BamFrameEntryBinary
        {
            public Int16 Width;
            public Int16 Height;
            public Int16 XCentre;
            public Int16 YCentre;
            public Int32 FrameDataOffset; // 0-30 - offset, 31 - IsNotRLECompressed
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct BamCycleEntryBinary
        {
            public Int16 FrameIndexCount;
            public Int16 FrameIndexOffset; // Index into FrameLookupTable
        }

        public class RGBA
        {
            public byte Red { get; set; }
            public byte Green { get; set; }
            public byte Blue { get; set; }
            public byte Alpha { get; set; }
        }
    }
}