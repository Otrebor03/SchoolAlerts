using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using System.Windows.Data;

namespace ProyectoRRC.Frontend.MVVM
{
    /// <summary>
    /// Clase modelo vista del objeto persona
    /// </summary>
    internal class MVPersona : MVBaseCRUD<Persona>
    {
        /// <summary>
        /// Variable donde se almacena el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto donde almacenmaos la persona
        /// </summary>
        private Persona _persona;

        /// <summary>
        /// Objeto donde almacenmaos el rol
        /// </summary>
        private Role _rol;

        /// <summary>
        /// Objeto donde almacenmaos el grupo
        /// </summary>
        private Grupo _grupo;

        /// <summary>
        /// Objeto donde almacenmaos el nombre de la persona
        /// </summary>
        private string _nombrePersona;

        /// <summary>
        /// Objeto donde almacenmaos el dni de la persona
        /// </summary>
        private string _dniPersona;

        /// <summary>
        /// Objeto servicio persona
        /// </summary>
        private PersonaServicio personaServ;

        /// <summary>
        /// Objeto servicio rol
        /// </summary>
        private RolServicio rolServ;

        /// <summary>
        /// Objeto servicio grupo
        /// </summary>
        private GrupoServicio grupoServ;

        /// <summary>
        /// Lista auxiliar donde almacenamos las personas
        /// </summary>
        private ListCollectionView listaAux;

        /// <summary>
        /// Lista donde almacenamos los criterios para filtrar personas
        /// </summary>
        private List<Predicate<Persona>> criterios;

        /// <summary>
        /// Lista de criterios por nombre
        /// </summary>
        private Predicate<Persona> criterioNombre;

        /// <summary>
        /// Lista de criterios por dni
        /// </summary>
        private Predicate<Persona> criterioDni;

        /// <summary>
        /// Lista de criterios por rol
        /// </summary>
        private Predicate<Persona> criterioRol;

        /// <summary>
        /// Varibale para comprobar si se va a realizar un informe
        /// </summary>
        private bool informe;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contexto">El contexto de la base de datos.</param>
        public MVPersona(IncidenciaspartesrrcContext cont)
        {
            this.contexto = cont;
            incializa(false);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contexto">El contexto de la base de datos.</param>
        public MVPersona(IncidenciaspartesrrcContext cont, bool info)
        {
            this.contexto = cont;
            this.informe = info;
            incializa(informe);
        }

        /// <summary>
        /// Metodo que inicializa
        /// </summary>
        private void incializa(bool informe)
        {
            _persona = new Persona();
            personaServ = new PersonaServicio(contexto);
            rolServ = new RolServicio(contexto);
            grupoServ = new GrupoServicio(contexto);
            servicio = personaServ;
            criterios = new List<Predicate<Persona>>();


            if (informe)
            {
                List<Persona> lista = new List<Persona>();

                personaServ.GetAll.ForEach(persona =>
                {
                    if(persona.IdRolNavigation.NombreRol.Equals("Tutor") | persona.IdRolNavigation.NombreRol.Equals("Profesor"))
                    {
                        lista.Add(persona);
                    }
                });

                listaAux = new ListCollectionView(lista);
            }
            else
            {
                listaAux = new ListCollectionView(personaServ.GetAll);
            }

            inicializaCriterios();
        }

        /// <summary>
        /// Metodo que inicializa los criterios
        /// </summary>
        public void inicializaCriterios()
        {
            criterioNombre = new Predicate<Persona>(m => m.Nombre != null && m.Nombre.ToLower().Contains(nombrePersona.ToLower()));
            criterioDni = new Predicate<Persona>(m => m.Dni != null && m.Dni.ToLower().Contains(dniPersona.ToLower()));
            criterioRol = new Predicate<Persona>(m => m.IdRolNavigation.NombreRol != null && m.IdRolNavigation.NombreRol.Equals(rolSeleccionado.NombreRol));
        }


        /// <summary>
        /// Lista con las personas
        /// </summary>
        public ListCollectionView listaPersonas { get { return listaAux; } }

        /// <summary>
        /// Metodo donde almacenamos a la persona
        /// </summary>
        public Persona persona
        {
            get { return _persona; }
            set { _persona = value; NotifyPropertyChanged(nameof(persona)); }
        }

        /// <summary>
        /// Metodo que almacena el nombre de la persona introducido por el usuario
        /// </summary>
        public string nombrePersona
        {
            get { return _nombrePersona; }
            set { _nombrePersona = value; NotifyPropertyChanged(nameof(nombrePersona)); }
        }

        /// <summary>
        /// Metodo que almacena el dni de la persona introducido por el usuario
        /// </summary>
        public string dniPersona
        {
            get { return _dniPersona; }
            set { _dniPersona = value; NotifyPropertyChanged(nameof(dniPersona)); }
        }

        /// <summary>
        /// Metodo que almacena el rol de la persona introducido por el usuario
        /// </summary>
        public Role rolSeleccionado
        {
            get { return _rol; }
            set { _rol = value; NotifyPropertyChanged(nameof(rolSeleccionado)); }
        }

        /// <summary>
        /// Metodo que almacena el grupo de la persona introducido por el usuario
        /// </summary>
        public Grupo grupoSeleccionado
        {
            get { return _grupo; }
            set { _grupo = value; NotifyPropertyChanged(nameof(grupoSeleccionado)); }
        }

        /// <summary>
        /// Lista de roles
        /// </summary>
        public List<Role> listaRoles { get { return rolServ.GetAll; } }

        /// <summary>
        /// Lista de grupos
        /// </summary>
        public List<Grupo> listaGrupos { get { return grupoServ.GetAll; } }

        /// <summary>
        /// Metodo que elimina el filtrado de la lista auxiliar
        /// </summary>
        public void borrarFiltro()
        {
            listaAux.Filter = null;
        }

        /// <summary>
        /// Recibe un objeto de la lista de personas
        /// La chequea con todos los filtros incluidos en la lista criterios
        /// </summary>
        /// <param name="item">Objeto persona</param>
        /// <returns>Verdadero o falso en funcion si cumple con los criterios o no</returns>
        public bool FiltroCombinadoCriterios(object item)
        {
            bool correcto = true;
            Persona persona = (Persona)item;
            if (criterios.Count != 0)
            {
                correcto = criterios.TrueForAll(x => x(persona));
            }

            return correcto;
        }

        /// <summary>
        /// Metodo que se encarga de añadir los filtros que hemos creado
        /// </summary>
        public void AddFiltros()
        {
            criterios.Clear();
            if (!string.IsNullOrEmpty(nombrePersona)) criterios.Add(criterioNombre);
            if (!string.IsNullOrEmpty(dniPersona)) criterios.Add(criterioDni);
            if (rolSeleccionado != null) criterios.Add(criterioRol);
        }

        /// <summary>
        /// Metodo que se encarga de añadir los filtros y combinarlos
        /// </summary>
        public void Filtrar()
        {
            AddFiltros();
            listaPersonas.Filter = new Predicate<object>(FiltroCombinadoCriterios);
        }


       
    }
}

