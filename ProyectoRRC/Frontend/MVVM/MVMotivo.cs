using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using System.Windows.Data;

namespace ProyectoRRC.Frontend.MVVM
{
    /// <summary>
    /// Clase modelo vista del objeto Motivo
    /// </summary>
    class MVMotivo :MVBaseCRUD<Motivo>
    {
        /// <summary>
        /// Objeto con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto donde almacenamos el motivo
        /// </summary>
        private Motivo _motivo;

        /// <summary>
        /// Obtjeto donde almacenamos el servicio de motivo
        /// </summary>
        private MotivoServicio motivoServ;

        /// <summary>
        /// Lista auxiliar donde almacenamos los motivos
        /// </summary>
        private ListCollectionView listaAux;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contexto">Contexto de la base de datos</param>
        public MVMotivo(IncidenciaspartesrrcContext contexto)
        {
            this.contexto = contexto;
            inicializa();
        }

        /// <summary>
        /// Metodo donde incilizamos los objetos, servicios y lista auxiliar
        /// </summary>
        private void inicializa()
        {
            _motivo =new Motivo();
            motivoServ = new MotivoServicio(contexto);
            servicio = motivoServ;
            listaAux = new ListCollectionView(motivoServ.GetAll);
        }

        /// <summary>
        /// Lista donde almacenmaos todos los motivos
        /// </summary>
        public ListCollectionView listaMotivos { get { return listaAux; } }

        /// <summary>
        /// Metodo donde almacenamos el motivo
        /// </summary>
        public Motivo motivo
        {
            get { return _motivo; }
            set { _motivo = value; NotifyPropertyChanged(nameof(motivo)); }
        }
    }
}
