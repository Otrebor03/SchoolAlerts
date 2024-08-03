using ProyectoRRC.Frontend.MVVM;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRRC.Backend.Modelo;

public partial class Amonestacione: PropertyChangedDataError
{
    public int IdAmonestacion { get; set; }

    [MaxLength(100, ErrorMessage = "Como máximo 100 carácteres")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public DateTime FechaHoraHecho { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public DateTime FechaHoraRegistro { get; set; }

    [MaxLength(45, ErrorMessage = "Como máximo 45 carácteres")]
    public string? Sancion { get; set; } = null!;

    [Required(ErrorMessage = "Campo requerido")]
    [MaxLength(45, ErrorMessage = "Como máximo 45 carácteres")]
    public string Tipo { get; set; } = null!;

    public int IdMotivo { get; set; }

    public int IdAlumno { get; set; }

    public int IdProfesorRegistra { get; set; }

    public int IdProfesorHecho { get; set; }

    [Required(ErrorMessage = "Campo requerido")]
    public virtual Alumno IdAlumnoNavigation { get; set; } = null!;

    [Required(ErrorMessage = "Campo requerido")]
    public virtual Motivo IdMotivoNavigation { get; set; } = null!;

    [Required(ErrorMessage = "Campo requerido")]
    public virtual Profesor IdProfesorHechoNavigation { get; set; } = null!;

    [Required(ErrorMessage = "Campo requerido")]
    public virtual Profesor IdProfesorRegistraNavigation { get; set; } = null!;
}
