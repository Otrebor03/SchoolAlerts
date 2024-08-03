using ProyectoRRC.Frontend.MVVM;

namespace ProyectoRRC.Backend.Modelo;

public partial class Profesor: PropertyChangedDataError
{
    public int IdProfesor { get; set; }

    public int IdPersona { get; set; }

    public virtual ICollection<Amonestacione> AmonestacioneIdProfesorHechoNavigations { get; set; } = new List<Amonestacione>();

    public virtual ICollection<Amonestacione> AmonestacioneIdProfesorRegistraNavigations { get; set; } = new List<Amonestacione>();

    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    public virtual ICollection<Grupo> IdGrupos { get; set; } = new List<Grupo>();
    
    override
        public string ToString()
    {
        return IdPersonaNavigation.Nombre + " " + IdPersonaNavigation.Apellido1 + " " + IdPersonaNavigation.Apellido2 + " (DNI: " + IdPersonaNavigation.Dni+")";
    }
}
