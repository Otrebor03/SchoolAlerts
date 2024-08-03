using ProyectoRRC.Frontend.MVVM;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRRC.Backend.Modelo;

public partial class Motivo: PropertyChangedDataError
{
    public int IdMotivo { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    [MaxLength(45, ErrorMessage = "Como máximo 45 carácteres")]
    public string Motivo1 { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "Como máximo 100 carácteres")]
    public string? Descripcion { get; set; }

    public virtual ICollection<Amonestacione> Amonestaciones { get; set; } = new List<Amonestacione>();
}
