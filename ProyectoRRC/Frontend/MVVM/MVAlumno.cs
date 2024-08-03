using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using System.Windows.Data;
using LiveCharts;
using LiveCharts.Wpf;

namespace ProyectoRRC.Frontend.MVVM
{
    /// <summary>
    /// Clase modelo vista del objeto Alumno
    /// </summary>
    internal class MVAlumno:MVBaseCRUD<Alumno>
    {
        /// <summary>
        /// Objeto con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto donde almacenamos al usuario
        /// </summary>
        private Persona persona;

        /// <summary>
        /// Objeto para manejar datos de alumno
        /// </summary>
        private Alumno _alumno;

        /// <summary>
        /// Objeto para manejar datos de grupo
        /// </summary>
        private Grupo _grupo;

        /// <summary>
        /// Objeto para manejar datos de padre
        /// </summary>
        private Padre _padre;
        
        /// <summary>
        /// Objeto para manejar datos de nia
        /// </summary>
        private string _nia;
        
        /// <summary>
        /// Objeto para manejar datos de nombre
        /// </summary>
        private string _nombre;
        
        /// <summary>
        /// Objeto para manejar datos de telefono
        /// </summary>
        private string _telefono;

        /// <summary>
        /// Objeto donde almacenamos el año seleccionado
        /// </summary>
        private int _anyo;

        /// <summary>
        /// Objeto donde seleccionamos al alumno para mostrar en el grafico
        /// </summary>
        private Alumno alumnoGrafico;

        /// <summary>
        /// Servicio de alumno
        /// </summary>
        private AlumnosServicio alumnoServ;
        
        /// <summary>
        /// Servicio de grupo
        /// </summary>
        private GrupoServicio grupoServ;

        /// <summary>
        /// Servicio de padre
        /// </summary>
        private PadreServicio padreServ;


        /// <summary>
        /// Lista auxiliar donde almacenaremos todos los alumnos
        /// </summary>
        private ListCollectionView listaAux;


        /// <summary>
        /// Lista donde almacenaremos los criterios sobre el alumno
        /// </summary>
        private List<Predicate<Alumno>> criterios;

        /// <summary>
        /// Lista donde almacenaremos los criterios de grupo
        /// </summary>
        private Predicate<Alumno> criterioGrupo;

        /// <summary>
        /// Lista donde almacenaremos los criterios de nia
        /// </summary>
        private Predicate<Alumno> criterioNia;

        /// <summary>
        /// Lista donde almacenaremos los criterios de nombre
        /// </summary>
        private Predicate<Alumno> criterioNombre;

        /// <summary>
        /// Lista donde almacenaremos los criterios de telefono
        /// </summary>
        private Predicate<Alumno> criterioTelefono;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        public MVAlumno(IncidenciaspartesrrcContext con) 
        {
            this.contexto = con;
            inicializa();
        }  

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        /// <param name="per"></param>
        public MVAlumno(IncidenciaspartesrrcContext con, Persona per)
        {
            this.contexto = con;
            this.persona = per;

            inicializa();
        }

        /// <summary>
        /// COnstructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        /// <param name="al">Alumno seleccionado</param>
        /// <param name="anyo">Año seleccionado</param>
        public MVAlumno(IncidenciaspartesrrcContext con, Alumno al, int anyo)
        {
            this.contexto = con;
            alumnoGrafico = al;
            this._anyo = anyo;
            inicializa();
        }

        /// <summary>
        /// Cosntructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        /// <param name="per">Persona que ha iniciado sesion</param>
        /// <param name="al">Alumno seleccionado</param>
        /// <param name="anyo">Año selecionado</param>
        public MVAlumno(IncidenciaspartesrrcContext con, Persona per, Alumno al, int anyo)
        {
            this.contexto = con;
            alumnoGrafico = al;
            this.persona = per;
            this._anyo = anyo;
            inicializa();
        }


