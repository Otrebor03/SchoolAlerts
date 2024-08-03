using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Frontend.Dialogos.InsertarActualizar;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoRRC.Frontend.UC
{
    /// <summary>
    /// Lógica de interacción para UCRol.xaml
    /// </summary>
    public partial class UCRol : UserControl
    {
        /// <summary>
        /// Objeto con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto con el modelo vista del rol
        /// </summary>
        private MVRol mvRol;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        public UCRol(IncidenciaspartesrrcContext con)
        {
            InitializeComponent();
            this.contexto = con;
            inicializa();
        }

        /// <summary>
        /// Metodo donde inicializamos el contexto y el modelo vista
        /// </summary>
        private void inicializa()
        {
            mvRol = new MVRol(contexto);
            this.DataContext = mvRol;
        }

        /// <summary>
        /// Metodo donde dependiendo si tenemos elegido un rol o no, mostramos u ocultamos parte del interfaz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeRol_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(treeRol.SelectedItem is Role)
            {
                dgRol.ItemsSource = ((Role)treeRol.SelectedItem).IdPermisos;
                txtNombreRol.Text = ((Role)treeRol.SelectedItem).NombreRol;
                txtElegirPermiso.Visibility = Visibility.Visible;
                cmbSeleccionarPermiso.Visibility = Visibility.Visible;
                btnAnyadirPermiso.Visibility = Visibility.Visible;
                btnBorrarRol.Visibility = Visibility.Visible;
            }
            else
            {
                txtElegirPermiso.Visibility = Visibility.Hidden;
                cmbSeleccionarPermiso.Visibility = Visibility.Hidden;
                btnAnyadirPermiso.Visibility = Visibility.Hidden;
                btnBorrarRol.Visibility = Visibility.Hidden;
                txtNombreRol.Text = string.Empty;
            }
        }

        /// <summary>
        /// Metodo el cual elimina el rol que hemos seleccionado, comprueba si el rol aun lo tienen los usuarios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrarRol_Click(object sender, RoutedEventArgs e)
        {
            if(((Role)treeRol.SelectedItem) != null)
            {
                if (MessageBox.Show("Esta seguro de querer eliminar el rol", "Eliminar rol", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    if (((Role)treeRol.SelectedItem).Personas.Count > 0)
                    {
                        MessageBox.Show("No se puede borrar este rol debido a que hay usuarios que pertenecen a este rol", "No se pudo eliminar el rol", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        ((Role)treeRol.SelectedItem).IdPermisos.Clear();
                        mvRol.delete(((Role)treeRol.SelectedItem));
                        MessageBox.Show("Rol eliminado con exito", "Eliminar rol", MessageBoxButton.OK, MessageBoxImage.Information);
                        inicializa();
                        dgRol.ItemsSource = null;
                    }
                }
            }
            else
            {
                MessageBox.Show("Porfavor seleccione un rol", "Eliminar rol", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            

            
        }

        /// <summary>
        /// Metodo que nos permite añadir permisos al rol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditarRol_Click(object sender, RoutedEventArgs e)
        {
            if(mvRol.permisoSeleccionado != null)
            {

                 if (mvRol.rol.IdPermisos.Contains(mvRol.permisoSeleccionado)) 
                {
                    MessageBox.Show("El permiso seleccionado ya esta en este rol", "No se pudo añadir el permiso", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    mvRol.rol = ((Role)treeRol.SelectedItem);
                    mvRol.rol.IdPermisos.Add(mvRol.permisoSeleccionado);
                    mvRol.update(mvRol.rol);
                    dgRol.ItemsSource = null; 
                    dgRol.ItemsSource = mvRol.rol.IdPermisos;
                }
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ningun permiso", "No se pudo añadir el permiso", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metodo que nos permite crear un rol
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCrearRol_Click(object sender, RoutedEventArgs e)
        {
            DialogoCrearRol dg = new DialogoCrearRol(contexto);
            dg.ShowDialog();

            if ((bool)dg.DialogResult)
            {

                inicializa();
                treeViewItem.IsExpanded = true;
            }
        }

        /// <summary>
        /// Metodo que nos permite eliminar permisos de los roles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de querer eliminar el permiso del rol?", "Eliminar permiso de rol", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if(((Role)treeRol.SelectedItem).IdPermisos.Contains((Permiso)dgRol.SelectedItem)) 
                {

                    ((Role)treeRol.SelectedItem).IdPermisos.Remove((Permiso)dgRol.SelectedItem);
                    mvRol.update(mvRol.rol);
                    dgRol.ItemsSource = null; 
                    dgRol.ItemsSource = mvRol.rol.IdPermisos;
                }
                else
                {
                    MessageBox.Show("El rol no contiene este permiso", "Error al eliminar el permiso", MessageBoxButton.OK, MessageBoxImage.Error);
                }
      
            }
            else
            {
                MessageBox.Show("No se pudo eliminar el permiso", "Error al eliminar el permiso", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }
    }
}
