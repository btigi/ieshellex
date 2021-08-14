using IEBAMPreview;
using IETISPreview;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            //var SelectedItemPath = @"C:\tests\ar20pb.mos";  // compressed
            //var SelectedItemPath = @"C:\tests\X#CH12.MOS";  // uncompressed

            var SelectedItemPath = @"D:\tis3\AR1000.TIS";  // uncompressed
            
            var tisPreview = new TisPreviewHandlerControl();
            var bitmap = tisPreview.LoadTis(SelectedItemPath);
            bitmap.Save(@"D:\tis3\out.bmp");
        }
    }
}