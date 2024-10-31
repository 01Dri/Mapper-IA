public class Carro {
    private String marca;
    private String modelo;
    private int ano;

    public Carro(String marca, String modelo, int ano) {
        this.marca = marca;
        this.modelo = modelo;
        this.ano = ano;
    }

    public String getMarca() {
        return marca;
    }
    
    public String getModelo() {
        return modelo;
    }

    public int getAno() {
        return ano;
    }

    public void exibirInformacoes() {
        System.out.println("Marca: " + marca);
        System.out.println("Modelo: " + modelo);
        System.out.println("Ano: " + ano);
    }

    public boolean isAntigo() {
        int anoAtual = java.util.Calendar.getInstance().get(java.util.Calendar.YEAR);
        return (anoAtual - ano) > 20;
    }

}
