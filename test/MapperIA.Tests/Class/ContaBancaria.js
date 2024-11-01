class ContaBancaria {
    constructor(numero, titular, saldo = 0) {
        this.numero = numero;
        this.titular = titular;
        this.saldo = saldo;
    }

    depositar(valor) {
        if (valor > 0) {
            this.saldo += valor;
            return `Depósito de R$${valor} realizado com sucesso. Novo saldo: R$${this.saldo}.`;
        } else {
            return "Valor de depósito inválido.";
        }
    }

    sacar(valor) {
        if (valor > 0 && valor <= this.saldo) {
            this.saldo -= valor;
            return `Saque de R$${valor} realizado com sucesso. Saldo restante: R$${this.saldo}.`;
        } else {
            return "Saldo insuficiente ou valor de saque inválido.";
        }
    }

    consultarSaldo() {
        return `Saldo atual: R$${this.saldo}.`;
    }
}
