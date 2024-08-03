using MahApps.Metro.Controls;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoRRC.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DialogoAnyadirMotivoIncidencia.xaml
    /// </summary>
    public partial class DialogoAnyadirMotivoIncidencia : MetroWindow
    {
        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Variable con el modelo vista de motivo
        /// </summary>
        private MVMotivo mvMotivo;

        /// <summary>
        /// Variable con el modelo vista de incidencias
        /// </summary>
        private MVIncidencias mvIncidencias;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Contexto de la base de datos</param>
        public DialogoAnyadirMotivoIncidencia(IncidenciaspartesrrcContext context)
        {
            InitializeComponent();
            this.contexto = context;
            mvMotivo = new MVMotivo(contexto);
            mvIncidencias = new MVIncidencias(contexto);
            this.DataContext = mvMotivo;
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvIncidencias.OnErrorEvent));
            mvIncidencias.btnGuardar = btnGuardar;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        /// <param name="mot">Motivo</param>
        public DialogoAnyadirMotivoIncidencia(IncidenciaspartesrrcContext con, Motivo mot) 
        {
            InitializeComponent();
            this.contexto = con;
            mvMotivo = new MVMotivo(contexto);
            this.DataContext= mvMotivo;
            mvMotivo.motivo = mot;
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvMotivo.OnErrorEvent));
            mvMotivo.btnGuardar = btnGuardar;
        }

        /// <summary>
        /// Metodo que almacena el motivo en la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (mvMotivo.update(mvMotivo.motivo))
            {
                MessageBox.Show("El motivo ha sido añadido a la base de datos","Motivo Añadido",MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;

            }
            else
            {
                MessageBox.Show("No se pudo añadir el motivo en la base de datos", "Error al añadir el motivo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metodo que cierra el dialogo actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
