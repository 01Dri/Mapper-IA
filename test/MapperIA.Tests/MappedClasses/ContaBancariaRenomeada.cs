using System;
namespace MapperIA.Tests.MappedClasses
{
    public class ContaBancariaRenomeada
    {
        public string Numero { get; set; }
        public string Titular { get; set; }
        public decimal Saldo { get; set; }
        public ContaBancariaRenomeada(string numero, string titular, decimal saldo = 0)
        {
            Numero = numero;
            Titular = titular;
            Saldo = saldo;
        }
        public string Depositar(decimal valor)
        {
            if (valor > 0)
            {
                Saldo += valor;
                return $"Depósito de R${valor:F2} realizado com sucesso. Novo saldo: R${Saldo:F2}.";
            }
            else
            {
                return "Valor de depósito inválido.";
            }
        }
        public string Sacar(decimal valor)
        {
            if (valor > 0 && valor <= Saldo)
            {
                Saldo -= valor;
                return $"Saque de R${valor:F2} realizado com sucesso. Saldo restante: R${Saldo:F2}.";
            }
            else
            {
                return "Saldo insuficiente ou valor de saque inválido.";
            }
        }
        public string ConsultarSaldo()
        {
            return $"Saldo atual: R${Saldo:F2}.";
        }
    }
}