using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using System.Windows.Data;

namespace ProyectoRRC.Frontend.MVVM
{
    /// <summary>
    /// Clase modelo vista del objeto incidencia
    /// </summary>
    internal class MVIncidencias:MVBaseCRUD<Amonestacione>
    {
        /// <summary>
        /// Objeto donde almacenamos el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext context;

        /// <summary>
        /// Objeto donde almacenamos a la persona
        /// </summary>
        private Persona persona;

        /// <summary>
        /// Objeto donde almacenamos 
        /// </summary>
        private Alumno alumno;

        /// <summary>
        /// Objeto donde almacenamos la incidencia/amonestacion
        /// </summary>
        private Amonestacione _incidencias;

        /// <summary>
        /// Objeto donde almacenamos el motivo
        /// </summary>
        private Motivo _motivo;

        /// <summary>
        /// Objeto donde almacenamos al alumno
        /// </summary>
        private Alumno _alumno;

        /// <summary>
        /// Objeto donde almacenamos el tipo
        /// </summary>
        private string _tipo;

        /// <summary>
        /// Objeto donde almacenamos el nia del alumno
        /// </summary>
        private string _niaAlumno;

        /// <summary>
        /// Objeto donde almacenamos la fecha de inicio (fecha del hecho)
        /// </summary>
        private DateTime? _fechaInicio;
        
        /// <summary>
        /// Objeto donde almacenamos la fecha de fin (fecha del hecho)
        /// </summary>
        private DateTime ?_fechaFin;
        
        /// <summary>
        /// Objeto donde almacenamos el servicio de incidencias
        /// </summary>
        private IncidenciasServicio inicServ;

        /// <summary>
        /// Objeto donde almacenamos el servicio de alumnos
        /// </summary>
        private AlumnosServicio alumnoServ;

        /// <summary>
        /// Objeto donde almacenamos el servicio de motivos
        /// </summary>
        private MotivoServicio motivoServ;

        /// <summary>
        /// Objeto donde almacenamos el servicio de profesor
        /// </summary>
        private ProfesorServicio profesorServ;

        /// <summary>
        /// Objeto donde almacenamos el servicio de persona
        /// </summary>
        private PersonaServicio personaServ;

        /// <summary>
        /// Lista de incidencias
        /// </summary>
        private ListCollectionView listaAux;

        /// <summary>
        /// Lista donde almacenaremos los criterios sobre el alumno
        /// </summary>
        private List<Predicate<Amonestacione>> criterios;

        /// <summary>
        /// Lista de criterios de fechas inicio
        /// </summary>
        private Predicate<Amonestacione> criteriosFechaInicio;

        /// <summary>
        /// Lista de criterios de fechas fin
        /// </summary>
        private Predicate<Amonestacione> criteriosFechaFin;

        /// <summary>
        /// Lista de criterios de nia
        /// </summary>
        private Predicate<Amonestacione> criterioNia;

        /// <summary>
        /// Lista de criterios de tipo
        /// </summary>
        private Predicate<Amonestacione> criteriosTipo;

