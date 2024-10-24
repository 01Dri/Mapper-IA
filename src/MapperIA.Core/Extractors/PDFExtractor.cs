using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using MapperIA.Core.Interfaces;
using Newtonsoft.Json;

namespace MapperIA.Core.Extractors;

public class PDFExtractor : IExtractor
{
    public string ExtractContent(string pdfPath)
    {
        if (string.IsNullOrEmpty(pdfPath))
        {
            throw new ArgumentException("The PDF path cannot be null or empty.", nameof(pdfPath));
        }

        if (!File.Exists(pdfPath))
        {
            throw new FileNotFoundException("The specified PDF file does not exist.", pdfPath);
        }

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
        return string.Join(" ", input.Split(new[] { '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries))
            .Replace("\\n", " ")
            .Replace("\\r", " ")
            .Trim();
    }
}