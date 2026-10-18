namespace RestaurantHub.API.DTOs;

public record ClienteDto(int IdCliente, string Nombre, string Apellido, string? Telefono, string? Correo);

// No incluye la contraseña: la API nunca la devuelve
public record UsuarioDto(int IdUsuario, string Nombre, string Apellido, string Correo, string Rol);
