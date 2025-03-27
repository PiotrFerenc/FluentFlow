namespace FluentFlow.Console.Exceptions;

public class DatabaseProviderNotSupportException(string name) : Exception($"Database provider {name} is not supported.");