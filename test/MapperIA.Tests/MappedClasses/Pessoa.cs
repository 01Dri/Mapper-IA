using System;
namespace MapperIA.Tests.MappedClasses;
public class Pessoa
{
    public string Nome { get; set; }
    public int Idade { get; set; }
    public Pessoa(string nome, int idade)
    {
        Nome = nome;
        Idade = idade;
    }
    public bool IsMaiorDeIdade()
    {
        return Idade >= 18;
    }
}