namespace MapperIA.Tests.MappedClasses;
public class Carro
{
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Ano { get; set; }
    public Carro(string marca, string modelo, int ano)
    {
        Marca = marca;
        Modelo = modelo;
        Ano = ano;
    }
    public void ExibirInformacoes()
    {
        Console.WriteLine("Marca: " + Marca);
        Console.WriteLine("Modelo: " + Modelo);
        Console.WriteLine("Ano: " + Ano);
    }
    public bool IsAntigo()
    {
        int anoAtual = DateTime.Now.Year;
        return (anoAtual - Ano) > 20;
    }
}