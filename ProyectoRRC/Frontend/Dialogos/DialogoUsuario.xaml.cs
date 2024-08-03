using MahApps.Metro.Controls;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ProyectoRRC.Frontend.Dialogos.Admin
{
    /// <summary>
    /// Dialogo donde se muestra la informacion del usuario
    /// </summary>
    public partial class DialogoUsuario : MetroWindow
    {
        /// <value>
        /// Variable que almacena el contexto de la base de datos
        /// </value>
        private IncidenciaspartesrrcContext contexto;

        /// <value>
        /// Variable donde almacenamo la informacion de la persona que ha inicado sesion
        /// </value>
        private Persona usuario;

        /// <value>
        /// Variable con el modelo vista
        /// </value>
        private MVPersona mvPersona;

        /// <summary>
        /// Constructor de la clase, tambien establece la imagen del usuario
        /// </summary>
        /// <param name="cont"></param>
        /// <param name="pers"></param>
        public DialogoUsuario(IncidenciaspartesrrcContext cont, Persona pers)
        {
            InitializeComponent();
            this.contexto = cont;
            mvPersona = new MVPersona(contexto);
            this.DataContext = mvPersona;
            //this.mvPersona = pers;
            mvPersona.persona = pers;

            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();

            
                bitmap.UriSource = new Uri("../../../Recursos/Iconos/fotoPersona.png", UriKind.Relative);
                bitmap.EndInit();
                imgUsuario.Source = bitmap;
            
        }


        
        
        /// <summary>
        /// Metodo que cierra el dialogo actual y abre el dialogo de inicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            
            DialogResult = false;
         
        }

        /// <summary>
        /// Metodo el cual abre un dialogo para modificar la informacion del usuario que ha inicado sesion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            DialogoUsuarioEditar dgUsuarioAdminEditar = new DialogoUsuarioEditar(contexto, mvPersona.persona);
            dgUsuarioAdminEditar.Show();
            this.Close();
        }

        private void btnModificarContra_Click(object sender, RoutedEventArgs e)
        {
            DialogoModificarContrasenya dgModificarContrasenya = new DialogoModificarContrasenya(contexto, mvPersona.persona);
            dgModificarContrasenya.ShowDialog();
        }
    }
}
