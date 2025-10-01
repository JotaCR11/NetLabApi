namespace Netlab.Domain.Entities;

public class Usuario : General
{
    public int IdUsuario { get; set; }
    public string Login { get; set; }
    public byte[] Contrasenia { get; set; }
    public string Correo { get; set; }
    public string Nombres { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
    public int Estado { get; set; }
    public int Respuesta { get; set; }
    public DateTime fechaIngreso { get; set; }
    public DateTime fechaUltimoAcceso { get; set; }
    public DateTime fechaCaducidad { get; set; }
    public List<Rol> Roles { get; set; } = new();
}

public class Rol : General
{
    public int IdRol { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string Tipo { get; set; }
}

public class UsuarioRol : General
{
    public int IdUsuario { get; set; }
    public int IdRol { get; set; }
}

public class MenuRol : General
{
    public int IdMenu { get; set; }
    public int IdRol { get; set; }
}

public class Menu : General
{
    public int IdMenu { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string URL { get; set; }
    public string Icon { get; set; }
    public int IdMenuPadre { get; set; }
    public int Orden { get; set; }
    public List<Menu> Hijos { get; set; }
}

public class UsuarioAreaProcesamiento : General
{
    public int IdUsuario { get; set; }
    public int IdAreaProcesamiento { get; set; }
}

public class UsuarioEstablecimiento : General
{
    public int? IdUsuario { get; set; }
    public string IdDISA { get; set; }
    public string IdRed { get; set; }
    public string IdMicrored { get; set; }
    public int? IdEstablecimiento { get; set; }
    public string NomDisa { get; set; }
    public string NomRed { get; set; }
    public string NomMicrored { get; set; }
    public string NomEstablecimiento { get; set; }
    public string NomInstitucion { get; set; }
    public int? IdInstitucion { get; set; }
}

public class UsuarioLaboratorio : General
{
    public int? IdUsuario { get; set; }
    public int? IdDISA { get; set; }
    public int? IdRed { get; set; }
    public int? IdMicrored { get; set; }
    public int? IdLaboratorio { get; set; }
    public string NomDisa { get; set; }
    public string NomRed { get; set; }
    public string NomMicrored { get; set; }
    public string NomLaboratorio { get; set; }
    public string NomInstitucion { get; set; }
    public int? IdInstitucion { get; set; }
}

public class Examen : General
{
    public string IdExamen { get; set; }
    public string Nombre { get; set; }
}