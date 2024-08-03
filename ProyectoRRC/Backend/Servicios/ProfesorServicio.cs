using ProyectoRRC.Backend.Modelo;

namespace ProyectoRRC.Backend.Servicios
{

    /// <summary>
    /// Clase que contiene las reglas de negocio de la clase Profesor
    /// </summary>
    class ProfesorServicio : ServicioGenerico<Profesor>
    {
        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">ContextoContexto de la base de datos</param>
        public ProfesorServicio(IncidenciaspartesrrcContext context) : base(context)
        {
            contexto = context;
        }


         
    }
}
