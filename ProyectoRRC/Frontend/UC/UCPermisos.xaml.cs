using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Frontend.Dialogos.InsertarActualizar;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoRRC.Frontend.UC
{
    /// <summary>
    /// Lógica de interacción para UCPermisos.xaml
    /// </summary>
    public partial class UCPermisos : UserControl
    {
        /// <summary>
        /// Objeto con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto con el modelo vista de permiso
        /// </summary>
        private MVPermiso mvPermiso;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        public UCPermisos(IncidenciaspartesrrcContext con)
        {
            InitializeComponent();
            this.contexto = con;
            inicializa();
        }

        /// <summary>
        /// Metodo donde inicializamos el modelo vista
        /// </summary>
        private void inicializa()
        {
            mvPermiso = new MVPermiso(contexto);
            this.DataContext = mvPermiso;
        }

        /// <summary>
        /// Metodo que muestra parte del interfaz cuando seleccionamos un permiso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treePermisos_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(treePermisos.SelectedItem is Permiso)
            {
                txtNombrePermiso.Text = ((Permiso)treePermisos.SelectedItem).IdPermiso.ToString();
                dgPermiso.ItemsSource = ((Permiso)treePermisos.SelectedItem).IdRols;
                spAnyadirRol.Visibility = Visibility.Visible;
            }
            else
            {
                txtNombrePermiso.Text = string.Empty;
                spAnyadirRol.Visibility = Visibility.Hidden; 
            }
        }

        private void btnAnyadirPermiso_Click(object sender, RoutedEventArgs e)
        {
            DialogoAnyadirPermiso dg = new DialogoAnyadirPermiso(contexto);
            dg.ShowDialog();

            if ((bool)dg.DialogResult)
            {
                inicializa();
                treeViewItem.IsExpanded = true;
            }
        }

        /// <summary>
        /// Metodo que comprueba que se haya elegido un permiso y lo elimina 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrarPermiso_Click(object sender, RoutedEventArgs e)
        {
            if (((Permiso)treePermisos.SelectedItem) != null)
            {

                if (MessageBox.Show("¿Esta seguro de querer eliminar el permiso?", "Eliminar permiso", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    ((Permiso)treePermisos.SelectedItem).IdRols.Clear();
                    mvPermiso.delete(((Permiso)treePermisos.SelectedItem));
                    MessageBox.Show("Permiso eliminado con exito", "Eliminar permiso", MessageBoxButton.OK, MessageBoxImage.Information);
                    inicializa();
                    dgPermiso.ItemsSource = null;

                }
            }
            else
            {
                MessageBox.Show("Porfavor seleccione un permiso", "Eliminar permiso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       

        /// <summary>
        /// Metodo donde comprobamos que se ha elegido un permiso y un rol y se comprueba que dicho permiso no tenga ya ese rol. En caso de no tenerlo lo añade
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnyadirRol_Click(object sender, RoutedEventArgs e)
        {
            mvPermiso.permiso = ((Permiso)treePermisos.SelectedItem);

            if (mvPermiso.rolSeleccionado != null)
            {
                if (mvPermiso.permiso.IdRols.Contains(mvPermiso.rolSeleccionado))
                {
                    MessageBox.Show("El rol seleccionado ya esta en este permiso", "No se pudo añadir el rol", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    
                    mvPermiso.permiso.IdRols.Add(mvPermiso.rolSeleccionado);
                    mvPermiso.update(mvPermiso.permiso);
                    
                    

                    dgPermiso.ItemsSource = null;
                    dgPermiso.ItemsSource = mvPermiso.permiso.IdRols;
                }
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ningun rol", "No se pudo añadir el rol", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
