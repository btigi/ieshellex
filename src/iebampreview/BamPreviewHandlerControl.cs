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
                iconImages.AddRange(images);

                AddIconsToControl();
            }
            catch (Exception ex)
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
            var signature = string.Join("", br.ReadChars(4));
            var version = string.Join("", br.ReadChars(4));

            if (version == "V1  " && signature == "BAM ")
            {
                var frameEntries = new List<BamFrameEntryBinary>();
                var cycles = new List<BamCycleEntryBinary>();
                var palette = new List<RGBA>();
                var frameLookups = new List<ushort>();
                var frames = new List<Stream>();

                // header
                var frameEntryCount = br.ReadInt16();
                var cycleCount = br.ReadByte();
                var rleColourIndex = br.ReadByte();
                var frameEntryOffset = br.ReadInt32();
                var paletteOffset = br.ReadInt32();
                var frameLookupTableOffset = br.ReadInt32();
                var frameLookupTableCount = 0;

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

                    // We need to track the highest frame lookup index referenced, as this info isn't stored in the BAM file directly
                    if (frameIndexCount + frameIndexOffset > frameLookupTableCount)
                    {
                        frameLookupTableCount = frameIndexCount + frameIndexOffset;
                    }

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

                br.BaseStream.Seek(frameLookupTableOffset, SeekOrigin.Begin);
                for (int i = 0; i < frameLookupTableCount; i++)
                {
                    var flt = br.ReadUInt16();
                    frameLookups.Add(flt);
                }

                var result = new List<Bitmap>();
                for (int i = 0; i < frameEntries.Count; i++)
                {
                    br.BaseStream.Seek(frameEntries[i].FrameDataOffset & 0x7FFFFFFF, SeekOrigin.Begin);
                    ulong pixelCount = (ulong)(frameEntries[i].Height * frameEntries[i].Width);
                    var rleCompressed = (frameEntries[i].FrameDataOffset & 0x80000000) == 0;
                    var pixels = new byte[pixelCount];

                    br.BaseStream.Read(pixels, 0, (int)pixelCount);
                    if (rleCompressed)
                    {
                        // RLE encoding: 
                        // If the byte is the transparent index, read the next byte (x) and output (x+1) copies of the transarent colour
                        // If the byte is not the transparent index, it represents itself
                        // e.g. for a transparent colour of 0 the values 1203 would indicate
                        // 1x colour 1, 1x colour 2, 4x transparent colour

                        var decodedPixels = new List<byte>();
                        for (int m = 0; m < pixels.Length; m++)
                        {
                            // We need to stop when we've read/calculated all the pixels required to make this frame
                            if (decodedPixels.Count == (int)pixelCount)
                            {
                                pixels = decodedPixels.ToArray();
                                break;
                            }

                            if (pixels[m] == rleColourIndex)
                            {
                                var rlePixelCount = 1;

                                if (m + 1 < pixels.Length)
                                {
                                    rlePixelCount = pixels[m + 1] + 1;
                                }
                                for (var runLength = 0; runLength < rlePixelCount; runLength++)
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

                        // We need to stop when we've read/calculated all the pixels required to make this frame
                        if (decodedPixels.Count == pixels.Length)
                        {
                            pixels = decodedPixels.ToArray();
                        }
                        else
                        {
                            // We read the decoded data into the expected location (pixels) byte by byte as 
                            // copying the entire thing over results in an exception
                            for (int idx = 0; idx < decodedPixels.Count; idx++)
                            {
                                pixels[idx] = decodedPixels[idx];
                            }
                        }
                    }

                    var data = new List<RGBA>();
                    foreach (var p in pixels)
                    {
                        data.Add(palette[p]);
                    }

                    var size = frameEntries[i].Width * 4 * frameEntries[i].Height;
                    var xdata = new byte[size];
                    var cnt = 0;
                    for (int y = 0; y < frameEntries[i].Height; y++)
                    {
                        for (int x = 0; x < frameEntries[i].Width; x++)
                        {
                            var alpha = data[(y * frameEntries[i].Width) + x].Alpha;
                            xdata[cnt] = data[(y * frameEntries[i].Width) + x].Blue;
                            xdata[cnt + 1] = data[(y * frameEntries[i].Width) + x].Green;
                            xdata[cnt + 2] = data[(y * frameEntries[i].Width) + x].Red;
                            xdata[cnt + 3] = alpha == (byte)0 ? (byte)255 : alpha;
                            cnt += 4;
                        }
                    }

                    // Saving the bitmap image directly to a frames array here results in the image being corrupt
                    // when we next access it, however if we save the bitmap to a stream and save the stream,
                    // everything works fine, if a little inefficiently
                    var s = new MemoryStream();
                    var img = new Bitmap(frameEntries[i].Width, frameEntries[i].Height, frameEntries[i].Width * 4, PixelFormat.Format32bppArgb, Marshal.UnsafeAddrOfPinnedArrayElement(xdata, 0));
                    img.Save(s, ImageFormat.Bmp);
                    frames.Add(s);
                }


                foreach (var cycle in cycles)
                {
                    for (int frameIndexCount = 0; frameIndexCount < cycle.FrameIndexCount; frameIndexCount++)
                    {
                        var frameLookupIndex = cycle.FrameIndexOffset + frameIndexCount;
                        var frameIndex = frameLookups[frameLookupIndex];
                        var frame = frames[frameIndex];
                        var bitmap = new Bitmap(frame);
                        result.Add(bitmap);
                    }
                }

                return result;
            }

            return new List<Bitmap>() { new Bitmap(1, 1) };
        }

        private class BamFrameEntryBinary
        {
            public Int16 Width;
            public Int16 Height;
            public Int16 XCentre;
            public Int16 YCentre;
            public Int32 FrameDataOffset; // 0-30 - offset, 31 - IsNotRLECompressed
        }

        private class BamCycleEntryBinary
        {
            public Int16 FrameIndexCount;
            public Int16 FrameIndexOffset; // Index into FrameLookupTable
        }

        private class RGBA
        {
            public byte Red { get; set; }
            public byte Green { get; set; }
            public byte Blue { get; set; }
            public byte Alpha { get; set; }
        }
    }
}