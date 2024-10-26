using System;
public class Carro
{
    private string marca;
    private string modelo;
    private int ano;
    public Carro(string marca, string modelo, int ano)
    {
        this.marca = marca;
        this.modelo = modelo;
        this.ano = ano;
    }
    public string Marca
    {
        get { return marca; }
    }
    public string Modelo
    {
        get { return modelo; }
    }
    public int Ano
    {
        get { return ano; }
    }
    public void ExibirInformacoes()
    {
        Console.WriteLine("Marca: " + marca);
        Console.WriteLine("Modelo: " + modelo);
        Console.WriteLine("Ano: " + ano);
    }
    public bool IsAntigo()
    {
        int anoAtual = DateTime.Now.Year;
        return (anoAtual - ano) > 20;
    }
}