namespace RestaurantHub.API.Exceptions;

// El recurso solicitado no existe (se responde 404)
public class NoEncontradoException : Exception
{
    public NoEncontradoException(string mensaje) : base(mensaje) { }
}

// La operación viola una regla de negocio (se responde 409)
public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string mensaje) : base(mensaje) { }
}
