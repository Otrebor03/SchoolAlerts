using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using ProyectoRRC.Frontend.Dialogos;
using ProyectoRRC.Frontend.Dialogos.InsertarActualizar;
using ProyectoRRC.Frontend.MVVM;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoRRC.Frontend.UC
{
    /// <summary>
    /// Lógica de interacción para UCUsuarios.xaml
    /// </summary>
    public partial class UCUsuarios : UserControl
    {
        /// <summary>
        /// Objeto con el contexto de la base de datos
        /// </summary>
        private IncidenciaspartesrrcContext contexto;

        /// <summary>
        /// Objeto con el usuario que ha iniciado sesion
        /// </summary>
        private Persona usuario;

        /// <summary>
        /// Objeto con el modelo vista de persona
        /// </summary>
        private MVPersona mvPersona;

        /// <summary>
        /// Objeto con el servicio de profesor
        /// </summary>
        private ProfesorServicio profesorServ;

        /// <summary>
        /// Objeto con el servicio de padre
        /// </summary>
        private PadreServicio padreServ;

        private bool informe;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="con">Contexto de la base de datos</param>
        public UCUsuarios(IncidenciaspartesrrcContext con, bool info, Persona per)
        {
            InitializeComponent();
            this.contexto = con;
            this.informe = info;
            this.usuario = per;
            inicializa(informe);
        }

        /// <summary>
        /// Metodo donde inicializamos el modelo vista y el servicio
        /// </summary>
        private void inicializa(bool informe)
        {
            mvPersona = new MVPersona(contexto, informe);
            this.DataContext = mvPersona;
            profesorServ = new ProfesorServicio(contexto);
            padreServ = new PadreServicio(contexto);

        }

        /// <summary>
        /// Metod que abre un dialogo para añadir un usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnyadirUsuario_Click(object sender, RoutedEventArgs e)
        {
            DialogoAnyadirUsuario dg = new DialogoAnyadirUsuario(contexto);
            dg.ShowDialog();

            if ((bool)dg.DialogResult)
            {
                inicializa(informe);
            }
            
        }

        /// <summary>
        /// Metodo que comprueba si el usuario seleccionado es un profesor, tutor o padre. En caso de si serlo comprueba que no tenga alumnos a su cargo y lo elimina
        /// </summary>
        /// <param name="persona">Persona seleccionada</param>
        /// <returns>True si el profesor/tutor ha sido eliminado, False en caso de no ser eliminado</returns>
        private bool borrarProfesor(Persona persona)
        {
            bool borrado = true;
            //Comprobamos el rol de la persoan
            if (persona.IdRolNavigation.NombreRol.Equals("Profesor") | persona.IdRolNavigation.NombreRol.Equals("Tutor"))
            {
                //Comprobamos si el profesor tiene alguna incidencia o amonestacion y si no tiene lo elimina
                profesorServ.GetAll.ForEach(profesor =>
                {
                    if(profesor.IdPersona == ((Persona)dgUsuarios.SelectedItem).IdPersona)
                    {
                        
                        if(profesor.AmonestacioneIdProfesorHechoNavigations.Count > 0 | profesor.AmonestacioneIdProfesorRegistraNavigations.Count > 0) 
                        {
                            MessageBox.Show("Existen incidencias o amonestaciones donde este usuario esta registrado, porfavor editelas o eliminelas antes de eliminar al usuario", "No se puede eliminar este usuario", MessageBoxButton.OK, MessageBoxImage.Warning);
                            borrado = false; 
                        }
                        else
                        {
                            profesor.IdGrupos.Clear();
                            profesorServ.Delete(profesor);
                        }
                    }
                });


            }else if (persona.IdRolNavigation.NombreRol.Equals("Padre"))
            {
                //Comprueba que el pare no tenga alumnos (hijos) y despues lo elimina
                padreServ.GetAll.ForEach(padre =>
                {

                    if(padre.IdPersona == persona.IdPersona)
                    {
                        if (padre.Alumnos.Count > 0)
                        {
                            MessageBox.Show("No se pudo eliminar al siguiente usuario debido a que tiene alumnos asignados, porfavor edite la informacion de los alumnos", "No se pudo eliminar al usuario", MessageBoxButton.OK, MessageBoxImage.Error);
                            borrado = false; ;
                        }
                        else
                        {
                            padre.Alumnos.Clear();
                            padreServ.Delete(padre);
                        }
                    }
                    
                });

                
            }
            

            return borrado;
            
        }

        /// <summary>
        /// Metodo que abre un dialogo para editar la contraseña del usuario seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemEditar_Click(object sender, RoutedEventArgs e)
        {
            DialogoContrasenya dg = new DialogoContrasenya(contexto, (Persona)dgUsuarios.SelectedItem);
            dg.ShowDialog();
        }

        /// <summary>
        /// Metodo que comprueba si el usuario seleccionado es un profesor/tutor y se puede eliminar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro de querer eliminar al usuario?", "Eliminar un usuario", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {

                if (((Persona)dgUsuarios.SelectedItem).Dni.Equals(usuario.Dni))
                {
                    MessageBox.Show("No puede borrar su propio usuario", "Elimina un usuario", MessageBoxButton.OK, MessageBoxImage.Warning);
                }else if (borrarProfesor((Persona)dgUsuarios.SelectedItem))
                {
                    mvPersona.delete((Persona)dgUsuarios.SelectedItem);
                }

                
                inicializa(informe);
            }
        }

        /// <summary>
        /// Metodo que elimina los filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            mvPersona.borrarFiltro();
            txtNombre.Text = string.Empty;
            cmbRol.SelectedItem = null;
            txtDni.Text = string.Empty;
        }

        /// <summary>
        /// Metodo que añade los filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            mvPersona.Filtrar();
        }

        /// <summary>
        /// Metodo que genera un informe y lo amacena donde el usuario indique
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemInforme_Click(object sender, RoutedEventArgs e)
        {

            if((Persona)dgUsuarios.SelectedItem is Persona && (((Persona)dgUsuarios.SelectedItem).IdRolNavigation.NombreRol.Equals("Tutor") | ((Persona)dgUsuarios.SelectedItem).IdRolNavigation.NombreRol.Equals("Profesor")))
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

                    Persona persona = (Persona)dgUsuarios.SelectedItem;
                    PdfPCell titleCell;

                    if (persona.IdRolNavigation.NombreRol.Equals("Tutor"))
                    {
                        titleCell = new PdfPCell(new Phrase("INFORME DE TUTOR", titleFont));
                    }
                    else
                    {
                        titleCell = new PdfPCell(new Phrase("INFORME DE PROFESOR", titleFont));
                    }

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

                    // Crear tabla de información del Profesor
                    PdfPTable profesorInfoTable = new PdfPTable(2); // 2 columnas para Nombre y DNI 


                    if (persona.IdRolNavigation.NombreRol.Equals("Tutor"))
                    {
                        profesorInfoTable = new PdfPTable(3); 
                        profesorInfoTable.SetWidths(new float[] { 2f, 1f, 2f });
                    }
                    else
                    {
                        profesorInfoTable.SetWidths(new float[] { 2f, 2f }); 
                    }
                     
                    profesorInfoTable.WidthPercentage = 100;
                    profesorInfoTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    profesorInfoTable.SpacingAfter = 5f; 

                    

                    // Agregar información del profesor a la tabla
                    profesorInfoTable.AddCell(new PdfPCell(new Phrase("Profesor: " + persona.Nombre+" "+persona.Apellido1, cellFont)) { Border = PdfPCell.NO_BORDER });

                    profesorInfoTable.AddCell(new PdfPCell(new Phrase("DNI: " + persona.Dni, cellFont)) { Border = PdfPCell.NO_BORDER });

                    if (persona.IdRolNavigation.NombreRol.Equals("Tutor"))
                    {
                        List<string> listaGrupos = new List<string>();

                        profesorInfoTable.AddCell(new PdfPCell(new Phrase("Grupo: " + persona.Profesors.First().IdGrupos.First().NombreGrupo, cellFont)) { Border = PdfPCell.NO_BORDER });

                    }

                    // Agregar tabla de información del profesor al documento
                    doc.Add(profesorInfoTable);

                    // Agregar salto de línea entre la información del profesor y la tabla de amonestaciones/incidencias
                    doc.Add(Chunk.NEWLINE);

                    // Crear tabla de amonestaciones/incidencias que ha estado presente
                    PdfPTable presentSanctionsTable = new PdfPTable(4);
                    presentSanctionsTable.WidthPercentage = 100;

                    // Encabezado de la tabla de amonestaciones/incidencias que ha estado presente
                    PdfPCell presentHeaderCell1 = new PdfPCell(new Phrase("Tipo", cellFont));
                    presentHeaderCell1.BackgroundColor = BaseColor.LIGHT_GRAY;
                    presentSanctionsTable.AddCell(presentHeaderCell1);

                    PdfPCell presentHeaderCell2 = new PdfPCell(new Phrase("Motivo", cellFont));
                    presentHeaderCell2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    presentSanctionsTable.AddCell(presentHeaderCell2);

                    PdfPCell presentHeaderCell3 = new PdfPCell(new Phrase("Sanción", cellFont));
                    presentHeaderCell3.BackgroundColor = BaseColor.LIGHT_GRAY;
                    presentSanctionsTable.AddCell(presentHeaderCell3);

                    PdfPCell presentHeaderCell4 = new PdfPCell(new Phrase("Descripción", cellFont));
                    presentHeaderCell4.BackgroundColor = BaseColor.LIGHT_GRAY;
                    presentSanctionsTable.AddCell(presentHeaderCell4);


                    PdfPCell dataCell1;
                    PdfPCell dataCell2;
                    PdfPCell dataCell3;
                    PdfPCell dataCell4;

                    foreach(Amonestacione amonestacion in persona.Profesors.First().AmonestacioneIdProfesorHechoNavigations)
                    {
                        dataCell1 = new PdfPCell(new Phrase(amonestacion.Tipo, cellFont));
                        dataCell1.HorizontalAlignment = Element.ALIGN_LEFT;
                        presentSanctionsTable.AddCell(dataCell1);

                        dataCell2 = new PdfPCell(new Phrase(amonestacion.IdMotivoNavigation.Motivo1, cellFont));
                        dataCell2.HorizontalAlignment = Element.ALIGN_LEFT;
                        presentSanctionsTable.AddCell(dataCell2);

                        dataCell3 = new PdfPCell(new Phrase(amonestacion.Sancion, cellFont));
                        dataCell3.HorizontalAlignment = Element.ALIGN_LEFT;
                        presentSanctionsTable.AddCell(dataCell3);

                        dataCell4 = new PdfPCell(new Phrase(amonestacion.Descripcion, cellFont));
                        dataCell4.HorizontalAlignment = Element.ALIGN_LEFT;
                        presentSanctionsTable.AddCell(dataCell4);
                    }

                   

                    // Agregar tabla de amonestaciones/incidencias que ha estado presente al documento
                    doc.Add(new iTextSharp.text.Paragraph("Incidencias/Amonestaciones que ha estado presente\n", titleFont));
                    doc.Add(Chunk.NEWLINE);
                    doc.Add(presentSanctionsTable);

                    
                    doc.Add(Chunk.NEWLINE);

                    // Crear tabla de amonestaciones/incidencias que ha registrado
                    PdfPTable registeredSanctionsTable = new PdfPTable(4);
                    registeredSanctionsTable.WidthPercentage = 100;

                    // Encabezado de la tabla de amonestaciones/incidencias que ha registrado
                    PdfPCell registeredHeaderCell1 = new PdfPCell(new Phrase("Tipo", cellFont));
                    registeredHeaderCell1.BackgroundColor = BaseColor.LIGHT_GRAY;
                    registeredSanctionsTable.AddCell(registeredHeaderCell1);

                    PdfPCell registeredHeaderCell2 = new PdfPCell(new Phrase("Motivo", cellFont));
                    registeredHeaderCell2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    registeredSanctionsTable.AddCell(registeredHeaderCell2);

                    PdfPCell registeredHeaderCell3 = new PdfPCell(new Phrase("Sanción", cellFont));
                    registeredHeaderCell3.BackgroundColor = BaseColor.LIGHT_GRAY;
                    registeredSanctionsTable.AddCell(registeredHeaderCell3);

                    PdfPCell registeredHeaderCell4 = new PdfPCell(new Phrase("Descripción", cellFont));
                    registeredHeaderCell4.BackgroundColor = BaseColor.LIGHT_GRAY;
                    registeredSanctionsTable.AddCell(registeredHeaderCell4);

                    PdfPCell dataCell5;
                    PdfPCell dataCell6;
                    PdfPCell dataCell7;
                    PdfPCell dataCell8;

                    foreach (Amonestacione amonestacion in persona.Profesors.First().AmonestacioneIdProfesorRegistraNavigations)
                    {
                        dataCell5 = new PdfPCell(new Phrase(amonestacion.Tipo, cellFont));
                        dataCell5.HorizontalAlignment = Element.ALIGN_LEFT;
                        registeredSanctionsTable.AddCell(dataCell5);

                        dataCell6 = new PdfPCell(new Phrase(amonestacion.IdMotivoNavigation.Motivo1, cellFont));
                        dataCell6.HorizontalAlignment = Element.ALIGN_LEFT;
                        registeredSanctionsTable.AddCell(dataCell6);

                        dataCell7 = new PdfPCell(new Phrase(amonestacion.Sancion, cellFont));
                        dataCell7.HorizontalAlignment = Element.ALIGN_LEFT;
                        registeredSanctionsTable.AddCell(dataCell7);

                        dataCell8 = new PdfPCell(new Phrase(amonestacion.Descripcion, cellFont));
                        dataCell8.HorizontalAlignment = Element.ALIGN_LEFT;
                        registeredSanctionsTable.AddCell(dataCell8);
                    }

                    // Agregar tabla de amonestaciones/incidencias que ha registrado al documento
                    doc.Add(new iTextSharp.text.Paragraph("Incidencias/Amonestaciones que ha registrado\n", titleFont));
                    doc.Add(Chunk.NEWLINE);
                    doc.Add(registeredSanctionsTable);

                    // Cerrar el documento y el FileStream
                    doc.Close();
                    fs.Close();
                }






            }
            else
            {
                MessageBox.Show("Para crear un informe debe seleccionar un tutor o un profesor", "Error al crear un informe", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        
    }
}
