using MahApps.Metro.Controls;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoRRC.Frontend.Dialogos.InsertarActualizar
{
    /// <summary>
    /// Lógica de interacción para DialogoCrearGrupo.xaml
    /// </summary>
    public partial class DialogoCrearGrupo : MetroWindow
    {
        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Variable con el modelo vista grupo
        /// </summary>
        private MVGrupo mvGrupo;

        /// <summary>
        /// Variable con el servicio del grupo
        /// </summary>
        private GrupoServicio grupoServ;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        public DialogoCrearGrupo(IncidenciaspartesrrcContext con)
        {
            InitializeComponent();
            this.contexto = con;
            mvGrupo = new MVGrupo(contexto);
            grupoServ = new GrupoServicio(contexto);
            this.DataContext = mvGrupo;
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvGrupo.OnErrorEvent));
            mvGrupo.btnGuardar = btnGuardar;
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
        /// Metodo que comprueba que el grupo no este repetido y lo guarda en la base de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (grupoServ.nombreGrupos().Contains(mvGrupo.grupo.NombreGrupo))
            {
                MessageBox.Show("No se pudo crear el grupo debido a que ya existe un grupo con este nombre", "No se pudo crear el grupo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (mvGrupo.update(mvGrupo.grupo))
                {
                    MessageBox.Show("El grupo se creó correctamente", "Grupo Creado", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;

                }
                else
                {
                    MessageBox.Show("No se pudo crear el grupo debido a un error", "Error al crear el grupo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            
            
        }
    }
}
