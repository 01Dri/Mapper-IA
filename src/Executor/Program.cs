// See https://aka.ms/new-console-template for more information

using System.Text.Json.Serialization;
using ConvertersIA.Converters.Configuration;
using ConvertersIA.Interfaces;
using Executor;
using Extractors;
using MappersIA.PDFMapper;
using MappersIA.PDFMapper.Interfaces;


// MAPPER COM IA DE CLASSE PARA OUTRA CLASSE (FAZER)
// CRIAR PROJETO DE TESTES (PROVAVELMENTE xUnit)
Test model = new Test();
IAOptions iaOptions = new IAOptions()
{
    Key = Environment.GetEnvironmentVariable("GEMINI_KEY"),
    jsonSerializerOptions =
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        WriteIndented = true
    }
};
IConverterIA geminiConverter = new GeminiConverter(iaOptions);
IPDFMapper pdfMapper = new PDFMapper(geminiConverter, new PDFExtractor());
model = await pdfMapper.Map(model, @"F:\Downloads\Curriculo Atualizado (1).pdf");
Console.WriteLine(model.ToString());
 