        /// <summary>
        /// Metodo donde inicializamos los objetos, servicios, listas, criterios y tambien en la lista auxiliar en el caso de iniciar sesion un padre solo le añadiremos los estudiantes que sean sus hijos
        /// </summary>
        private void inicializa()
        {
            _alumno = new Alumno();
            alumnoServ = new AlumnosServicio(contexto);
            grupoServ = new GrupoServicio(contexto);
            padreServ = new PadreServicio(contexto);
            servicio = alumnoServ;
            criterios = new List<Predicate<Alumno>>();


            if(persona != null && persona.IdRolNavigation.NombreRol.Equals("Padre")) 
            {
                List<Alumno> listaAlumnos = new List<Alumno>();

                alumnoServ.GetAll.ForEach(alumno =>
                {
                    if (persona.Padres.Contains(alumno.IdPadreNavigation))
                    {
                        listaAlumnos.Add(alumno);
                    }
                });

                listaAux = new ListCollectionView(listaAlumnos);
            }else if (persona != null && persona.IdRolNavigation.NombreRol.Equals("Tutor"))
            {
                List<Alumno> listaAlumnos = new List<Alumno>();

                alumnoServ.GetAll.ForEach(alumno =>
                {
                    if (persona.Profesors.First().IdGrupos.Contains(alumno.IdGrupoNavigation))
                    {
                        listaAlumnos.Add(alumno);
                    }
                });

                listaAux = new ListCollectionView(listaAlumnos);

            }
            else
            {
                listaAux = new ListCollectionView(alumnoServ.GetAll);
            }




            if (alumnoGrafico != null)
            {
                inicializaGrafico(alumnoGrafico, anyo);
            }


            inicializaCriterios();
        }


        /// <summary>
        /// Metodo donde inicilizamos todos los criterios
        /// </summary>
        private void inicializaCriterios()
        {
            criterioGrupo = new Predicate<Alumno>(g => g.IdGrupo != null && g.IdGrupo == grupoSeleccionado.IdGrupo);
            criterioNombre = new Predicate<Alumno>(a => a.Nombre != null && a.Nombre.ToLower().Contains(nombreAlumno.ToLower()));
            criterioNia = new Predicate<Alumno>(a => a.Nia != null && a.Nia.ToLower().Contains(niaAlumno.ToLower()));
            criterioTelefono = new Predicate<Alumno>(a => a.Telefono != null && a.Telefono.ToLower().Contains(telefonoAlumno.ToLower()));
        }

        /// <summary>
        /// Metodo donde inicilizamos todos los graficos
        /// </summary>
        private void inicializaGrafico(Alumno alumno, int anyo)
        {

            // Inicializamos la colección de series para el gráfico de columnas
            SeriesCollection = new SeriesCollection
            {
                // Agregamos una serie para las amonestaciones
                new ColumnSeries
                {
                       
                    Title = "Amonestaciones",
                    Values = new ChartValues<double> (alumnoServ.obtenerAmonestacionesPorMes(alumno, "Amonestación", anyo))
                }
            };

            // Agregamos otra serie para las incidencias
            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Incidencias",
                Values = new ChartValues<double>(alumnoServ.obtenerAmonestacionesPorMes(alumno, "Incidencia", anyo))
            });

            // Inicializamos los nombres de las etiquetas para los meses
            Labels = new[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

            // Definimos una función para dar formato a los valores del gráfico
            Formatter = value => value.ToString("N");
        }

        /// <summary>
        /// Metodo que almacena el grupo seleccionado por el usuario
        /// </summary>
        public Grupo grupoSeleccionado
        {
            get { return _grupo; }
            set { _grupo = value; NotifyPropertyChanged(nameof(grupoSeleccionado)); }
        }

        /// <summary>
        /// Metodo que almacena el nia del alumno escrito por el usuario
        /// </summary>
        public string niaAlumno
        {
            get { return _nia; }
            set { _nia = value; NotifyPropertyChanged(nameof(niaAlumno)); }
        }

