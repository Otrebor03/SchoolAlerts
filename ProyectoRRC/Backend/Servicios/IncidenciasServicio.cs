using ProyectoRRC.Backend.Modelo;

namespace ProyectoRRC.Backend.Servicios
{
    /// <summary>
    /// Clase que contiene las reglas de negocio de la clase Incidencias
    /// </summary>
    internal class IncidenciasServicio:ServicioGenerico<Amonestacione>
    {
        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Contexto de la base de datos</param>
        public IncidenciasServicio(IncidenciaspartesrrcContext context):base(context) 
        {
            contexto = context;
        }

        /// <summary>
        /// Metodo que devuelve la fecha y hora del hecho de la ultima amonestacion/incidencia registrada en la base de datos
        /// </summary>
        /// <returns>Fecha y hora del hecho de la ultima amonestacion/incidencia registrada</returns>
        public DateTime FechaHecho()
        {
            return contexto.Amonestaciones.OrderBy(f => f.FechaHoraHecho).Reverse().FirstOrDefault().FechaHoraHecho;
        }

        /// <summary>
        /// Metodo que devuelve la fecha y hora del registro de la ultima amonestacion/incidencia registrada en la base de datos
        /// </summary>
        /// <returns>Fecha y hora del registro de la ultima amonestacion/incidencia registrada</returns>
        public DateTime FechaRegistro()
        {
            return contexto.Amonestaciones.OrderBy(f => f.FechaHoraRegistro).Reverse().FirstOrDefault().FechaHoraRegistro;
        }

    }
}
