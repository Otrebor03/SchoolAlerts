using MahApps.Metro.Controls;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;

namespace ProyectoRRC.Frontend.Dialogos.InsertarActualizar
{
    /// <summary>
    /// Lógica de interacción para DialogoContrasenya.xaml
    /// </summary>
    public partial class DialogoContrasenya : MetroWindow
    {
        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Variable con el modelo vista persona
        /// </summary>
        private MVPersona mvPersona;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        /// <param name="per">Persona</param>
        public DialogoContrasenya(IncidenciaspartesrrcContext con, Persona per)
        {
            InitializeComponent();
            this.contexto = con;
            mvPersona = new MVPersona(contexto);
            mvPersona.persona = per;
        }

        /// <summary>
        /// Metodo que comprueba que el campo contraseña este completo, en caso de estarlo modifica la contraseña del usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (txtPasswordNueva.Password.Equals(""))
            {
                MessageBox.Show("El campo de contraseña nueva esta incompleto", "Campo Nueva Contraseña Vacio", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                mvPersona.persona.Contrasenya = txtPasswordNueva.Password;

                mvPersona.update(mvPersona.persona);

                if (MessageBox.Show("Contraseña modificada con exito", "Contraseña Modificada", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    DialogResult = true;
                }

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
