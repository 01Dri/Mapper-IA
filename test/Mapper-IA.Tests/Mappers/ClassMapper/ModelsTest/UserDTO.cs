namespace Mapper_IA.Tests.Mappers.ClassMapper.ModelsTest;

public class UserDTO
{
    
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public List<AddressDTO> Address { get; set; } = new List<AddressDTO>();
    public bool Ativo { get; set; }
    public decimal Salario { get; set; }
    
    public string Cargo { get; set; }
    
    public DateTime DataNascimento { get; set; }

    public List<DepartmentDTO> Departments { get; set; } = new List<DepartmentDTO>();



}