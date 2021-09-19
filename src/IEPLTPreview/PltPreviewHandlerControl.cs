using SharpShell.SharpPreviewHandler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace IEPLTPreview
{
    public partial class PltPreviewHandlerControl : PreviewHandlerControl
    {
        public PltPreviewHandlerControl()
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
                var image = LoadPlt(selectedFilePath);

                iconImages.Add(image);

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

        public Bitmap LoadPlt(string filename)
        {
            using (var s = new MemoryStream())
            {
                using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                using (var br = new BinaryReader(fs))
                {
                    var signature = string.Join("", br.ReadChars(4));
                    var version = string.Join("", br.ReadChars(4));

                    if (version == "V1  " && signature == "PLT ")
                    {
                        fs.Position = 0;
                        fs.CopyTo(s);
                    }
                }

                s.Position = 0;
                using (var br = new BinaryReader(s))
                {
                    return HandlePlt(br);
                }
            }
        }

        private Bitmap HandlePlt(BinaryReader br)
        {
            var signature = string.Join("", br.ReadChars(4));
            var version = string.Join("", br.ReadChars(4));
            var unknown1 = br.ReadInt16();
            var unknown2 = br.ReadInt16();
            var unknown3 = br.ReadInt16();
            var unknown4 = br.ReadInt16();
            var width = br.ReadInt32();
            var height = br.ReadInt32();

            if (version == "V1  " && signature == "PLT ")
            {
                var path = Path.Combine(Environment.GetEnvironmentVariable("ieshellex", EnvironmentVariableTarget.Machine), "config", "MPAL256.BMP");
                var palette = new Bitmap(path);
                var colours = new List<Color>();

                for (int i = 0; i < width * height; i++)
                {
                    // Read palette
                    var column = br.ReadByte();
                    var row = br.ReadByte();

                    var colour = palette.GetPixel(column, row);
                    colours.Add(colour);
                }

                var bytes = new byte[width * height * 4];
                var idx = 0;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var colourIndex = x + (y * width);
                        bytes[idx + 0] = colours[colourIndex].B; // Blue
                        bytes[idx + 1] = colours[colourIndex].G; // Green
                        bytes[idx + 2] = colours[colourIndex].R; // Red
                        bytes[idx + 3] = 255;

                        idx += 4;
                    }
                }


                var img = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0));
                img.RotateFlip(RotateFlipType.RotateNoneFlipY);

                //img.Save(@"D:\aa.bmp", ImageFormat.Bmp);
                return img;
            }

            return new Bitmap(1, 1);
        }
    }
}