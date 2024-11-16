namespace MapperIA.Tests.MappedClasses;
public class ContaBancariaRenomeada
{
    public int Numero { get; set; }
    public string Titular { get; set; }
    public double Saldo { get; set; }
    public ContaBancariaRenomeada(int numero, string titular, double saldo = 0)
    {
        Numero = numero;
        Titular = titular;
        Saldo = saldo;
    }
    public string Depositar(double valor)
    {
        if (valor > 0)
        {
            Saldo += valor;
            return $"Depósito de R${valor} realizado com sucesso. Novo saldo: R${Saldo}.";
        }
        else
        {
            return "Valor de depósito inválido.";
        }
    }
    public string Sacar(double valor)
    {
        if (valor > 0 && valor <= Saldo)
        {
            Saldo -= valor;
            return $"Saque de R${valor} realizado com sucesso. Saldo restante: R${Saldo}.";
        }
        else
        {
            return "Saldo insuficiente ou valor de saque inválido.";
        }
    }
    public string ConsultarSaldo()
    {
        return $"Saldo atual: R${Saldo}.";
    }
}