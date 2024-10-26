using System;
public class Pessoa
{
    private string nome;
    private int idade;
    public Pessoa(string nome, int idade)
    {
        this.nome = nome;
        this.idade = idade;
    }
    public string GetNome()
    {
        return nome;
    }
    public int GetIdade()
    {
        return idade;
    }
    public bool IsMaiorDeIdade()
    {
        return idade >= 18;
    }
}