using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using System.Windows.Data;

namespace ProyectoRRC.Frontend.MVVM
{
    /// <summary>
    /// Clase modelo vista del objeto rol
    /// </summary>
    class MVRol :MVBaseCRUD<Role>
    {
        /// <summary>
        /// Objeto con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto donde almacenos el rol
        /// </summary>
        private Role _rol;

        /// <summary>
        /// Objeto donde almacenos el rol
        /// </summary>
        private Permiso _permiso;

        /// <summary>
        /// Objeto servicio del rol
        /// </summary>
        private RolServicio rolServ;

        /// <summary>
        /// Objeto servicio del permiso
        /// </summary>
        private PermisoServicio permisoServ;

        /// <summary>
        /// Lista auxiliar donde almacenamos los roles
        /// </summary>
        private ListCollectionView listaAux;


        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        public MVRol(IncidenciaspartesrrcContext con) {
            this.contexto = con;
            incializa();
        }

        /// <summary>
        /// Metodo donde inicializamos los objetos, servicios y listas
        /// </summary>
        private void incializa()
        {
            _rol = new Role();
            rolServ = new RolServicio(contexto);
            permisoServ = new PermisoServicio(contexto);
            servicio = rolServ;
            listaAux = new ListCollectionView(rolServ.GetAll);
        }


        /// <summary>
        /// Metodo donde almacenamos el rol
        /// </summary>
        public Role rol
        {
            get { return _rol; }
            set { _rol = value; NotifyPropertyChanged(nameof(rol)); }
        }

        /// <summary>
        /// Metodo donde almacenamos el permiso seleccionado pr el usuario
        /// </summary>
        public Permiso permisoSeleccionado
        {
            get { return _permiso; }
            set { _permiso = value; NotifyPropertyChanged(nameof(permisoSeleccionado)); }
        }

        /// <summary>
        /// Lista de roles
        /// </summary>
        public ListCollectionView listaRoles { get { return listaAux; } }

        /// <summary>
        /// Lista de permisos
        /// </summary>
        public List<Permiso> listaPermisos { get { return permisoServ.GetAll; } }

        /// <summary>
        /// Metodo que devuelve una lista con todos los permisos que tiene un rol
        /// </summary>
        /// <param name="rol">Rol</param>
        /// <returns>Lista de permisos</returns>
        public List<Permiso> sacarPermisos(Role rol)
        {
            return rol.IdPermisos.ToList();
        }
    }
}
