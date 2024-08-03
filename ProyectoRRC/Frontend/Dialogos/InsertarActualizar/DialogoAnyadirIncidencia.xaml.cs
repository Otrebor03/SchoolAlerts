using MahApps.Metro.Controls;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoRRC.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DialogoAnyadirIncidencia.xaml
    /// </summary>
    public partial class DialogoAnyadirIncidencia : MetroWindow
    {
        /// <summary>
        /// Variable con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Variable de tipo Persona
        /// </summary>
        private Persona usuario;

        /// <summary>
        /// Variable Alumno
        /// </summary>
        private Alumno alumno;

        /// <summary>
        /// Variable modelo vista incidencias
        /// </summary>
        private MVIncidencias mvIncidencias;


        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        /// <param name="amo">Amonestacion</param>
        public DialogoAnyadirIncidencia(IncidenciaspartesrrcContext con, Amonestacione amo)
        {
            InitializeComponent();
            this.contexto = con;
            mvIncidencias = new MVIncidencias(contexto);
            this.DataContext = mvIncidencias;
            mvIncidencias.incidencia = amo;
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvIncidencias.OnErrorEvent));
            mvIncidencias.btnGuardar = btnGuardar;

            if (mvIncidencias.incidencia.Tipo != null)
            {
                if (mvIncidencias.incidencia.Tipo.Equals("Amonestación"))
                {

                    rbAmonestacion.IsChecked = true;
                    rbIncidencia.IsChecked = false;
                }
                else
                {
                    rbIncidencia.IsChecked = true;
                    rbAmonestacion.IsChecked = false;
                }
            }

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        /// <param name="pro">Persona</param>
        /// <param name="al">Alumno</param>
        public DialogoAnyadirIncidencia(IncidenciaspartesrrcContext con, Persona pro, Alumno al)
        {
            InitializeComponent();
            this.contexto = con;
            this.usuario = pro;
            this.alumno = al;
            mvIncidencias = new MVIncidencias(contexto, usuario, alumno);
            this.DataContext = mvIncidencias;
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(mvIncidencias.OnErrorEvent));
            mvIncidencias.btnGuardar = btnGuardar;

            

        }

        /// <summary>
        /// Metodo que comprueba si los parametros estan introducids correctamente, si es asi guarda la incidencia/amonestacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            
            if(rbAmonestacion.IsChecked == true) 
            {
                mvIncidencias.incidencia.Tipo = "Amonestación";
            }
            else
            {
                mvIncidencias.incidencia.Tipo = "Incidencia";
            }

            //Comprobacion de las fechas si todo es correcto se guarda
            if(mvIncidencias.incidencia.FechaHoraHecho > mvIncidencias.incidencia.FechaHoraRegistro)
            {
                MessageBox.Show("La fecha del hecho no puede ser posterior a la del registro, reviselo", "Revisar Fechas", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (mvIncidencias.update(mvIncidencias.incidencia) )
            {
                MessageBox.Show("La incidencia fue editada con exito", "Incidencia Editada", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                mvIncidencias.listaIncidencias.EditItem(mvIncidencias);
                mvIncidencias.listaIncidencias.CommitEdit();
                mvIncidencias.listaIncidencias.Refresh();
            }
            else
            {
                MessageBox.Show("Algo no salió bien al editar la incidencia", "Error al editar la incidencia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Metodo que cierra el dialogo actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelr_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Metodo que abre el dialogo para añadir un motivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnyadirMotivo_Click(object sender, RoutedEventArgs e)
        {
            DialogoAnyadirMotivoIncidencia dg = new DialogoAnyadirMotivoIncidencia(contexto);
            dg.ShowDialog();
            

            if ((bool)dg.DialogResult)
            {

                cmbMotivo.ItemsSource = null;
                cmbMotivo.ItemsSource = mvIncidencias.listaMotivos;
                
                
            }
            
            
        }
    }
}
