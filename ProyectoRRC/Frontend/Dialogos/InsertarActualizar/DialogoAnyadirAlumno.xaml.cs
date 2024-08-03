using MahApps.Metro.Controls;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoRRC.Frontend.Dialogos.InsertarActualizar
{
    /// <summary>
    /// Lógica de interacción para DialogoAnyadirAlumno.xaml
    /// </summary>
    public partial class DialogoAnyadirAlumno : MetroWindow
    {

        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Variable modelo vista del alumno
        /// </summary>
        private MVAlumno mvAlumno;

        /// <summary>
        /// Variale servicio del alumno
        /// </summary>
        private AlumnosServicio alumnoServ;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        public DialogoAnyadirAlumno(IncidenciaspartesrrcContext con)
        {
            InitializeComponent();
            this.contexto = con;
            inicializa();
        }

        /// <summary>
        /// Metodo que inicializa el modelo vista y el servicio
        /// </summary>
        private void inicializa()
        {
            mvAlumno = new MVAlumno(contexto);
            alumnoServ = new AlumnosServicio(contexto);
            this.DataContext = mvAlumno;
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvAlumno.OnErrorEvent));
            mvAlumno.btnGuardar = btnGuardar;
        }

        /// <summary>
        /// Metodo que comprubea que el NIA no este repetido y en caso de no estarlo guarda el alumno
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (alumnoServ.obtenerNia().Contains(mvAlumno.alumno.Nia))
            {
                MessageBox.Show("El nia ya esta registrado en la base de datos", "No se pudo guardar al alumno", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (mvAlumno.update(mvAlumno.alumno))
                {
                    MessageBox.Show("Alumno añadido correctamente", "Alumno Añadido", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                }
            }
            
        }

        /// <summary>
        /// Metodo que cierra el dialogo actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Metodo que abre el dialogo Crear grupo y cierra el actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCrearGrupo_Click(object sender, RoutedEventArgs e)
        {
            
            DialogoCrearGrupo dg = new DialogoCrearGrupo(contexto);
            dg.ShowDialog();



            if ((bool)dg.DialogResult)
            {
                cmbGrupo.ItemsSource = null;
                cmbGrupo.ItemsSource = mvAlumno.listaGrupos;
            }
            
        }
    }
}
