
namespace IETISPreview.Model
{
    public class AreaData
    {
        public string Filename { get; set; }
        public long Filesize { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public AreaData() { }

        public AreaData(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}