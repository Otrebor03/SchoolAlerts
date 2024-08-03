using MahApps.Metro.Controls;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoRRC.Frontend.Dialogos.InsertarActualizar
{
    /// <summary>
    /// Lógica de interacción para DialogoCrearRol.xaml
    /// </summary>
    public partial class DialogoCrearRol : MetroWindow
    {
        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Variable con el modelo vista del rol
        /// </summary>
        private MVRol mvRol;

        /// <summary>
        /// Variable con el servicio del rol
        /// </summary>
        private RolServicio rolServ;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        public DialogoCrearRol(IncidenciaspartesrrcContext con)
        {
            InitializeComponent();
            this.contexto = con;
            mvRol = new MVRol(contexto);
            rolServ = new RolServicio(contexto);
            this.DataContext = mvRol;
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvRol.OnErrorEvent));
            mvRol.btnGuardar = btnGuardar;
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
        /// Metodo que comprueba que el rol no este repetido y lo almacena en la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (rolServ.listaNombresRol().Contains(txtNombreRol.Text))
            {
                MessageBox.Show("El nombre del rol que has introducido ya esta registrado", "No se pudo registrar el rol", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                mvRol.update(mvRol.rol);
                MessageBox.Show("El rol ha sido registrado", "Rol creado con éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
        }
    }
}
