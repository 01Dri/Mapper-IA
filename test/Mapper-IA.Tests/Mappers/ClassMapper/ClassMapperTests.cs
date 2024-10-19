
using ConvertersIA.Converters.Configuration;
using ConvertersIA.Interfaces;
using MappersIA.PDFMapper.Interfaces;

namespace Mapper_IA.Tests.Mappers.ClassMapper;

public class ClassMapperTests
{
    private readonly IClassMapper _classMapper;

    public  ClassMapperTests()
    {
        IAOptions iaOptions = new IAOptions()
        {
            Key = Environment.GetEnvironmentVariable("GEMINI_KEY")
        };
        IConverterIA geminiConverter = new GeminiConverter(iaOptions);
        _classMapper = new global::Mappers.ClassMapper.ClassMapper(geminiConverter);
    }

    [Fact]
    public async Task Test()
    {
        var test1 = new Test1
        {
            Nome = "Diego Henrique",
            Idade = 30,
            Email = "diego@example.com",
            Telefone = "123456789",
            Endereco = "Rua Exemplo, 123",
            DataNascimento = new DateTime(1994, 1, 1),
            Ativo = true,
            Salario = 5000.00m,
            Cargo = "Desenvolvedor",
            Departamento = "TI",
            Test3s = new List<Test3>(){new Test3()
            {
                Id = 1,
                Admin = true,
                Setor = "Testee"
            }}
        };
        
        Test2 test2 = await _classMapper.Map<Test1, Test2>(test1);
        
        Assert.Equal("Diego Henrique", test2.Nome);
        Assert.Equal(30, test2.Idade);
        Assert.Equal("diego@example.com", test2.Email);
        Assert.True(test2.Ativo);
        Assert.Equal(5000.00m, test2.Salario);
        Assert.Equal(test1.DataNascimento, test2.DataNascimento);
        Assert.Equal(test1.Test3s.Count, test2.Test3Dtos.Count);

        
    }
    
}

class Test1
{
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; }
        public decimal Salario { get; set; }
        public string Cargo { get; set; }
        public string Departamento { get; set; }
        public List<Test3> Test3s { get; set; } = new List<Test3>();


}

class Test2
{
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }
    public bool Ativo { get; set; }
    public decimal Salario { get; set; }
    public DateTime DataNascimento { get; set; }

    public List<Test3DTO> Test3Dtos { get; set; } = new List<Test3DTO>();
}

class Test3
{
    public int Id { get; set; }
    public string Setor { get; set; }
    public bool Admin { get; set; }
}

class  Test3DTO 
{
    public string Setor { get; set; }
    public bool Admin { get; set; }
}