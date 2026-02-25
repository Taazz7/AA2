namespace Models;

public class Usuario {

    public int IdUsuario { get; set; }
    public string UsuarioNombre { get; set; } = ""; // Mapea a 'usuario' en SQL
    public string Email { get; set; } = "";
    public int Telefono { get; set; } 
    public string Contraseña { get; set; } = ""; // Mapea a 'contraseña' en SQL
    public string Rol { get; set; } = "";

    public Usuario(){}

    public Usuario(string usuarioNombre, string email, int telefono, string contraseña, string rol) {
        UsuarioNombre = usuarioNombre;
        Email = email;
        Telefono = telefono;
        Contraseña = contraseña;
        Rol = rol;
    }
}