using ProyectoRRC.Backend.Modelo;

namespace ProyectoRRC.Backend.Servicios
{
    /// <summary>
    /// Clase que contiene las reglas de negocio de la clase Persona
    /// </summary>
    public class PersonaServicio:ServicioGenerico<Persona>
    {
        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Metodo donde almacenamos el usuario que quiere iniciar sesion
        /// </summary>
        public Persona usuLogin { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">ContextoContexto de la base de datos</param>
        public PersonaServicio(IncidenciaspartesrrcContext context): base(context)
        {
            contexto = context;
        }
    
        /// <summary>
        /// Metodo que comprueba si el usuario y la contraseña estan en la base de datos
        /// </summary>
        /// <param name="user">usuario</param>
        /// <param name="password">contraseña</param>
        /// <returns>True / False</returns>
        public Boolean login(String user, String password)
        {
            Boolean correcto = true;

            try
            {
                usuLogin = contexto.Set<Persona>().Where(u => u.Dni == user).FirstOrDefault();
            }catch (Exception ex)
            {
                System.Console.WriteLine(ex.StackTrace);
            }

            if(usuLogin == null)
            {
                correcto = false;
            }
            else if(!usuLogin.Dni.Equals(user) || !usuLogin.Contrasenya.Equals(password))
            {
                correcto = false;
            }

            return correcto;
        }

        /// <summary>
        /// Metodo que recorre todas las tablas de una persona y almacena en una lista los DNI
        /// </summary>
        /// <returns>Devuelve una lista con todos los DNI de las personas</returns>
        public List<string> obtenerDni()
        {
            List<string> list = new List<string>();

            GetAll.ForEach(x =>
            {
                list.Add(x.Dni);
            });

            return list;
        }
    
    }
}
