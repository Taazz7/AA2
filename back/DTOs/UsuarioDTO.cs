namespace AA1.DTOs
{
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string UsuarioNombre { get; set; } = ""; // Antes Nombre
        public string Email { get; set; } = "";        // Antes Apellido
        public int Telefono { get; set; }
        public string Contraseña { get; set; } = "";   // Antes Direccion
        public string Rol { get; set; } = "";          // Antes FechaNac
    }

    public class CreateUsuarioDto
    {
        public string UsuarioNombre { get; set; } = "";
        public string Email { get; set; } = "";
        public int Telefono { get; set; }
        public string Contraseña { get; set; } = "";
        public string Rol { get; set; } = "";
    }

    public class UpdateUsuarioDto
    {
        public string UsuarioNombre { get; set; } = "";
        public string Email { get; set; } = "";
        public int Telefono { get; set; }
        public string Contraseña { get; set; } = "";
        public string Rol { get; set; } = "";
    }
}