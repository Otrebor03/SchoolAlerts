using ProyectoRRC.Backend.Modelo;

namespace ProyectoRRC.Backend.Servicios
{
    /// <summary>
    /// Clase que contiene las reglas de negocio de la clase Alumno
    /// </summary>
    internal class AlumnosServicio : ServicioGenerico<Alumno>
    {
        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Contexto de la base de datos</param>
        public AlumnosServicio(IncidenciaspartesrrcContext context) : base(context)
        {
            contexto = context;
        }

        /// <summary>
        /// Metodo que recorre todas las tablas de un alumno y almacena en una lista los NIA
        /// </summary>
        /// <returns>Devuelve una lista con todos los NIA de los alumnos</returns>
        public List<string> obtenerNia()
        {
            List<string> list = new List<string>();

            GetAll.ForEach(x =>
            {
                list.Add(x.Nia);
            });

            return list;
        }

        /// <summary>
        /// Metodo que permite obtener las amonestaciones o incidencias en un año concreto de un alumno concreto
        /// </summary>
        /// <param name="alumno">Alumno seleccionado</param>
        /// <param name="tipo">Amonestacion / Incidencia</param>
        /// <param name="anyo">Año seleccionado</param>
        /// <returns>Lista con la cantidad de amonestaciones o incidencias ordenadas por mes</returns>
        public List<double> obtenerAmonestacionesPorMes(Alumno alumno, string tipo, int anyo)
        {
            
            Dictionary<int, double> cantidadPorMes = new Dictionary<int, double>();

            // Inicializar el diccionario con todos los meses del año y asignarles un valor de 0
            for (int i = 1; i <= 12; i++)
            {
                cantidadPorMes[i] = 0.0;
            }

            // Iterar sobre las amonestaciones del alumno
            foreach (Amonestacione amonestacion in alumno.Amonestaciones)
            {
                

                // Incrementar el valor correspondiente al mes encontrado

                if(tipo.Equals(amonestacion.Tipo) && anyo == amonestacion.FechaHoraRegistro.Year)
                {
                    cantidadPorMes[amonestacion.FechaHoraRegistro.Month]++;
                }
                
            }

            

            return new List<double>(cantidadPorMes.Values);
            
        }

            
        
    }
}
