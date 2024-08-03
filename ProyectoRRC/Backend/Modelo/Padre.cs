using ProyectoRRC.Frontend.MVVM;

namespace ProyectoRRC.Backend.Modelo;

public partial class Padre: PropertyChangedDataError
{
    public int IdPadre { get; set; }

    public int IdPersona { get; set; }

    public virtual ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();

    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    override
        public string ToString()
    {
        return IdPersonaNavigation.Nombre + " " + IdPersonaNavigation.Apellido1 + " " + IdPersonaNavigation.Apellido2 + " (DNI: " + IdPersonaNavigation.Dni + ")";
    }
}
