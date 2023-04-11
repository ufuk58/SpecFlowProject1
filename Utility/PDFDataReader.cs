using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Layout;
using SpecFlowProject1.Support;

namespace SpecFlowProject1.Utility
{
    public class PDFDataReader
    {

        public static IList<string> DownloadedFileNames()
        {
            IList<string> fileNames = Directory.GetFiles(Hooks1.downloads);
            return fileNames;
        }

        public static string ReadDownloadedPDF()
        {
            string path = GenericHelper.DirectoryPath("Downloads") + DownloadedFileNames()[0];
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(path));
            Document doc =new  Document(pdfDoc);
            string text = "";

            for(int i=0; i < pdfDoc.GetNumberOfPages(); i++)
            {
                var page = pdfDoc.GetPage(i);
                var pageText = PdfTextExtractor.GetTextFromPage(page);
                pageText.Trim();
                text += pageText + "\n";
            }
            pdfDoc.Close();
            return text;
        }
        public static string ReadPDF(string path)
        {
            
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(path));
            Document doc = new Document(pdfDoc);
            string text = "";

            for (int i = 0; i < pdfDoc.GetNumberOfPages(); i++)
            {
                var page = pdfDoc.GetPage(i);
                var pageText = PdfTextExtractor.GetTextFromPage(page);
                pageText.Trim();
                text += pageText + "\n";
            }
            pdfDoc.Close();
            return text;
        }

    }
}
