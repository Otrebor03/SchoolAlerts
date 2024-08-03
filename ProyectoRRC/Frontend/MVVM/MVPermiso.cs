using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using System.Windows.Data;

namespace ProyectoRRC.Frontend.MVVM
{
    /// <summary>
    /// Clase modelo vista del objeto Permiso
    /// </summary>
    internal class MVPermiso : MVBaseCRUD<Permiso>
    {
        /// <summary>
        /// Objeto con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto donde almacenamos el permiso
        /// </summary>
        private Permiso _permiso;

        /// <summary>
        /// Objeto donde almacenamos el rol
        /// </summary>
        private Role _role;

        /// <summary>
        /// Lista auxiliar donde almacenamos los permisos
        /// </summary>
        private ListCollectionView listaAux;

        /// <summary>
        /// Objeto con el servicio de permiso
        /// </summary>
        private PermisoServicio permisoServ;

        /// <summary>
        /// Objeto donde almacenamos el servicio de rol
        /// </summary>
        private RolServicio rolServ;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Contexto de la base de datos</param>
        public MVPermiso(IncidenciaspartesrrcContext context)
        {
            this.contexto = context;
            inicializa();

        }

        /// <summary>
        /// Metodo donde incializamos el objeto, servicio y lista auxiliar
        /// </summary>
        public void inicializa()
        {
            _permiso = new Permiso();
            permisoServ = new PermisoServicio(contexto);
            rolServ = new RolServicio(contexto);
            servicio = permisoServ;
            listaAux = new ListCollectionView(permisoServ.GetAll);
        }

        /// <summary>
        /// Metodo que permite obtener la informaciond de un permiso y modificarla
        /// </summary>
        public Permiso permiso
        {
            get { return _permiso; }
            set { _permiso = value; NotifyPropertyChanged(nameof(permiso)); } 
        }

        /// <summary>
        /// Metodo donde almacenamos el permiso seleccionado pr el usuario
        /// </summary>
        public Role rolSeleccionado
        {
            get { return _role; }
            set { _role = value; NotifyPropertyChanged(nameof(rolSeleccionado)); }
        }

        /// <summary>
        /// Lista de permisos
        /// </summary>
        public ListCollectionView listaPermisos { get { return listaAux; } }

        /// <summary>
        /// Lista de roles
        /// </summary>
        public List<Role> listaRoles { get { return rolServ.GetAll; } }


    }
}
