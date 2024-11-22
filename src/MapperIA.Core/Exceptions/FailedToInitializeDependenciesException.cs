namespace MapperIA.Core.Exceptions;

public class FailedToInitializeDependenciesException : Exception
{
    public FailedToInitializeDependenciesException(string msg)
    :base(msg)
    {
    }
}