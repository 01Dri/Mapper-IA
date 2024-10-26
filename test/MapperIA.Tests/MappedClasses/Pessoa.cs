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
    public string Nome
    {
        get { return nome; }
    }
    public int Idade
    {
        get { return idade; }
    }
    public bool IsMaiorDeIdade()
    {
        return idade >= 18;
    }
}