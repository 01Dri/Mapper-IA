using MapperIA.Core.Configuration;

namespace MapperIA.Core.Interfaces;

public interface IFileClassMapper
{
    Task<string> Map
    (
        FileClassMapperConfiguration configuration
    );

}