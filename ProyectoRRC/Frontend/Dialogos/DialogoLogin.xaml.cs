using MahApps.Metro.Controls;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using ProyectoRRC.Frontend.Dialogos.Admin;
using System.Windows;

namespace ProyectoRRC.Frontend.Dialogos
{
    /// <summary>
    /// Dialogo donde el usuario puede iniciar sesion, si algo no es correcto se notificará
    /// </summary>
    public partial class DialogoLogin : MetroWindow
    {
        /// <summary>
        /// Contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Constructor
        /// </summary>
        public DialogoLogin()
        {
            
            InitializeComponent();
            contexto = new IncidenciaspartesrrcContext();
        }

        /// <summary>
        /// Metodo que se encarga de comprobar el usuario y en caso de que este si este registrado verifica su rol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            PersonaServicio personaServ = new PersonaServicio(contexto);

            if(personaServ.login(txtUsuario.Text, txtPassword.Password))
            {

                DialogoInicio dgInicio = new DialogoInicio(contexto, personaServ.usuLogin);
                dgInicio.Show();
                this.Close();

            }
            else if(txtUsuario.Text == "" && txtPassword.Password == "")
            {
                MessageBox.Show("No has introducido el usuario ni la contraseña", "Error Inicio Sesión", MessageBoxButton.OK, MessageBoxImage.Error);
            }else if(txtUsuario.Text == "")
            {
                MessageBox.Show("No has introducido el usuario", "Error Inicio Sesión", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(txtPassword.Password == "")
            {
                MessageBox.Show("No has introducido la contraseña", "Error Inicio Sesión", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos", "Error Inicio Sesión", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
