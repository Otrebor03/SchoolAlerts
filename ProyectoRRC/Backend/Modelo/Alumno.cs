using ProyectoRRC.Frontend.MVVM;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRRC.Backend.Modelo;


public partial class Alumno: PropertyChangedDataError
{
    public int IdAlumno { get; set; }

    [Required(ErrorMessage = "Campo Requerido")]
    [MaxLength(9, ErrorMessage = "Como máximo 9 carácteres")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El campo debe contener 8 dígitos numéricos")]
    public string Nia { get; set; } = null!;

    [Required(ErrorMessage = "Campo Requerido")]
    [MaxLength(45, ErrorMessage = "Como máximo 45 carácteres")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚ\s]*$", ErrorMessage = "Este campo solo puede contener letras, espacios y letras con tilde.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Campo Requerido")]
    [MaxLength(45, ErrorMessage = "Como máximo 45 carácteres")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚ\s]*$", ErrorMessage = "Este campo solo puede contener letras, espacios y letras con tilde.")]
    public string Apellido1 { get; set; } = null!;

    [MaxLength(45, ErrorMessage = "Como máximo 45 carácteres")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚ\s]*$", ErrorMessage = "Este campo solo puede contener letras, espacios y letras con tilde.")]
    public string? Apellido2 { get; set; }

    [Required(ErrorMessage = "Campo Requerido")]
    [MaxLength(15, ErrorMessage = "El campo debe contener 15 dígitos numéricos")]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "Este campo solo puede contener números.")]
    public string? Telefono { get; set; }

    
    public int IdGrupo { get; set; }

    public byte[]? Foto { get; set; }

    public int IdPadre { get; set; }

    public virtual ICollection<Amonestacione> Amonestaciones { get; set; } = new List<Amonestacione>();

    [Required(ErrorMessage = "Campo Requerido")]
    public virtual Grupo IdGrupoNavigation { get; set; } = null!;

    [Required(ErrorMessage = "Campo Requerido")]
    public virtual Padre IdPadreNavigation { get; set; } = null!;

    override
        public string ToString()
    {
        return Nombre + " " + Apellido1+ " "+Apellido2+" /NIA: "+Nia;
    }
}
