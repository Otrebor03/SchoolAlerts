using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;

namespace ProyectoRRC.Frontend.MVVM
{
    /// <summary>
    /// Clase modelo vista del objeto profesor
    /// </summary>
    internal class MVProfesor:MVBaseCRUD<Profesor>
    {
        /// <summary>
        /// Objeto con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto donde almacenamos al profesor
        /// </summary>
        private Profesor _profesor;

        /// <summary>
        /// Objeto donde almacenamos el servicio
        /// </summary>
        private ProfesorServicio profesorServ;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        public MVProfesor(IncidenciaspartesrrcContext con)
        {
            this.contexto = con;
            inicializa();
        }

        /// <summary>
        /// Metodo donde iniclizamos el servicio y objeto
        /// </summary>
        private void inicializa()
        {
            _profesor = new Profesor();
            profesorServ = new ProfesorServicio(contexto);

        }

        /// <summary>
        /// Metodo donde almacenamos al profesor
        /// </summary>
        public Profesor profesor
        {
            get { return _profesor; }
            set { _profesor = value; NotifyPropertyChanged(nameof(profesor)); }
        }
    }
}
