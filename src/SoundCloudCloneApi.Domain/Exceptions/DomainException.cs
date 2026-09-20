namespace SoundCloudCloneApi.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityName, object key)
        : base($"{entityName} con identificador '{key}' no fue encontrado.") { }
}


public class ConflictException : DomainException
{
    public ConflictException(string message) : base(message) { }
}

