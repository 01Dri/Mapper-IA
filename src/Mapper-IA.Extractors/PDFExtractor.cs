using Extractors.Interfaces;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Newtonsoft.Json;

namespace Extractors;

public class PDFExtractor : IPDFExtractor
{
    public string ExtractContent(string pdfPath)
    {

        var pdfReader = new PdfReader(pdfPath);
        var pdfDoc = new PdfDocument(pdfReader);
        var extractedData = new List<string>();

        for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
        {
            var page = pdfDoc.GetPage(i);
            string text = PdfTextExtractor.GetTextFromPage(page);
            string cleanedText = CleanText(text);

            extractedData.Add(cleanedText);
        }

        return JsonConvert.SerializeObject(extractedData, Formatting.Indented);
    }

    private string CleanText(string input)
    {
        return string.Join(" ", input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            .Replace("\\n", " ")
            .Replace("\\r", " ")
            .Trim();
    }
}