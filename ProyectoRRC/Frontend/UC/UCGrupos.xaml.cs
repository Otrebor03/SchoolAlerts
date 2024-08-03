using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
using ProyectoRRC.Backend.Modelo;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ProyectoRRC.Frontend.MVVM;

namespace ProyectoRRC.Frontend.UC
{
    /// <summary>
    /// Lógica de interacción para UCGrupos.xaml
    /// </summary>
    public partial class UCGrupos : UserControl
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
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        /// <param name="per">Objeto persona que ha iniciado sesion</param>
        public UCGrupos(IncidenciaspartesrrcContext con, Persona per)
        {
            InitializeComponent();
            this.contexto = con;
            mvGrupo = new MVGrupo(contexto, per);
            this.DataContext = mvGrupo;
        }

        /// <summary>
        /// Metodo que genera un informe y lo amacena donde el usuario indique
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemInforme_Click(object sender, RoutedEventArgs e)
        {
            if (dgGrupo.SelectedItem is Grupo)
            {

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
                if (saveFileDialog.ShowDialog() == true)
                {
                    FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create);

                    Document doc = new Document(PageSize.A4, 25, 25, 25, 25);
                    PdfWriter.GetInstance(doc, fs);

                    doc.Open(); 

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
                    PdfPCell titleCell = new PdfPCell(new Phrase("INFORME DE GRUPO", titleFont));
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

                    // Crear tabla de información del Grupo (2 columnas para Nombre y Numero de alumnos )
                    PdfPTable studentInfoTable = new PdfPTable(2); 
                    // Configurar espacio entre columnas y distribución de ancho de columnas
                    studentInfoTable.SetWidths(new float[] { 2f, 2f }); 

                    studentInfoTable.WidthPercentage = 100;
                    studentInfoTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    studentInfoTable.SpacingAfter = 5f; 

                    Grupo grupo = (Grupo)dgGrupo.SelectedItem;

                    // Agregar información del alumno a la tabla
                    studentInfoTable.AddCell(new PdfPCell(new Phrase("Grupo: " + grupo.NombreGrupo, cellFont)) { Border = PdfPCell.NO_BORDER });

                    studentInfoTable.AddCell(new PdfPCell(new Phrase("Número de alumnos: " + grupo.Alumnos.Count.ToString(), cellFont)) { Border = PdfPCell.NO_BORDER });
                    doc.Add(studentInfoTable);

                    doc.Add(Chunk.NEWLINE);

                    // Crear tabla de incidencias con bordes
                    PdfPTable incidentTable = new PdfPTable(2);
                    incidentTable.WidthPercentage = 100;
                    incidentTable.SpacingBefore = 10f;

                    // Configurar estilo de borde de celdas
                    incidentTable.DefaultCell.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;

                    // Agregar celdas para número de incidencias y valor
                    PdfPCell cell1 = new PdfPCell(new Phrase("Numero de incidencias", cellFont));
                    cell1.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    incidentTable.AddCell(cell1);

                    //Contadores de incidencias y amonestaciones
                    int numInciden = 0;
                    int numAmones = 0;
                    
                    //Comprobamos las incidencias y amonestaciones de cada alumno
                    foreach(Alumno alumno in grupo.Alumnos)
                    {
                        if(alumno.Amonestaciones.Count > 0)
                        {
                            foreach (Amonestacione amonestacion in alumno.Amonestaciones)
                            {
                                if (amonestacion.Tipo.Equals("Incidencia"))
                                {
                                    numInciden++;
                                }
                                else
                                {
                                    numAmones++;
                                }
                            }
                        }
                        
                    }

                    PdfPCell cell2 = new PdfPCell(new Phrase(numInciden.ToString(), cellFont));
                    cell2.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    incidentTable.AddCell(cell2);

                    // Agregar celdas para número de amonestaciones y valor
                    PdfPCell cell3 = new PdfPCell(new Phrase("Numero de Amonestaciones", cellFont));
                    cell3.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    incidentTable.AddCell(cell3);

                    PdfPCell cell4 = new PdfPCell(new Phrase(numAmones.ToString(), cellFont));
                    cell4.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    incidentTable.AddCell(cell4);

                    // Agregar celdas para total y valor
                    PdfPCell cell5 = new PdfPCell(new Phrase("Total", cellFont));
                    cell5.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    incidentTable.AddCell(cell5);

                    PdfPCell cell6 = new PdfPCell(new Phrase((numAmones+numInciden).ToString(), cellFont));
                    cell6.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER;
                    incidentTable.AddCell(cell6);

                    // Agregar tabla de incidencias al documento
                    doc.Add(incidentTable);


                    // Cerrar el documento y el FileStream
                    doc.Close();
                    fs.Close();







                }
            }
        }
    }
}
