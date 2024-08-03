using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoRRC.Frontend.UC
{
    /// <summary>
    /// Lógica de interacción para UCGraficoAlumnos.xaml
    /// </summary>
    public partial class UCGraficoAlumnos : UserControl
    {
        /// <summary>
        /// Objeto con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto con el modelo vista de alumno
        /// </summary>
        private MVAlumno mvAlumno;

        /// <summary>
        /// Objeto con la informacion del alumno seleccionado
        /// </summary>
        private Alumno alumno;

        /// <summary>
        /// Objeto con la informacion de la persona que ha iniciado sesion
        /// </summary>
        private Persona persona;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto</param>
        /// <param name="per">Persona que ha iniciado sesion</param>
        public UCGraficoAlumnos(IncidenciaspartesrrcContext con, Persona per)
        {
            InitializeComponent();
            this.contexto = con;
            this.persona = per;
            mvAlumno = new MVAlumno(contexto, persona);
            this.DataContext = mvAlumno;
            
        }

        /// <summary>
        /// Metodo que comprueba si se ha seleccionado un alumno, si es asi muestra parte del interfaz e inicializamos el modelo vista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeAlumnos_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(treeAlumno.SelectedItem is Alumno)
            {
                alumno = (Alumno)treeAlumno.SelectedItem;
                txtNombreAlumno.Text = ((Alumno)treeAlumno.SelectedItem).Nombre +" "+ ((Alumno)treeAlumno.SelectedItem).Apellido1;
                txtNombreAlumno.Visibility = Visibility.Visible;
                tbAlumno.Visibility = Visibility.Visible;
                grafico.Visibility = Visibility.Visible;
                spAnyo.Visibility = Visibility.Visible;
                btnBuscar.Visibility = Visibility.Visible;
                spAnyotxt.Visibility = Visibility.Visible;
                mvAlumno = new MVAlumno(contexto, persona,(Alumno)treeAlumno.SelectedItem, ((DateTime.Now).Year));
                this.DataContext = mvAlumno;
            }
            
        }

        /// <summary>
        /// Metodo que inicializa el modelo vista con la fecha seleccionada cuando se pulsa el boton de buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            mvAlumno = new MVAlumno(contexto, persona, alumno, fechaAnyo.Value.GetValueOrDefault());
            this.DataContext = mvAlumno;
        }
    }
}
