using ProyectoRRC.Frontend.MVVM;
using System.ComponentModel.DataAnnotations;

namespace ProyectoRRC.Backend.Modelo;

public partial class Grupo: PropertyChangedDataError
{
    public int IdGrupo { get; set; }

    [Required(ErrorMessage = "Campo Requerido")]
    [MaxLength(45, ErrorMessage = "Como máximo 45 carácteres")]
    public string NombreGrupo { get; set; } = null!;

    public virtual ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();

    public virtual ICollection<Profesor> IdProfesors { get; set; } = new List<Profesor>();
    override
        public string ToString()
    {
        return NombreGrupo;
    }
}
