using iTextSharp.text.pdf;
using iTextSharp.text;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using System.IO;
using System.Windows;

namespace ProyectoRRC.Frontend.Dialogos
{
    /// <summary>
    /// Lógica de interacción para DialogoFecha.xaml
    /// </summary>
    public partial class DialogoFecha : MetroWindow
    {
        /// <summary>
        /// Objeto en el que se almacena el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto en el que se almacena el servicio del grupo
        /// </summary>
        private GrupoServicio grupoServ;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        public DialogoFecha(IncidenciaspartesrrcContext con)
        {
            InitializeComponent();
            this.contexto = con;
            grupoServ = new GrupoServicio(contexto);
        }

        /// <summary>
        /// Metodo que obtiene los grupos y su numero de amonestaciones e incidencias y los motivos mas comunes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSacarInforme_Click(object sender, RoutedEventArgs e)
        {

            //OBTENCIONDE LOS DATOS PARA EL INFORME

            Dictionary<Grupo, int> gruposAmonestaciones = new Dictionary<Grupo, int>();

            Dictionary<Motivo, int> motivos = new Dictionary<Motivo, int>();

            //Recorro todos los grupos
            grupoServ.GetAll.ForEach(grupo =>
            {
                int contadorAmo = 0;
                //Recorro los alumnos de cada grupo
                foreach(Alumno alumno in grupo.Alumnos)
                {
                    bool nuevoAlumno = true;
                    //Recorro cada amonestacion/incidencia de cada alumno
                    foreach (Amonestacione amonestacion in alumno.Amonestaciones)
                    {
                        //Si la amonestacion ha sido en el año que se ha pedido 
                        if(amonestacion.FechaHoraRegistro.Year == fechaAnyo.Value.GetValueOrDefault())
                        {
                            if (nuevoAlumno)
                            {
                                //Sumo el numero de amonestaciones/incidencias de cada alumno
                                contadorAmo += alumno.Amonestaciones.Count;
                                nuevoAlumno = false;
                            }
                            
                            
                            //Comprueba que el motivo este o no en la lista
                            if (motivos.ContainsKey(amonestacion.IdMotivoNavigation))
                            {
                                //Si esta sumo el contador
                                motivos[amonestacion.IdMotivoNavigation]++;
                            }
                            else
                            {
                                //Si no esta lo añado
                                motivos.Add(amonestacion.IdMotivoNavigation, 1);
                            }
                        }


                    }
                    
                }

                //Añado a la lista el grupo y su contador de todas las amonestaciones/incidencias de sus alumnos
                gruposAmonestaciones.Add(grupo, contadorAmo);


            });

            //GENERAR INFORME

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


                PdfPCell titleCell = new PdfPCell(new Phrase("INFORME DEL AÑO "+ fechaAnyo.Value.GetValueOrDefault().ToString(), titleFont));



                // Agregar título
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

                // Crear tabla de incidencias por grupo
                PdfPTable incidentTable = new PdfPTable(2);
                incidentTable.WidthPercentage = 100;
                incidentTable.SpacingBefore = 10f;

                // Configurar estilo de borde y fondo
                incidentTable.DefaultCell.BorderColor = BaseColor.BLACK;
                incidentTable.DefaultCell.BorderWidth = 1f;
                incidentTable.DefaultCell.BackgroundColor = new BaseColor(192, 192, 192); // Gris
                incidentTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                doc.Add(new iTextSharp.text.Paragraph("Numero de amonestaciones e incidencias por grupo", titleFont));

                
                // Agregar título
                PdfPCell titleCell1 = new PdfPCell(new Phrase("Grupo", cellFont));
                titleCell1.HorizontalAlignment = Element.ALIGN_CENTER;
                titleCell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                titleCell1.BackgroundColor = new BaseColor(192, 192, 192); 
                incidentTable.AddCell(titleCell1);

                PdfPCell titleCell2 = new PdfPCell(new Phrase("Numero de Incidencias/amonestaciones", cellFont));
                titleCell2.HorizontalAlignment = Element.ALIGN_CENTER;
                titleCell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                titleCell2.BackgroundColor = new BaseColor(192, 192, 192); 
                incidentTable.AddCell(titleCell2);

                // Agregar fila de datos

                List<int> cantidad = gruposAmonestaciones.Values.ToList();
                List<Grupo> grupos = gruposAmonestaciones.Keys.ToList();

                for(int i = 0; i < gruposAmonestaciones.Count; i++)
                {
                    PdfPCell groupCell = new PdfPCell(new Phrase(grupos[i].NombreGrupo, cellFont));
                    groupCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    groupCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    incidentTable.AddCell(groupCell);

                    PdfPCell countCell = new PdfPCell(new Phrase(cantidad[i].ToString(), cellFont));
                    countCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    countCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    incidentTable.AddCell(countCell);

                    
                }

                doc.Add(incidentTable);


                doc.Add(Chunk.NEWLINE);

                doc.Add(new iTextSharp.text.Paragraph("Motivos mas comunes", titleFont));

                
                // Lista de motivos comunes

                motivos.OrderBy(x => x.Value);

                iTextSharp.text.List list = new iTextSharp.text.List(iTextSharp.text.List.UNORDERED);
                list.SetListSymbol("\u2022"); 

                int cont = 0;

                foreach (Motivo clave in motivos.Keys)
                {
                    
                    if(cont < 5)
                    { 
                        list.Add(new iTextSharp.text.ListItem(clave.Motivo1, cellFont));
                        cont++;
                    }
                    else
                    {
                        break;
                    }

                    
                }

                doc.Add(list);


                // Cerrar el documento y el FileStream
                doc.Close();
                fs.Close();

            }

            DialogResult = true;

        }

        /// <summary>
        /// Metodo que cierra el dialogo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
