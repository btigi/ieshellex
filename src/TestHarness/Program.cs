using IEBAMPreview;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            //var SelectedItemPath = @"C:\tests\ar20pb.mos";  // compressed
            //var SelectedItemPath = @"C:\tests\X#CH12.MOS";  // uncompressed
            //var infotip = new MosPreviewHandlerControl();
            //infotip.LoadMos(SelectedItemPath);

            var SelectedItemPath = @"D:\eee.bam";
            var infotip = new BamPreviewHandlerControl();

            infotip.DoPreview(SelectedItemPath);
        }
    }
}