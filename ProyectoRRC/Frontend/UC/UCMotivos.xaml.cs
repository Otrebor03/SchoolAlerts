using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Frontend.Dialogos;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoRRC.Frontend.UC
{
    /// <summary>
    /// Lógica de interacción para UCMotivos.xaml
    /// </summary>
    public partial class UCMotivos : UserControl
    {
        /// <summary>
        /// Objeto con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto con el modelo vista del motivo
        /// </summary>
        private MVMotivo mvMotivo;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        public UCMotivos(IncidenciaspartesrrcContext con)
        {
            InitializeComponent();
            this.contexto = con;
            inicializa();
        }

        /// <summary>
        /// Metodo donde incializamos el modelo vista y la lista de objetos 
        /// </summary>
        private void inicializa()
        {
            mvMotivo = new MVMotivo(contexto);
            this.DataContext = mvMotivo;
        }

        /// <summary>
        /// Metodo que abre dialogo para poder añadir un motivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemEditar_Click(object sender, RoutedEventArgs e)
        {
            DialogoAnyadirMotivoIncidencia dg = new DialogoAnyadirMotivoIncidencia(contexto, (Motivo)dgMotivos.SelectedItem);
            dg.ShowDialog();

            if ((bool)dg.DialogResult)
            {
                dgMotivos.Items.Refresh();
            }
        }

        /// <summary>
        /// Metodo para eliminar un motivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de querer eliminar el motivo?", "Eliminar un motivo", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if(((Motivo)dgMotivos.SelectedItem).Amonestaciones.Count > 0)
                {
                    MessageBox.Show("No se pudo borrar el motivo debido a que algunas amonestaciones o incidencias lo contienen","No se pudo borrar el motivo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    mvMotivo.delete((Motivo)dgMotivos.SelectedItem);
                    inicializa();
                }

                
                
            }
        }

        /// <summary>
        /// Metodo para añadir un motivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnyadirMotivo_Click(object sender, RoutedEventArgs e)
        {
            DialogoAnyadirMotivoIncidencia dg = new DialogoAnyadirMotivoIncidencia(contexto);
            dg.ShowDialog();
            inicializa();
        }

        
    }
}
