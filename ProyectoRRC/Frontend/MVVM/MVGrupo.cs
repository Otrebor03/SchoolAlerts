using LiveCharts;
using LiveCharts.Wpf;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using System.Windows.Data;

namespace ProyectoRRC.Frontend.MVVM
{
    /// <summary>
    /// Clase modelo vista del objeto grupo
    /// </summary>
    class MVGrupo :MVBaseCRUD<Grupo>
    {
        /// <summary>
        /// Objeto con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto donde almacenammos el grupo
        /// </summary>
        private Grupo _grupo;

        /// <summary>
        /// Objeto donde almacenamos el grupo seleccionado para el grafico
        /// </summary>
        private Grupo _grupoGrafico;

        /// <summary>
        /// Objeto donde almacenamos el año seleccionado
        /// </summary>
        private int _anyo;

        /// <summary>
        /// Objeto donde almacenamos el servicio de grupo
        /// </summary>
        private GrupoServicio grupoServ;

        /// <summary>
        /// Objeto donde almacenamos a la persona que ha iniciado sesion
        /// </summary>
        private Persona persona;

        /// <summary>
        /// Lista auxiliar para almacenar grupos
        /// </summary>
        private ListCollectionView listaAux;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contexto">Contexto de la base de datos</param>
        public MVGrupo(IncidenciaspartesrrcContext contexto)
        {
            this.contexto = contexto;
            inicializa();

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contexto">Contexto de la base de datos</param>
        /// <param name="per">Persona que ha iniciado sesion</param>
        public MVGrupo(IncidenciaspartesrrcContext contexto, Persona per)
        {
            this.contexto = contexto;
            this.persona = per;
            inicializa();

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        /// <param name="grupo">Grupo seleccionado</param>
        /// <param name="anyo">Año seleccionado</param>
        public MVGrupo(IncidenciaspartesrrcContext con, Grupo grupo, int anyo)
        {
            this.contexto =con;
            this._grupoGrafico = grupo;
            this._anyo = anyo;


            inicializa();
        }

        /// <summary>
        /// Metodo donde inicializamos el objeto y los servicios
        /// </summary>
        private void inicializa()
        {
            _grupo = new Grupo();
            grupoServ = new GrupoServicio(contexto);
            servicio = grupoServ;

            

            if(_grupoGrafico != null)
            {
                inicializaGraficos(_grupoGrafico, _anyo);
            }

            if(persona != null && persona.IdRolNavigation.NombreRol.Equals("Tutor"))
            {
                List<Grupo> listaGrupos = new List<Grupo>();
                grupoServ.GetAll.ForEach(grupo =>
                {
                    
                    if (grupo.IdProfesors.Contains(persona.Profesors.First()))
                    {
                        listaGrupos.Add(grupo);
                    }
                });

                listaAux = new ListCollectionView(listaGrupos);
            }
            else
            {
                listaAux = new ListCollectionView(grupoServ.GetAll);
            }
            
        }

        private void inicializaGraficos(Grupo grupo, int anyo)
        {
            // Inicializamos la colección de series para el gráfico de columnas
            SeriesCollection = new SeriesCollection
            {
                // Agregamos una serie para las amonestaciones
                new ColumnSeries
                {

                    Title = "Amonestaciones",
                    Values = new ChartValues<double> (grupoServ.obtenerAmonestacionesPorMes(grupo, "Amonestación", anyo))
                }
            };

            // Agregamos otra serie para las incidencias
            SeriesCollection.Add(new ColumnSeries
            {
                Title = "Incidencias",
                Values = new ChartValues<double>(grupoServ.obtenerAmonestacionesPorMes(grupo, "Incidencia", anyo))
            });

            // Inicializamos los nombres de las etiquetas para los meses
            Labels = new[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

            // Definimos una función para dar formato a los valores del gráfico
            Formatter = value => value.ToString("N");
        }

        /// <summary>
        /// Metodo donde almacenamos el grupo
        /// </summary>
        public Grupo grupo
        {
            get { return _grupo; }
            set { _grupo = value; NotifyPropertyChanged(nameof(grupo)); }

            
        }

        /// <summary>
        /// Metodo que alamcena el año seleccionado por el usuario
        /// </summary>
        public int anyo
        {
            get { return _anyo; }
            set { _anyo = value; NotifyPropertyChanged(nameof(anyo)); }
        }

        /// <summary>
        /// Lista de grupos
        /// </summary>
        public ListCollectionView listaGrupos { get { return listaAux; } }

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
