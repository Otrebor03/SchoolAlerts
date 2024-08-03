using Microsoft.Win32;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Frontend.Dialogos;
using ProyectoRRC.Frontend.Dialogos.InsertarActualizar;
using ProyectoRRC.Frontend.MVVM;
using System.Windows;
using System.Windows.Controls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace ProyectoRRC.Frontend.UC
{
    /// <summary>
    /// Lógica de interacción para UCAlumnos.xaml
    /// </summary>
    public partial class UCAlumnos : UserControl
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
        /// Objeto persona
        /// </summary>
        private Persona usuario;

        /// <summary>
        /// Lista con los alumnos seleccionados por el usuario
        /// </summary>
        private List<Alumno> alumnoList;


        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        /// <param name="per">Persona que ha iniciado sesion</param>
        public UCAlumnos(IncidenciaspartesrrcContext con, Persona per)
        {
            InitializeComponent();
            this.contexto = con;
            this.usuario = per;
            inicializa();
        }

        /// <summary>
        /// Metodo dodne inicializamos el modelo vista y ocultamos la posibilidad de eliminar y amonestar a los usuarios que sean padres
        /// </summary>
        private void inicializa()
        {
            mvAlumno = new MVAlumno(contexto, usuario);
            this.DataContext = mvAlumno;
            alumnoList = new List<Alumno>();

            if (usuario.IdRolNavigation.NombreRol.Equals("Padre"))
            { 
                itemAmonestar.Visibility = Visibility.Collapsed;
                itemBorrar.Visibility = Visibility.Collapsed;
                itemInforme.Visibility = Visibility.Collapsed;
            }

            if (usuario.IdRolNavigation.NombreRol.Equals("Profesor"))
            {
                itemInforme.Visibility = Visibility.Collapsed;
            }
        }

       
        /// <summary>
        /// Metodo donde abrimos el dialogo para añadir un alumno
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnyadirAlumno_Click(object sender, RoutedEventArgs e)
        {
            DialogoAnyadirAlumno dg = new DialogoAnyadirAlumno(contexto);
            dg.ShowDialog();
            inicializa();
        }

        /// <summary>
        /// Metodo para elimina varios alumnos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrarAlumno_Click(object sender, RoutedEventArgs e)
        {
            //Comprobamos que se haya seleccionado algun alumno
            if(alumnoList.Count == 0)
            {
                MessageBox.Show("No ha seleccionado ningun alumno", "Eliminar alumnos ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (MessageBox.Show("¿Está seguro de querer eliminar los alumnos?", "Eliminar varios alumnos", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //Comprobamos que los alumnos no tengan amonestaciones/incidencias para poder eliminarlo
                    alumnoList.ForEach(a => {

                        if(a.Amonestaciones.Count > 0)
                        {
                            MessageBox.Show("No se ha podido eliminar al alumno debido a que tiene incidencias/amonestaciones, eliminelas", "Error al borrar alumno", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            mvAlumno.delete(a);
                        }

                        
                    });

                    inicializa();

                }
            }
        }

        /// <summary>
        /// Metodo para aplicar los filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            mvAlumno.Filtrar();
        }

        /// <summary>
        /// Metodo para eliminar los filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            mvAlumno.borrarFiltro();
            txtNia.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            cmbGrupo.SelectedItem = null;
        }

        /// <summary>
        /// Metodo para añadir a la lista los alumnos seleccionados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            alumnoList.Add((Alumno)dgAlumnos.SelectedItem);
        }

        /// <summary>
        /// Metodo para eliminar de la lista los alumnos seleccionados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            alumnoList.Remove((Alumno)dgAlumnos.SelectedItem);
        }

        /// <summary>
        /// Metodo para amonestar a un alumno seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemAmonestar_Click(object sender, RoutedEventArgs e)
        {
            DialogoAnyadirIncidencia dg = new DialogoAnyadirIncidencia(contexto, usuario, (Alumno)dgAlumnos.SelectedItem);
            dg.ShowDialog();
        }

        /// <summary>
        /// Metodo para eliminar a un alumno
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de querer eliminar al alumno?", "Eliminar al alumno", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //Comprbamos que el alumno no tenga amonestaciones/incidencias para borrarlo
                if(((Alumno)dgAlumnos.SelectedItem).Amonestaciones.Count > 0)
                {
                    MessageBox.Show("No se ha podido eliminar al alumno debido a que tiene incidencias/amonestaciones, eliminelas", "Error al borrar alumno", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    mvAlumno.delete((Alumno)dgAlumnos.SelectedItem);
                    //dgAlumnos.Items.Refresh(); 
                    //TODO
                    inicializa();
                }
                
            }
        }

        /// <summary>
        /// Metodo que genera un informe y lo amacena donde el usuario indique
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemInforme_Click(object sender, RoutedEventArgs e)
        {
           
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = DateTime.Now.ToString("ddMMyyyyHHmmss")+".pdf";
            if(saveFileDialog.ShowDialog() == true)
            {
                FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create);

                Document doc = new Document(PageSize.A4, 25, 25, 25, 25);
                PdfWriter.GetInstance(doc, fs);

                doc.Open(); /*../../../Recursos/Iconos/gorro.png*/

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
                PdfPCell titleCell = new PdfPCell(new Phrase("INFORME DE ESTUDIANTE", titleFont));
                titleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                titleCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                titleCell.Border = PdfPCell.NO_BORDER;
                mainTable.AddCell(titleCell);

                // Agregar información de fecha
                PdfPCell dateCell = new PdfPCell(new Phrase("Fecha: " + DateTime.Now.ToString(), cellFont));
                dateCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                dateCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                dateCell.Border = PdfPCell.NO_BORDER;
                mainTable.AddCell(dateCell);

                // Agregar tabla principal al documento
                doc.Add(mainTable);

                // Agregar salto de línea
                doc.Add(Chunk.NEWLINE);

                // Crear tabla de información del alumno
                PdfPTable studentInfoTable = new PdfPTable(3); // 3 columnas para Alumno, NIA y Grupo
                studentInfoTable.WidthPercentage = 100;
                studentInfoTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                studentInfoTable.SpacingAfter = 5f; // Espacio después de la tabla

                // Configurar espacio entre columnas y distribución de ancho de columnas
                studentInfoTable.SetWidths(new float[] { 2f, 1f, 2f });  

                // Agregar información del alumno a la tabla
                studentInfoTable.AddCell(new PdfPCell(new Phrase("Alumno: " + ((Alumno)dgAlumnos.SelectedItem).Nombre + " " + ((Alumno)dgAlumnos.SelectedItem).Apellido1, cellFont)) { Border = PdfPCell.NO_BORDER });

                studentInfoTable.AddCell(new PdfPCell(new Phrase("NIA: " + ((Alumno)dgAlumnos.SelectedItem).Nia, cellFont)) { Border = PdfPCell.NO_BORDER });

                studentInfoTable.AddCell(new PdfPCell(new Phrase("Grupo: " + ((Alumno)dgAlumnos.SelectedItem).IdGrupoNavigation.NombreGrupo, cellFont)) { Border = PdfPCell.NO_BORDER });

                // Agregar tabla de información del alumno al documento
                doc.Add(studentInfoTable);

                // Agregar salto de línea entre la información del alumno y la tabla de amonestaciones/incidencias
                doc.Add(Chunk.NEWLINE);

                // Crear tabla de amonestaciones/incidencias
                PdfPTable sanctionsTable = new PdfPTable(4);
                sanctionsTable.WidthPercentage = 100;

                // Encabezado de tabla de amonestaciones/incidencias
                PdfPCell headerCell1 = new PdfPCell(new Phrase("Tipo", cellFont));
                headerCell1.BackgroundColor = BaseColor.LIGHT_GRAY;
                sanctionsTable.AddCell(headerCell1);

                PdfPCell headerCell2 = new PdfPCell(new Phrase("Motivo", cellFont));
                headerCell2.BackgroundColor = BaseColor.LIGHT_GRAY;
                sanctionsTable.AddCell(headerCell2);

                PdfPCell headerCell3 = new PdfPCell(new Phrase("Sanción", cellFont));
                headerCell3.BackgroundColor = BaseColor.LIGHT_GRAY;
                sanctionsTable.AddCell(headerCell3);

                PdfPCell headerCell4 = new PdfPCell(new Phrase("Descripción", cellFont));
                headerCell4.BackgroundColor = BaseColor.LIGHT_GRAY;
                sanctionsTable.AddCell(headerCell4);

                // Datos de amonestaciones/incidencias
                PdfPCell dataCell1;
                PdfPCell dataCell2;
                PdfPCell dataCell3;
                PdfPCell dataCell4;

                studentInfoTable.AddCell(new PdfPCell(new Phrase(((Alumno)dgAlumnos.SelectedItem).Nombre + " " + ((Alumno)dgAlumnos.SelectedItem).Apellido1 + " " + ((Alumno)dgAlumnos.SelectedItem).Apellido2, cellFont)) { Border = PdfPCell.NO_BORDER });

                studentInfoTable.AddCell(new PdfPCell(new Phrase(((Alumno)dgAlumnos.SelectedItem).Nia, cellFont)) { Border = PdfPCell.NO_BORDER });

                studentInfoTable.AddCell(new PdfPCell(new Phrase(((Alumno)dgAlumnos.SelectedItem).IdGrupoNavigation.NombreGrupo, cellFont)) { Border = PdfPCell.NO_BORDER });

                foreach (Amonestacione amonestacion in ((Alumno)dgAlumnos.SelectedItem).Amonestaciones)
                {
                    dataCell1 = new PdfPCell(new Phrase(amonestacion.Tipo, cellFont));
                    dataCell1.HorizontalAlignment = Element.ALIGN_LEFT;
                    sanctionsTable.AddCell(dataCell1);

                    dataCell2 = new PdfPCell(new Phrase(amonestacion.IdMotivoNavigation.Motivo1, cellFont));
                    dataCell2.HorizontalAlignment = Element.ALIGN_LEFT;
                    sanctionsTable.AddCell(dataCell2);

                    dataCell3 = new PdfPCell(new Phrase(amonestacion.Sancion, cellFont));
                    dataCell3.HorizontalAlignment = Element.ALIGN_LEFT;
                    sanctionsTable.AddCell(dataCell3);

                    dataCell4 = new PdfPCell(new Phrase(amonestacion.Descripcion, cellFont));
                    dataCell4.HorizontalAlignment = Element.ALIGN_LEFT;
                    sanctionsTable.AddCell(dataCell4);
                }

                // Agregar tabla de amonestaciones/incidencias al documento
                doc.Add(sanctionsTable);

                // Cerrar el documento y el FileStream
                doc.Close();
                fs.Close();
            }
            

        }

        
    }
}
