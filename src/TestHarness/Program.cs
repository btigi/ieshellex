using IEBAFInfotip;

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

            var SelectedItemPath = @"C:\tests\AR0903a31b.BAF";
            var infotip = new BafInfotipHandler();

            infotip.HandleRequest(SelectedItemPath, false);
        }
    }
}