        /// <summary>
        /// Metodo que almacena el nombre del alumno escrito por el usuario
        /// </summary>
        public string nombreAlumno
        {
            get { return _nombre; }
            set { _nombre = value; NotifyPropertyChanged(nameof(nombreAlumno)); }
        }

        /// <summary>
        /// Metodo que alamcena el año seleccionado por el usuario
        /// </summary>
        public int anyo
        {
            get { return _anyo; }
            set { _anyo = value; NotifyPropertyChanged(nameof(anyo));}
        }

        /// <summary>
        /// Metodo que almacena el telefono del alumno escrito por el usuario
        /// </summary>
        public string telefonoAlumno
        {
            get { return _telefono; }
            set { _telefono = value; NotifyPropertyChanged(nameof(telefonoAlumno)); }
        }

        /// <summary>
        /// Metodo que almacena el padre seleccionado por el usuario
        /// </summary>
        public Padre padreSeleccionado
        {
            get { return _padre; }
            set { _padre = value; NotifyPropertyChanged(nameof(padreSeleccionado)); }
        }

        /// <summary>
        /// Metodo que alamcena el alumno 
        /// </summary>
        public Alumno alumno
        {
            get { return _alumno; }
            set { _alumno = value; NotifyPropertyChanged(nameof(alumno)); }
        }

        /// <summary>
        /// Lista de alumnos
        /// </summary>
        public ListCollectionView listaAlumnos { get { return listaAux; } }


        /// <summary>
        /// Lista de grupos
        /// </summary>
        public List<Grupo> listaGrupos { get { return grupoServ.GetAll; } }

        /// <summary>
        /// Lista de padres
        /// </summary>
        public List<Padre> listaPadres { get { return padreServ.GetAll; } }


        /// <summary>
        /// Metodo que se encarga de añadir los filtros que hemos creado
        /// </summary>
        public void AddFiltros()
        {
            criterios.Clear();
            if (grupoSeleccionado != null) criterios.Add(criterioGrupo);
            if (!string.IsNullOrEmpty(niaAlumno)) criterios.Add(criterioNia);
            if (!string.IsNullOrEmpty(nombreAlumno)) criterios.Add(criterioNombre);
            if (!string.IsNullOrEmpty(telefonoAlumno)) criterios.Add(criterioTelefono);
            

        }

        /// <summary>
        /// Metodo que se encarga de añadir los filtros y combinarlos
        /// </summary>
        public void Filtrar()
        {
            AddFiltros();
            listaAlumnos.Filter = new Predicate<object>(FiltroCombinadoCriterios);
        }

        /// <summary>
        /// Recibe un objeto de la lista de alumnos
        /// La chequea con todos los filtros incluidos en la lista criterios
        /// </summary>
        /// <param name="item">Objeto alumno</param>
        /// <returns>Verdadero o falso en funcion si cumple con los criterios o no</returns>
        public bool FiltroCombinadoCriterios(object item)
        {
            bool correcto = true;
            Alumno alumno = (Alumno)item;
            if (criterios.Count != 0)
            {
                correcto = criterios.TrueForAll(x => x(alumno));
            }

            return correcto;
        }

        /// <summary>
        /// Metodo que borra el filtro en la lista
        /// </summary>
        public void borrarFiltro()
        {
            listaAux.Filter = null;
        }

        /// <summary>
        /// Propiedad para almacenar la colección de series del gráfic
        /// </summary>
        public SeriesCollection SeriesCollection { get; set; }

        /// <summary>
        /// Propiedad para almacenar los nombres de las etiquetas de los meses
        /// </summary>
        public string[] Labels { get; set; }

        /// <summary>
        /// Propiedad para almacenar una función que dará formato a los valores del gráfico
        /// </summary>
        public Func<double, string> Formatter { get; set; }
    }
}
