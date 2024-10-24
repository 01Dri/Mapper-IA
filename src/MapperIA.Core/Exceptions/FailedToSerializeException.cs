namespace MapperIA.Core.Exceptions;

public class FailedToSerializeException : Exception
{
    public FailedToSerializeException(string msg)
    :base(msg)
    {
        
    }
}