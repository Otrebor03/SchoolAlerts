using ProyectoRRC.Frontend.MVVM;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRRC.Backend.Modelo;

public partial class Role : PropertyChangedDataError
{
    public int IdRol { get; set; }

    [Required(ErrorMessage = "Campo Requerido")]
    [MaxLength(45, ErrorMessage = "Como máximo 45 carácteres")]
    public string NombreRol { get; set; } = null!;

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();

    public virtual ICollection<Permiso> IdPermisos { get; set; } = new List<Permiso>();
    
    override
    public string ToString()
    {
        return NombreRol;
    }
}
