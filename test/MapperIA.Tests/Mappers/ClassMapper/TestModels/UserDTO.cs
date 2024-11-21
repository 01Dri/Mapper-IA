namespace Mapper_IA.Tests.Mappers.ClassMapper.ModelsTest;

public class UserDTO
{
    
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Email { get; set; }
    public IEnumerable<AddressDTO> Address { get; set; }
    public bool Ativo { get; set; }
    public decimal Salario { get; set; }
    
    public DateTime DataNascimento { get; set; }

    public List<DepartmentDTO> Departments { get; set; }

    public List<string>  AddressStreetsNames { get; set; }



}