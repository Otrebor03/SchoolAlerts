using MahApps.Metro.Controls;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoRRC.Frontend.Dialogos.InsertarActualizar
{
    /// <summary>
    /// Lógica de interacción para DialogoAnyadirPermiso.xaml
    /// </summary>
    public partial class DialogoAnyadirPermiso : MetroWindow
    {

        /// <summary>
        /// Objeto con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto con el modelo vista de permiso
        /// </summary>
        private MVPermiso mvPermiso;


        /// <summary>
        /// Cosntructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        public DialogoAnyadirPermiso(IncidenciaspartesrrcContext con)
        {
            InitializeComponent();
            this.contexto = con;
            mvPermiso = new MVPermiso(contexto);
            this.DataContext = mvPermiso;
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvPermiso.OnErrorEvent));
            mvPermiso.btnGuardar = btnGuardar;
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

        /// <summary>
        /// Metodo que crea un nuevo permiso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            mvPermiso.update(mvPermiso.permiso);
            MessageBox.Show("El permiso ha sido registrado", "Permiso creado con éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
        }
    }
}
