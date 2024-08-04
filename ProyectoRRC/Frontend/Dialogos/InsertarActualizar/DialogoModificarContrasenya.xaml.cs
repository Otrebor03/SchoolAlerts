using MahApps.Metro.Controls;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;

namespace ProyectoRRC.Frontend.Dialogos
{
    /// <summary>
    /// Ventana emergente donde el usuario podra modificar su contraseña, antes debea introducir la contraseña anterior
    /// </summary>
    public partial class DialogoModificarContrasenya : MetroWindow
    {
        /// <value>
        /// Variable que almacena el contexto de la base de datos
        /// </value>
        private IncidenciaspartesrrcContext contexto;

        /// <value>
        /// Variable con el modelo vista
        /// </value>
        private MVPersona mvPersona;

        private PersonaServicio personaServ;

        /// <summary>
        /// Constructo de la clase
        /// </summary>
        /// <param name="con"></param>
        /// <param name="per"></param>
        public DialogoModificarContrasenya(IncidenciaspartesrrcContext con, Persona per)
        {
            InitializeComponent();
            this.contexto = con;
            personaServ = new PersonaServicio(contexto);
            mvPersona = new MVPersona(contexto);
            mvPersona.persona = per;
        }

        /// <summary>
        /// Metodo que cierra la ventana actual y volver al dialogo anterior
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Metodo que comprueba los campos y si todo es correcto guarda la contraseña
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        { 
            if (txtPasswordAnterior.Password.Equals(""))
            { 

                MessageBox.Show("El campo de contraseña anterior esta incompleto", "Campo Anterior Contraseña Vacio", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            else if (!txtPasswordAnterior.Password.Equals(personaServ.Desencriptar(mvPersona.persona.Contrasenya)))
            {
                MessageBox.Show("La contraseña no coincide con la anterior", "Contraseña Anterior Incorrecta", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (txtPasswordNueva.Password.Equals(""))
                {
                    MessageBox.Show("El campo de contraseña nueva esta incompleto", "Campo Nueva Contraseña Vacio", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    
                    mvPersona.persona.Contrasenya = personaServ.Encriptar(txtPasswordNueva.Password);

                    mvPersona.update(mvPersona.persona);

                    if (MessageBox.Show("Contraseña modificada con exito", "Contraseña Modificada", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                    {
                        DialogResult = true;
                    }

                }
            }

            
        }

    }
}
