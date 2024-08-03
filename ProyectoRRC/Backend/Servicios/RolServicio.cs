using ProyectoRRC.Backend.Modelo;

namespace ProyectoRRC.Backend.Servicios
{
    /// <summary>
    /// Clase que contiene las reglas de negocio de la clase Motivo
    /// </summary>
    internal class RolServicio:ServicioGenerico<Role>
    {
        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">ContextoContexto de la base de datos</param>
        public RolServicio(IncidenciaspartesrrcContext context) : base(context) {
            contexto = context;
        }

        /// <summary>
        /// Metodo que recorre todas las tablas de los roles y almacena en una lista los nombres de dichos roles
        /// </summary>
        /// <returns>Devuelve una lista con todos los nombres de los roles</returns>
        public List<string> listaNombresRol()
        {
            List<string> list = new List<string>();

            GetAll.ForEach(x =>
            {
                list.Add(x.NombreRol);
            });

            return list;
        }
    }
}
