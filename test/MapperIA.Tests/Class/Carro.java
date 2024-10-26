public class Carro {
    // Atributos da classe
    private String marca;
    private String modelo;
    private int ano;

    // Construtor
    public Carro(String marca, String modelo, int ano) {
        this.marca = marca;
        this.modelo = modelo;
        this.ano = ano;
    }

    // Métodos getters
    public String getMarca() {
        return marca;
    }

    public String getModelo() {
        return modelo;
    }

    public int getAno() {
        return ano;
    }

    // Método para exibir informações do carro
    public void exibirInformacoes() {
        System.out.println("Marca: " + marca);
        System.out.println("Modelo: " + modelo);
        System.out.println("Ano: " + ano);
    }

    // Método para verificar se o carro é antigo
    public boolean isAntigo() {
        int anoAtual = java.util.Calendar.getInstance().get(java.util.Calendar.YEAR);
        return (anoAtual - ano) > 20;
    }

}
