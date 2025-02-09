namespace Vehicles.Application.Exceptions;

public class DomainMappingException(Type entityType, Type targetType, string propertyName, string message) 
    : Exception(message) { }