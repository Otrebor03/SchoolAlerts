using ProyectoRRC.Frontend.MVVM;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRRC.Backend.Modelo;

public partial class Persona:PropertyChangedDataError
{
    public int IdPersona { get; set; }

    public int IdRol { get; set; }

    [Required(ErrorMessage ="Campo Requerido")]
    [RegularExpression(@"^\d{8}[A-Z]$|^[A-Z]\d{7}[A-Z]$", ErrorMessage = "El campo debe contener un DNI (8 numéricos y el último una letra mayúscula) o un NIE (una letra seguida de 7 dígitos y otra letra)")]
    public string Dni { get; set; } = null!;

    [Required(ErrorMessage = "Campo Requerido"), MaxLength(45, ErrorMessage = "Como máximo 45 carácteres")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚ\s]*$", ErrorMessage = "Este campo solo puede contener letras, espacios y letras con acentos.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Campo Requerido")]
    [MaxLength(45, ErrorMessage = "Como máximo 45 carácteres")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚ\s]*$", ErrorMessage = "Este campo solo puede contener letras, espacios y letras con acentos.")]
    public string Apellido1 { get; set; } = null!;

    [Required(ErrorMessage = "Campo Requerido")]
    [MaxLength(45, ErrorMessage = "Como máximo 45 carácteres")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚ\s]*$", ErrorMessage = "Este campo solo puede contener letras, espacios y letras con acentos.")]
    public string? Apellido2 { get; set; }

    [Required(ErrorMessage = "Campo Requerido")]
    [MaxLength(15, ErrorMessage = "Como máximo 15 carácteres")]
    public string Contrasenya { get; set; } = null!;

    public byte[]? Foto { get; set; }

    [Required(ErrorMessage = "Campo Requerido")]
    public virtual Role IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Padre> Padres { get; set; } = new List<Padre>();

    public virtual ICollection<Profesor> Profesors { get; set; } = new List<Profesor>();


}
