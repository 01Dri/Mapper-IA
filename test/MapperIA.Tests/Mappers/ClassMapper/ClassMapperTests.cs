﻿using Mapper_IA.Tests.Mappers.ClassMapper.ModelsTest;
using MapperIA.Core.Configuration;
using MapperIA.Core.Converters.Gemini;
using MapperIA.Core.Enums.ModelsIA;
using MapperIA.Core.Interfaces;

namespace MapperIA.Tests.Mappers.ClassMapper;

public class ClassMapperTests
{
    private readonly IMapper _classMapper;

    public  ClassMapperTests()
    {
        ConverterConfiguration converterConfiguration = new ConverterConfiguration(Environment.GetEnvironmentVariable("GEMINI_KEY"), GeminiModels.FLASH_1_5_PRO.GetValue());
        IConverterIA geminiConverter = new GeminiConverter(converterConfiguration);
        _classMapper = new MapperIA.Core.Mappers.ClassMapper.ClassMapper(geminiConverter);
    }

    [Fact]
    public async Task Test_ClassMapper_User_To_UserDTO()
    {
        var user = new User()
        {
            Nome = "Diego Henrique",
            Idade = 30,
            Email = "diego@example.com",
            Telefone = "123456789",
            Enderecos = this.GetAddress(),
            DataNascimento = new DateTime(1994, 1, 1),
            Ativo = true,
            Salario = 5000.00m,
            Cargo = "Desenvolvedor",
            Departamentos = this.GetDepartments()
        };
        UserDTO userDto = await _classMapper.Map<User, UserDTO>(user);
        
        Assert.Equal("Diego Henrique", userDto.Nome);
        Assert.Equal(30, userDto.Idade);
        Assert.Equal("diego@example.com", userDto.Email);
        Assert.True(userDto.Ativo);
        Assert.Equal(5000.00m, userDto.Salario);
        Assert.Equal(user.DataNascimento, userDto.DataNascimento);
        Assert.Equal(user.Enderecos.Count, userDto.Address.Count());
        Assert.Equal(user.Departamentos.Count, userDto.Departments.Count);

    }


    private List<Address> GetAddress()
    {
        return  new List<Address>()
        {
            new Address()
            {
                Id = 1,
                Numero = "1550",
                Rua = "rua teste"
            },
            new Address()
            {
                Id = 2,
                Numero = "1560",
                Rua = "rua teste2"
            }
        };
        
    }

    private List<Departament> GetDepartments()
    {
        return new List<Departament>()
        {
            new Departament()
            {
                Id = 1,
                Nome = "departamento teste1"
            },
            new Departament()
            {
                Id = 2,
                Nome = "departamento teste2"
            },

        };
            
    }
    
}