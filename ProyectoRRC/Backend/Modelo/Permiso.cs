using ProyectoRRC.Frontend.MVVM;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRRC.Backend.Modelo;

public partial class Permiso: PropertyChangedDataError
{
    public int IdPermiso { get; set; }

    [MaxLength(100, ErrorMessage = "Como máximo 100 carácteres")]
    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Role> IdRols { get; set; } = new List<Role>();

    override
        public string ToString()
    {
        return IdPermiso+" - "+Descripcion;
    }
}
