using MahApps.Metro.Controls;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using ProyectoRRC.Frontend.Dialogos.InsertarActualizar;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoRRC.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DialogoAnyadirUsuario.xaml
    /// </summary>
    public partial class DialogoAnyadirUsuario : MetroWindow
    {
        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Variable con el modelo vista de la persona
        /// </summary>
        private MVPersona mvPersona;

        /// <summary>
        /// Variable servicio de profesor
        /// </summary>
        private ProfesorServicio profesoServ;

        /// <summary>
        /// Variable servicio de persona
        /// </summary>
        private PersonaServicio personaServ;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        public DialogoAnyadirUsuario(IncidenciaspartesrrcContext con)
        {
            InitializeComponent();
            this.contexto = con;
            mvPersona = new MVPersona(contexto);
            this.DataContext = mvPersona;
            profesoServ = new ProfesorServicio(contexto);
            personaServ = new PersonaServicio(contexto);
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvPersona.OnErrorEvent));
            mvPersona.btnGuardar = btnGuardar;
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        public DialogoAnyadirUsuario(IncidenciaspartesrrcContext con, Persona persona)
        {
            InitializeComponent();
            this.contexto = con;
            mvPersona = new MVPersona(contexto);
            this.DataContext = mvPersona;
            profesoServ = new ProfesorServicio(contexto);
            personaServ = new PersonaServicio(contexto);
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvPersona.OnErrorEvent));
            mvPersona.btnGuardar = btnGuardar;
            mvPersona.persona = persona;

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
        /// Metodo que asigna como contraseña el nombre del usuario, tambien comprueba si el usuario que se esta creando es un tutor para asignarle 
        /// un grupo, comrpueba que el dni del usuario no este ya registrado y en caso de no estarlo guarda al usuario, por ultimo en caso de ser 
        /// un profesor o tutor guarda tambien en el grupo que esta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            
            mvPersona.persona.Contrasenya = mvPersona.persona.Nombre;

            if (mvPersona.persona.IdRolNavigation.NombreRol.Equals("Tutor") && cmbGrupo.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un grupo para un tutor", "Es necesario un grupo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }else
            {

                if (personaServ.obtenerDni().Contains(mvPersona.persona.Dni))
                {
                    MessageBox.Show("El dni ya esta registrado en la base de datos", "No se pudo guardar al usuario", MessageBoxButton.OK, MessageBoxImage.Error);
                }else if (mvPersona.update(mvPersona.persona))
                {
                    if (mvPersona.persona.IdRolNavigation.NombreRol.Equals("Profesor") | mvPersona.persona.IdRolNavigation.NombreRol.Equals("Tutor"))
                    {

                        Profesor p = new Profesor();
                        p.IdPersonaNavigation = mvPersona.persona;
                        p.IdPersona = mvPersona.persona.IdPersona;

                        if (p.IdGrupos.Count > 0)
                        {
                            p.IdGrupos.Add(mvPersona.grupoSeleccionado);
                        }
                        
                        mvPersona.persona.Profesors.Add(p);

                        profesoServ.AddOrUpdate(p);

                    }

                    MessageBox.Show("El usuario se ha creado con exito", "Usuario Creado", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    mvPersona.listaPersonas.EditItem(mvPersona);
                    mvPersona.listaPersonas.CommitEdit();
                    mvPersona.listaPersonas.Refresh();
                }
                else
                {
                    MessageBox.Show("No ha sido posible crear al usuario", "Error al crear el usuario", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
        }

        /// <summary>
        /// Metodo que habilita o deshabilita el combobox de grupo dependiendo del rol seleccionado en el usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGrupo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Role rol = (Role)cmbRol.SelectedItem;
            
            switch (rol.NombreRol)
            {
                case "Profesor":
                    cmbGrupo.IsEnabled = true;
                    break;
                case "Tutor":
                    cmbGrupo.IsEnabled = true;
                    break;
                default:
                    cmbGrupo.IsEnabled=false;
                    break;
            }
            

            
        }

        /// <summary>
        /// Metodo que abre el dialogo de crear un grupo y cierra el actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCrearGrupo_Click(object sender, RoutedEventArgs e)
        {
            DialogoCrearGrupo dg = new DialogoCrearGrupo(contexto);
            dg.ShowDialog();

            if ((bool)dg.DialogResult)
            {
                DialogResult = false;
                DialogoAnyadirUsuario dgAnyadirUsuario = new DialogoAnyadirUsuario(contexto, mvPersona.persona);
                dgAnyadirUsuario.ShowDialog();
            }
            


        }
    }
}
