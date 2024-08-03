using MahApps.Metro.Controls;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ProyectoRRC.Frontend.Dialogos.Admin
{
    /// <summary>
    /// Dialogo en el cual el usuario puede modificar su propia informacion
    /// </summary>
    public partial class DialogoUsuarioEditar : MetroWindow
    {

        /// <value>
        /// Variable que almacena el contexto de la base de datos
        /// </value>
        private IncidenciaspartesrrcContext contexto;

       

        /// <value>
        /// Variable con el modelo vista
        /// </value>
        private MVPersona mvPersona;

        /// <summary>
        /// Constructor del dialogo, tambien establece la imagen del usuario
        /// </summary>
        /// <param name="cont"></param>
        /// <param name="pers"></param>
        public DialogoUsuarioEditar(IncidenciaspartesrrcContext cont, Persona pers)
        {
            InitializeComponent();
            this.contexto = cont;
            mvPersona = new MVPersona(contexto);
            this.DataContext = mvPersona;
            mvPersona.persona = pers;


            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();

            bitmap.UriSource = new Uri("../../../Recursos/Iconos/fotoPersona.png", UriKind.Relative);
            bitmap.EndInit();
            imgUsuario.Source = bitmap;

            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvPersona.OnErrorEvent));
            mvPersona.btnGuardar = btnGuardar;
        }

        /// <summary>
        /// Metodo dondo volvemos al dialogo anterior
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            
            DialogoUsuario dg = new DialogoUsuario(contexto, mvPersona.persona);
            dg.Show();
            
            this.Close();
        }

        /// <summary>
        /// Metodo donde comprobamos que los datos obligatorios esten completos y actualizamos la informacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (mvPersona.IsValid(this))
            {
                if (mvPersona.update(mvPersona.persona))
                {

                    if (MessageBox.Show("Se ha actualizado la información del usuario", "Información Actualizada", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                    {

                        DialogResult=true;

                    }
                }
            }
            else
            {
                MessageBox.Show("No se ha podido guardar la informacion", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        /// <summary>
        /// Metodo que muestra un mensaje para cerrar la sesion, si se pulsa si, la sesion sera cerrada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar la sesión?", "Cerrar Sesión", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DialogoLogin dgLogin = new DialogoLogin();
                dgLogin.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Metodo que abre el dialogo para modificar la contraseña del usuario que ha iniciado sesion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModificarContra_Click(object sender, RoutedEventArgs e)
        {
            DialogoModificarContrasenya dgModificar = new DialogoModificarContrasenya(contexto, mvPersona.persona);
            dgModificar.Show();

        }

    }
}
