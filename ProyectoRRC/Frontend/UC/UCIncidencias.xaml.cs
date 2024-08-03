using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Frontend.Dialogos;
using ProyectoRRC.Frontend.MVVM;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoRRC.Frontend.UC
{
    /// <summary>
    /// Lógica de interacción para UCIncidencias.xaml
    /// </summary>
    public partial class UCIncidencias : UserControl
    {
        /// <summary>
        /// Objeto con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;
        
        /// <summary>
        /// Objeto con el modelo vista de incidencias/amonestaciones
        /// </summary>
        private MVIncidencias mvIncidencias;

        /// <summary>
        /// Objeto persona que ha inciado sesion
        /// </summary>
        private Persona persona;

        /// <summary>
        /// Lista de amonestaciones/incidencias seleccionadas
        /// </summary>
        private List<Amonestacione> incidenciasSeleccionadas;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Contexto de la base de datos</param>
        /// <param name="per">Persona que ha iniciado sesion</param>
        public UCIncidencias(IncidenciaspartesrrcContext context, Persona per)
        {
            InitializeComponent();
            this.contexto = context;
            this.persona = per;
            inicializa();
        }

        /// <summary>
        /// Metodo donde incializamos el modelo vista y la lista
        /// </summary>
        private void inicializa()
        {

            mvIncidencias = new MVIncidencias(contexto, persona);
            this.DataContext = mvIncidencias;
            incidenciasSeleccionadas = new List<Amonestacione>();

            if (persona.IdRolNavigation.NombreRol.Equals("Profesor"))
            {
                itemInforme.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Metodo donde abrimos el dialogo de añadir incidencia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnyadirIncidencia_Click(object sender, RoutedEventArgs e)
        {
            DialogoAnyadirIncidencia dg = new DialogoAnyadirIncidencia(contexto, mvIncidencias.incidencia);
            dg.ShowDialog();
            inicializa();
        }

        /// <summary>
        /// Metodo donde borramos varias incidencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrarIncidencia_Click(object sender, RoutedEventArgs e)
        {
            if(incidenciasSeleccionadas.Count == 0)
            {
                MessageBox.Show("No ha seleccionado ninguna incidencia", "Eliminar una incidencia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if(MessageBox.Show("¿Está seguro de querer eliminar las incidencias?", "Eliminar varias incidencias", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {

                    incidenciasSeleccionadas.ForEach(i =>
                    {
                        mvIncidencias.delete(i);
                    });

                    inicializa();
                    
                }
            }
        }

        

        /// <summary>
        /// Metodo donde aplicamos los filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (mvIncidencias.FechaInicio > mvIncidencias.FechaFin)
            {
                MessageBox.Show("La fecha del hecho no puede ser despues de haberse registrado el mismo hecho. Reviselo", "Error en las fechas", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                mvIncidencias.Filtrar();
            }
        }

        /// <summary>
        /// Metodo donde eliminamos los filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            mvIncidencias.borrarFiltro();
            txtNia.Text = string.Empty;
            cmbTipo.SelectedItem = null;
            cmbMotivo.SelectedItem = null;
            dtpHecho.SelectedDateTime = null;
            dtpRegistro.SelectedDateTime = null;
        }

        /// <summary>
        /// Metodo donde editamos una incidencia/amonestacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemEditar_Click(object sender, RoutedEventArgs e)
        {
            DialogoAnyadirIncidencia dgAnyadirIncidencia = new DialogoAnyadirIncidencia(contexto, (Amonestacione)dgIncidencias.SelectedItem);
            dgAnyadirIncidencia.ShowDialog();

            if((bool)dgAnyadirIncidencia.DialogResult)
            {
                dgIncidencias.Items.Refresh();
            }
        }

        /// <summary>
        /// Metodo donde eliminamos una incidencia/amonestacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemBorrar_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("¿Está seguro de querer eliminar la incidencia?", "Eliminar una incidencia", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                mvIncidencias.delete((Amonestacione)dgIncidencias.SelectedItem);
                inicializa();
            }

        }

        /// <summary>
        /// Metodo donde añadimos a la lista la incidencia/amonestacion seleccionada por el usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            incidenciasSeleccionadas.Add((Amonestacione)dgIncidencias.SelectedItem);
        }

        /// <summary>
        /// Metodo donde eliminamos de la lista la incidencia/amonestacion seleccionada por el usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            incidenciasSeleccionadas.Remove((Amonestacione)dgIncidencias.SelectedItem);
        }

        /// <summary>
        /// Metodo que genera un informe y lo amacena donde el usuario indique
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemInforme_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create);

                Document doc = new Document(PageSize.A4, 25, 25, 25, 25);
                PdfWriter.GetInstance(doc, fs);

                doc.Open();

                // Obtener datos de la amonestacion seleccionado
                Amonestacione amonestacionSeleccionado = (Amonestacione)dgIncidencias.SelectedItem;

                // Configurar estilos
                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA, 18, BaseColor.BLACK);
                Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);

                // Crear tabla principal
                PdfPTable mainTable = new PdfPTable(3);
                mainTable.WidthPercentage = 100;
                mainTable.SpacingBefore = 10f;

                // Agregar imagen
                PdfPCell imgCell = new PdfPCell();
                imgCell.Border = PdfPCell.NO_BORDER;
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance("../../../Recursos/Iconos/gorro.png");
                // Ajusta el tamaño de la imagen según sea necesario
                img.ScaleToFit(100f, 100f); 
                imgCell.AddElement(img);
                mainTable.AddCell(imgCell);

                // Agregar título
                PdfPCell titleCell = new PdfPCell(new Phrase("INFORME DE " + amonestacionSeleccionado.Tipo.ToUpper(), titleFont));
                titleCell.Colspan = 2; 
                titleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                titleCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                titleCell.Border = PdfPCell.NO_BORDER;
                mainTable.AddCell(titleCell);

                // Agregar información de fecha junto con la imagen y el título
                PdfPCell dateCell = new PdfPCell(new Phrase("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:MM:ss"), cellFont));
                dateCell.Colspan = 3; 
                dateCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                dateCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                dateCell.Border = PdfPCell.NO_BORDER;
                mainTable.AddCell(dateCell);

                // Agregar tabla principal al documento
                doc.Add(mainTable);

                // Agregar salto de línea
                doc.Add(Chunk.NEWLINE);

                // Crear texto de información del alumno y amonestaciones
                string textoInforme = "El alumno/a " + amonestacionSeleccionado.IdAlumnoNavigation.Nombre + " "
                    + amonestacionSeleccionado.IdAlumnoNavigation.Apellido1 + " de NIA " + amonestacionSeleccionado.IdAlumnoNavigation.Nia +
                    " del grupo " + amonestacionSeleccionado.IdAlumnoNavigation.IdGrupoNavigation.NombreGrupo + ", ha recibido una " + amonestacionSeleccionado.Tipo
                    + " por " + amonestacionSeleccionado.IdMotivoNavigation.Descripcion + " El día " + amonestacionSeleccionado.FechaHoraHecho.ToString()
                    + " durante el servicio del profesor " + amonestacionSeleccionado.IdProfesorHechoNavigation.IdPersonaNavigation.Nombre + " " + amonestacionSeleccionado.IdProfesorHechoNavigation.IdPersonaNavigation.Apellido1;

                if (amonestacionSeleccionado.Sancion != null)
                {
                    textoInforme += "\n\nCon la siguiente sancion: " + amonestacionSeleccionado.Sancion;
                }

                textoInforme += ".\n\nRegistrado por " + amonestacionSeleccionado.IdProfesorRegistraNavigation.IdPersonaNavigation.Nombre + " " 
                    + amonestacionSeleccionado.IdProfesorRegistraNavigation.IdPersonaNavigation.Apellido1
                    + " el día " + amonestacionSeleccionado.FechaHoraRegistro.ToString() 
                    + "\n\nFirma Director:_______________________\n\nFirma Tutor:_______________________\n\nFirma Padres:_______________________";

                // Agregar texto de informe al documento
                iTextSharp.text.Paragraph informeParagraph = new iTextSharp.text.Paragraph(textoInforme, cellFont);
                doc.Add(informeParagraph);

                // Cerrar el documento y el FileStream
                doc.Close();
                fs.Close();
            }
        }
    }
}
