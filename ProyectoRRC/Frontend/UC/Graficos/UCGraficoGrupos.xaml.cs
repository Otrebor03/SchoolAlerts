using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoRRC.Frontend.UC
{
    /// <summary>
    /// Lógica de interacción para UCGraficoGrupos.xaml
    /// </summary>
    public partial class UCGraficoGrupos : UserControl
    {
        /// <summary>
        /// Objeto con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto con el modelo vista de grupo
        /// </summary>
        private MVGrupo mvGrupo;

        /// <summary>
        /// Objeto donde almacenamos el grupo
        /// </summary>
        private Grupo grupo;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        /// <param name="per">Persona que ha iniciado sesion</param>
        public UCGraficoGrupos(IncidenciaspartesrrcContext con, Persona per)
        {
            InitializeComponent();
            this.contexto = con;
            mvGrupo = new MVGrupo(contexto, per);
            this.DataContext = mvGrupo; 
        }

        /// <summary>
        /// Metodo que inicializa el modelo vista con la fecha seleccionada cuando se pulsa el boton de buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            mvGrupo = new MVGrupo(contexto, grupo, fechaAnyo.Value.GetValueOrDefault());
            this.DataContext = mvGrupo;
        }

        /// <summary>
        /// Metodo que comprueba si se ha seleccionado un alumno, si es asi muestra parte del interfaz e inicializamos el modelo vista
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeGrupo_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeGrupo.SelectedItem is Grupo)
            {
                grupo = (Grupo)treeGrupo.SelectedItem;
                txtNombreGrupo.Text = ((Grupo)treeGrupo.SelectedItem).NombreGrupo;
                txtNombreGrupo.Visibility = Visibility.Visible;
                tbGrupo.Visibility  = Visibility.Visible;
                grafico.Visibility = Visibility.Visible;
                spAnyo.Visibility = Visibility.Visible;
                btnBuscar.Visibility = Visibility.Visible;
                spAnyotxt.Visibility = Visibility.Visible;
                mvGrupo = new MVGrupo(contexto, (Grupo)treeGrupo.SelectedItem, ((DateTime.Now).Year));
                this.DataContext = mvGrupo;
            }
        }
    }
}
