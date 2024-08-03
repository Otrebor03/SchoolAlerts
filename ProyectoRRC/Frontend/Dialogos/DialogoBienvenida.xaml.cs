using MahApps.Metro.Controls;
using Microsoft.EntityFrameworkCore;
using ProyectoRRC.Backend.Modelo;
using System.Windows;
using System.Windows.Threading;

namespace ProyectoRRC.Frontend.Dialogos
{
    /// <summary>
    /// Dialogo donde se da la bienvenida al usuario, se realiza la conexion con la base de datos y donde podrá abrir el dialogo para iniciar sesion
    /// </summary>
    public partial class DialogoBienvenida : MetroWindow
    {

        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        DispatcherTimer dt = new DispatcherTimer();

        /// <summary>
        /// Metodo que comprueba que se ha podido conectar con la base de datos e inicia
        /// </summary>
        public DialogoBienvenida()
        {
           

            if (ConectaBD())
            {
                InitializeComponent();
                dt.Tick += new EventHandler(btnIniciarSesion_Click);
                dt.Interval = new TimeSpan(0,0,2);
                dt.Start();
            }
            else
            {
                
                MessageBox.Show("ERROR, no hay comunicacion con la base de datos", "ACCESO A LA BASE DE DATOS", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }

        }



        /// <summary>
        /// Boton qe abre la ventana de iniciar sesion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            DialogoLogin dgLogin = new DialogoLogin();
            dgLogin.Show();
            dt.Stop();
            this.Close();
        }

        /// <summary>
        /// Metodo que conecta con la base de datos
        /// </summary>
        /// <returns></returns>
        private bool ConectaBD()
        {
            bool conecta = true;
            contexto = new IncidenciaspartesrrcContext();

            try
            {
                contexto.Database.OpenConnection();
            }catch(Exception ex)
            {

                conecta = false;
            }

            return conecta;
        }
    }
}
