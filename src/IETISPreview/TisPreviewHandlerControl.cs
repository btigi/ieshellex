using SharpShell.SharpPreviewHandler;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using IETISPreview.Model;
using System.Xml.Serialization;
using System;
using System.Linq;

namespace IETISPreview
{
    public partial class TisPreviewHandlerControl : PreviewHandlerControl
    {
        public TisPreviewHandlerControl()
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
                var image = LoadTis(selectedFilePath);

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

        public Bitmap LoadTis(string filename)
        {
            var areaDatas = new List<AreaData>();
            var serializer = new XmlSerializer(areaDatas.GetType());
            var path = Path.Combine(Environment.GetEnvironmentVariable("ieshellex", EnvironmentVariableTarget.Machine), "config", "areadatas.xml");
            var areaData = new FileStream(path, FileMode.Open, FileAccess.Read);
            areaDatas = (List<AreaData>)serializer.Deserialize(areaData);

            var basefilename = Path.GetFileNameWithoutExtension(filename);
            var relevantAreaData = areaDatas.Where(w => w.Filename?.ToLower() == basefilename?.ToLower()).FirstOrDefault();
            relevantAreaData = relevantAreaData == null ? new AreaData(1, 1) : relevantAreaData;

            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            using (var br = new BinaryReader(fs))
            {
                var signature = string.Join("", br.ReadChars(4));
                var version = string.Join("", br.ReadChars(4));
                var tileCount = br.ReadInt32();
                var dataBlockLength = br.ReadInt32();
                var tileOffset = br.ReadInt32();
                var tileSize = br.ReadInt32();

                var blockArray = new List<(byte[] palette, byte[] data)>();

                if (version == "V1  " && signature == "TIS " && dataBlockLength == 0x1400)
                {
                    // read dataBlockLength bytes to read in one tile
                    for (int i = 0; i < tileCount; i++)
                    {
                        // Read palette
                        br.BaseStream.Seek(tileOffset + ((i * 256) * 4) + (i * (tileSize * tileSize)), SeekOrigin.Begin);
                        var palette = br.ReadBytes(256 * 4);

                        // Read image data
                        var data = br.ReadBytes(tileSize * tileSize);

                        blockArray.Add((palette, data));
                    }

                    var width = 64 * relevantAreaData.Width;
                    var height = 64 * relevantAreaData.Height;

                    var columns = width / tileSize;
                    var rows = height / tileSize;
                    var bytes = new byte[tileSize * tileSize * tileCount * 4];
                    // Now each Block has it's palette and data. We need to get that into DST[]
                    var pixelrow = 0;
                    var DSTIndex = 0;
                    for (int i = 0; i < rows; i++)
                    {
                        pixelrow = tileSize;

                        for (var k = 0; k < pixelrow; k++)
                        {

                            for (var j = 0; j < columns; j++)
                            {

                                var blockIndex = (columns * i) + j;
                                var pixelcol = tileSize;

                                for (var m = 0; m < pixelcol; m++)
                                {
                                    bytes[DSTIndex + 0] = blockArray[blockIndex].palette[blockArray[blockIndex].data[(k * pixelcol) + m] * 4 + 0]; // Blue
                                    bytes[DSTIndex + 1] = blockArray[blockIndex].palette[blockArray[blockIndex].data[(k * pixelcol) + m] * 4 + 1]; // Green;
                                    bytes[DSTIndex + 2] = blockArray[blockIndex].palette[blockArray[blockIndex].data[(k * pixelcol) + m] * 4 + 2]; // Red
                                    bytes[DSTIndex + 3] = 255;

                                    DSTIndex = DSTIndex + 4;
                                }
                            }
                        }
                    }

                    var img = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0));

                    //img.Save(@"D:\aa.bmp", ImageFormat.Bmp);
                    return img;
                }
                return new Bitmap(8, 8);
            }
        }
    }
}