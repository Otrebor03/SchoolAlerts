using ProyectoRRC.Backend.Modelo;

namespace ProyectoRRC.Backend.Servicios
{
    /// <summary>
    /// Clase que contiene las reglas de negocio de la clase Grupo
    /// </summary>
    internal class GrupoServicio : ServicioGenerico<Grupo>
    {

        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        private AlumnosServicio alumnoServ;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Contexto de la base de datos</param>
        public GrupoServicio(IncidenciaspartesrrcContext context) : base(context)
        {
            contexto = context;
            alumnoServ = new AlumnosServicio(context);
        }

        /// <summary>
        /// Metodo que recorre todas las tablas de un grupo y almacena en una lista los nombres
        /// </summary>
        /// <returns>Devuelve una lista con todos los nombres de los grupos</returns>
        public List<string> nombreGrupos()
        {
            List<string> list = new List<string>();

            GetAll.ForEach(x =>
            {
                list.Add(x.NombreGrupo);
            });

            return list;
        }

        /// <summary>
        /// Metodo que permite obtener las amonestaciones o incidencias en un año concreto de un grupo concreto
        /// </summary>
        /// <param name="grupo">Grupo seleccionado</param>
        /// <param name="tipo">Amonestacion / Incidencia</param>
        /// <param name="anyo">Año seleccionado</param>
        /// <returns>Lista con la cantidad de amonestaciones o incidencias ordenadas por mes</returns>
        public List<double> obtenerAmonestacionesPorMes(Grupo grupo, string tipo, int anyo)
        {
            Dictionary<int, double> cantidadPorMes = new Dictionary<int, double>();

            List<double> list = new List<double>(new double[12]);

            foreach (Alumno alumno in grupo.Alumnos)
            {
                

                for(int i = 0; i <=11; i++)
                {
                    list[i] += alumnoServ.obtenerAmonestacionesPorMes(alumno, tipo, anyo)[i];
                }
            }

            return list;
        }
    }
}
