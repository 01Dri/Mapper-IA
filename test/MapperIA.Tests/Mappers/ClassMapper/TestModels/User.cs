namespace Mapper_IA.Tests.Mappers.ClassMapper.ModelsTest;

public class User
{
    public long Id { get; set; }
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public List<Address> Enderecos { get; set; } = new List<Address>();
    public DateTime DataNascimento { get; set; }
    public bool Ativo { get; set; }
    public decimal Salario { get; set; }
    public string Cargo { get; set; }
    public List<Departament> Departamentos { get; set; } = new List<Departament>();

}