        /// <summary>
        /// Lista de criterios de motivo
        /// </summary>
        private Predicate<Amonestacione> criteriosMotivo;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Contexto de la base de datos</param>
        public MVIncidencias(IncidenciaspartesrrcContext context)
        {
            this.context = context;
            inicializa();
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contextod de la base de datos</param>
        /// <param name="usu">Persona</param>
        public MVIncidencias(IncidenciaspartesrrcContext con, Persona usu)
        {
            this.context = con;
            this.persona = usu;

            inicializa();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        /// <param name="usu">Persona</param>
        /// <param name="al">Alumno</param>
        public MVIncidencias(IncidenciaspartesrrcContext con, Persona usu, Alumno al)
        {
            this.context = con;
            this.persona = usu;
            this.alumno = al;
            inicializa();
        }

        /// <summary>
        /// Metodo donde inicializamos los objetos, servicios y comprueba el rol de la persona para que los profesores 
        /// y tutores solo puedan ver las incidencias de los alumnos de su grupo (en caso de tutores) y que puedan ver 
        /// las amonestaciones/incidencias de las cuales forman parte
        /// </summary>
        private void inicializa()
        {
            _incidencias = new Amonestacione();

            inicServ = new IncidenciasServicio(context);
            alumnoServ = new AlumnosServicio(context);
            motivoServ = new MotivoServicio(context);
            profesorServ = new ProfesorServicio(context);
            personaServ = new PersonaServicio(context);
            servicio = inicServ;
            _fechaInicio = null;
            _fechaFin = null;

            _incidencias.FechaHoraHecho = DateTime.Now;
            _incidencias.FechaHoraRegistro = DateTime.Now;

            if(persona != null && persona.IdRolNavigation.NombreRol.Equals("Padre"))
            {
                List<Amonestacione> listaAmo = new List<Amonestacione>();

                inicServ.GetAll.ForEach(amonestacion =>
                {
                    if(amonestacion.IdAlumnoNavigation.IdPadreNavigation.IdPersona == persona.IdPersona)
                    {
                        listaAmo.Add(amonestacion);
                    }
                });

                listaAux = new ListCollectionView(listaAmo);

            }
            else if(persona != null && persona.Profesors.Count > 0)//Comprobamos si hay una persona que sea profesor o tutor
            {
                _incidencias.IdProfesorHechoNavigation = persona.Profesors.First();
                _incidencias.IdProfesorRegistraNavigation = persona.Profesors.First();

                
                if (persona.Profesors.First().IdPersonaNavigation.IdRolNavigation.NombreRol.Equals("Profesor") | persona.Profesors.First().IdPersonaNavigation.IdRolNavigation.NombreRol.Equals("Tutor"))
                {
                    //Creamos una lista auxiliar donde almacenaremos las incidencias/amonestaciones las cuales contengan en el hecho o registro al profesor que ha inciado sesion
                    List<Amonestacione> listaAmo = new List<Amonestacione>();
                    inicServ.GetAll.ForEach(incidencia =>
                    {
                        //Comprobamos que el profesor que registra en la incidecia o el del hecho sea el mismo que ha inciado la sesion y lo añadimos a la lista
                        if (incidencia.IdProfesorRegistra == persona.Profesors.First().IdProfesor | incidencia.IdProfesorHecho == persona.Profesors.First().IdProfesor)
                        {

                            listaAmo.Add(incidencia);
                        }else if (persona.Profesors.First().IdPersonaNavigation.IdRolNavigation.NombreRol.Equals("Tutor") && persona.Profesors.First().IdGrupos.Contains(incidencia.IdAlumnoNavigation.IdGrupoNavigation)  )
                        {
                            listaAmo.Add(incidencia);
                        }
                    });

                    listaAux = new ListCollectionView(listaAmo);
                }
            }else
            {
                listaAux = new ListCollectionView(inicServ.GetAll);
            }

            if(alumno != null)
            {
                _incidencias.IdAlumnoNavigation = alumno;
            }

            criterios = new List<Predicate<Amonestacione>>();

            inicializaCriterios();
        }

        /// <summary>
        /// Metodo que inciliza los criterios
        /// </summary>
        private void inicializaCriterios()
        {
            criteriosFechaInicio = new Predicate<Amonestacione>(m => m.FechaHoraHecho != null && m.FechaHoraHecho >= FechaInicio);
            criteriosFechaFin = new Predicate<Amonestacione>(m => m.FechaHoraHecho != null && m.FechaHoraHecho <= FechaFin);
            criteriosTipo = new Predicate<Amonestacione>(m => m.Tipo != null && m.Tipo.Equals(tipoSeleccionado));
            criteriosMotivo = new Predicate<Amonestacione>(m => m.IdMotivoNavigation != null && m.IdMotivoNavigation.Equals(motivoSeleccionado));
            criterioNia = new Predicate<Amonestacione>(m => m.IdAlumnoNavigation.Nia != null && m.IdAlumnoNavigation.Nia.Contains(niaAlumno));
        }

        /// <summary>
        /// Metodo que develve los alumnos
        /// </summary>
        public List<Alumno> listaAlumnos { get { return alumnoServ.GetAll; } }

        /// <summary>
        /// Metodo que devuelve los tipos
        /// </summary>
        public List<string> listaTipos { get { return new List<string>() { "Amonestación", "Incidencia" }; } }

        /// <summary>
        /// Metodo que devuelve los motivos
        /// </summary>
        public List<Motivo> listaMotivos { get { return motivoServ.GetAll; } }

        /// <summary>
        /// Metodo que devuelve los profesores
        /// </summary>
        public List<Profesor> listaProfesores { get { return profesorServ.GetAll; } }

        /// <summary>
        /// Metodo que devuelve las personas
        /// </summary>
        public List<Persona> listaPersonas { get { return personaServ.GetAll; } }

        /// <summary>
        /// Metodo que devuelve las amonestaciones/incidencias
        /// </summary>
        public ListCollectionView listaIncidencias { get { return listaAux; } }

        /// <summary>
        /// Metodo que almacena al alumno seleccionado por el usuario
        /// </summary>
        public Alumno alumnoSeleccionado
        {
            get { return _alumno; }
            set { _alumno = value; NotifyPropertyChanged(nameof(alumnoSeleccionado)); }
        }

        /// <summary>
        /// Metodo que almacena el tipo seleccionado por el usuario
        /// </summary>
        public string tipoSeleccionado
        {
            get { return _tipo; }
            set { _tipo = value; NotifyPropertyChanged(nameof(tipoSeleccionado)); }
        }

        /// <summary>
        /// Metodo que almacena el motivo seleccionado por el usuario
        /// </summary>
        public Motivo motivoSeleccionado
        {
            get { return _motivo; }
            set { _motivo = value; NotifyPropertyChanged(nameof(motivoSeleccionado)); }
        }

        /// <summary>
        /// Metodo que almacena la fecha de inicio (fecha del hecho) 
        /// </summary>
        public DateTime? FechaInicio
        {
            get { return _fechaInicio; }
            set { _fechaInicio = value; NotifyPropertyChanged(nameof(FechaInicio)); }
        }

        /// <summary>
        /// Metodo que almcena la fecha de fin (fecha del hecho)
        /// </summary>
        public DateTime? FechaFin
        {
            get { return _fechaFin; }
            set { _fechaFin = value; NotifyPropertyChanged(nameof(FechaFin)); }
        }

        /// <summary>
        /// Meodo que almacena una incidencia/amonestacion
        /// </summary>
        public Amonestacione incidencia
        {
            get { return _incidencias; }
            set { _incidencias = value; NotifyPropertyChanged(nameof(incidencia)); }
        }

        /// <summary>
        /// Metodo que almcena un nia
        /// </summary>
        public string niaAlumno
        {
            get { return _niaAlumno; }
            set { _niaAlumno = value; NotifyPropertyChanged(nameof(niaAlumno)); }
        }

        /// <summary>
        /// Recibe un objeto de la lista de incidencias/amonestaciones
        /// La chequea con todos los filtros incluidos en la lista criterios
        /// </summary>
        /// <param name="item">Objeto incidenia/amonestacion</param>
        /// <returns>Verdadero o falso en funcion si cumple con los criterios o no</returns>
        public bool FiltroCombinadoCriterios(object item)
        {
            bool correcto = true;
            Amonestacione amonestacion = (Amonestacione)item;
            if (criterios.Count != 0)
            {
                correcto = criterios.TrueForAll(x => x(amonestacion));
            }

            return correcto;
        }

        /// <summary>
        /// Metodo que se encarga de añadir los filtros que hemos creado
        /// </summary>
        public void AddFiltros()
        {
            criterios.Clear();
            

            if (FechaInicio != null) criterios.Add(criteriosFechaInicio);
            if (FechaFin != null) criterios.Add(criteriosFechaFin);

            if (tipoSeleccionado != null) criterios.Add(criteriosTipo);
            if (motivoSeleccionado != null) criterios.Add(criteriosMotivo);
            if (!string.IsNullOrEmpty(niaAlumno)) criterios.Add(criterioNia);
        }

        /// <summary>
        /// Metodo que se encarga de añadir los filtros y combinarlos
        /// </summary>
        public void Filtrar()
        {
            AddFiltros();
            listaIncidencias.Filter = new Predicate<object>(FiltroCombinadoCriterios);
        }

        /// <summary>
        /// Elimina el filtrado de la lista auxiliar
        /// </summary>
        public void borrarFiltro()
        {
            listaAux.Filter = null;
            _fechaInicio = DateTime.Now;
            _fechaFin = DateTime.Now;
        }

        


    }
}
