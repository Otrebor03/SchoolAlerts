using MahApps.Metro.Controls;
using Microsoft.Win32;
using ProyectoRRC.Backend.Modelo;
using ProyectoRRC.Backend.Servicios;
using ProyectoRRC.Frontend.MVVM;
using ProyectoRRC.Frontend.UC;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;

namespace ProyectoRRC.Frontend.Dialogos.Admin
{
    /// <summary>
    /// Dialogo donde dependiendo del rol que tenga el usuario tendrá acceso a unas funciones u otras, tambien se modifica la interfaz dependiendo del rol
    /// </summary>
    public partial class DialogoInicio : MetroWindow
    {
        /// <value>
        /// Contexto de la base de datos
        /// </value>
        private IncidenciaspartesrrcContext contexto;

        private IncidenciasServicio amonestacionServ;

        private AlumnosServicio alumnosServ;

        private ProfesorServicio profesorServ;

        private PersonaServicio personaServ;

        private GrupoServicio grupoServ;

        private bool informes;

        /// <value>
        /// Variable con el modelo vista de persona
        /// </value>
        private MVPersona mvPersona;

        private MVIncidencias mvAmonestacion;

        /// <summary>
        /// Constructor de la clase, da acceso a unas funcionaldades u otras al usuario dependiendo del rol que este tenga y modifica el dialogo para mostrarlo de la forma deseada
        /// </summary>
        /// <param name="cont">Contexto de la base de datos</param>
        /// <param name="pers">Persona que ha inciado sesion</param>
        public DialogoInicio(IncidenciaspartesrrcContext cont, Persona pers)
        {
            InitializeComponent();
            this.contexto = cont;
            mvPersona = new MVPersona(contexto);
            this.DataContext = mvPersona;
            mvPersona.persona = pers;
            inicializa();
            
        }

        private void inicializa()
        {
            informes = false; ;

            gridCentral.Margin = new Thickness(-75, 0, 0, 0);

            amonestacionServ = new IncidenciasServicio(contexto);
            alumnosServ = new AlumnosServicio(contexto);
            profesorServ = new ProfesorServicio(contexto);
            personaServ = new PersonaServicio(contexto);
            grupoServ = new GrupoServicio(contexto);

            mvAmonestacion = new MVIncidencias(contexto);

            switch (mvPersona.persona.IdRolNavigation.NombreRol)
            {

                case "Profesor":
                    btnConfiguracion.Visibility = Visibility.Hidden;
                    btnUsuarios.Visibility = Visibility.Hidden;
                    btnGraficos.Visibility = Visibility.Hidden;
                    btnInformes.Visibility = Visibility.Hidden;
                    btnEstudiantes.Visibility = Visibility.Hidden;
                    Grid.SetColumn(btnMotivos, 2);
                    Grid.SetRow(btnMotivos, 0);
                    Grid.SetRow(btnIncidencias, 1);
                    Grid.SetRow(btnMotivos, 1);
                    Grid.SetColumn(btnMotivos, 2);
                    inicio.Height = 300;
                    LVIusuarios.Visibility = Visibility.Collapsed;
                    LVIgraficos.Visibility = Visibility.Collapsed;
                    LVIinformes.Visibility = Visibility.Collapsed;
                    LVIconfiguracion.Visibility = Visibility.Collapsed;
                    LVIestudiantes.Visibility = Visibility.Collapsed;
                    break;

                case "Padre":
                    btnUsuarios.Visibility = Visibility.Hidden;
                    btnConfiguracion.Visibility = Visibility.Hidden;
                    btnInformes.Visibility = Visibility.Hidden;
                    btnMotivos.Visibility = Visibility.Hidden;
                    Grid.SetRow(btnGraficos, 1);
                    Grid.SetRow(btnEstudiantes, 1);
                    Grid.SetRow(btnIncidencias, 1);
                    Grid.SetColumn(btnEstudiantes, 2);
                    Grid.SetColumn(btnIncidencias, 1);


                    inicio.Height = 300;
                    LVIusuarios.Visibility = Visibility.Collapsed;
                    LVIinformes.Visibility = Visibility.Collapsed;
                    LVIconfiguracion.Visibility = Visibility.Collapsed;
                    LVImotivos.Visibility = Visibility.Collapsed;
                    break;

                case "Tutor":
                    btnConfiguracion.Visibility = Visibility.Hidden;
                    btnUsuarios.Visibility = Visibility.Hidden;
                    Grid.SetColumn(btnEstudiantes, 2);
                    Grid.SetRow(btnEstudiantes, 0);
                    LVIusuarios.Visibility = Visibility.Collapsed;
                    LVIconfiguracion.Visibility = Visibility.Collapsed;
                    break;

                case "Directivo":
                    btnConfiguracion.Visibility = Visibility.Hidden;
                    btnUsuarios.Visibility = Visibility.Hidden;
                    Grid.SetColumn(btnUsuarios, 2);
                    Grid.SetColumn(btnMotivos, 3);
                    Grid.SetRow(btnEstudiantes, 0);
                    Grid.SetRow(btnMotivos, 0);
                    LVIconfiguracion.Visibility = Visibility.Collapsed;
                    LVIusuarios.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        /// <summary>
        /// Metodo el cual muestra un mensaje emergente y dependiendo de la respuesta del usuario cierra o no la sesion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("¿Desea cerrar la sesión?", "Cerrar Sesión", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DialogoLogin dgLogin = new DialogoLogin();
                dgLogin.Show();
                this.Close(); ;
            }
   
        }

        /// <summary>
        /// Metodo que abre el dialogo donde se muestra la informacion del usuario que ha iniciado sesion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUsuario_Click(object sender, RoutedEventArgs e)
        {
            DialogoUsuario dgUsuario = new DialogoUsuario(contexto, mvPersona.persona);
            dgUsuario.ShowDialog();
        }

        /// <summary>
        /// Metodo que ajusta el dialogo actual añadiendo un hamburguermenu en un lateral e insertando el control de usuario de incidencias
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIncidencias_Click(object sender, RoutedEventArgs e)
        {
            ajustarDialogo("Incidencias", "../../Recursos/Iconos/advertencia.png");

            UCIncidencias ucIncidencias = new UCIncidencias(contexto, mvPersona.persona);
            Grid.SetRowSpan(ucIncidencias, 3);
            Grid.SetColumnSpan(ucIncidencias, 3);
            gridCentral.Children.Add(ucIncidencias);

        }

        /// <summary>
        /// Metodo que ajusta el dialogo actual añadiendo un hamburguermenu en un lateral e insertando el control de usuario de usuarios
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUsuarios_Click(object sender, RoutedEventArgs e)
        {

            UCUsuarios ucUsuarios = new UCUsuarios(contexto, informes, mvPersona.persona);

            if (informes)
            {
                ajustarDialogo("Profesores", "../../Recursos/Iconos/usuarios.png");
                ucUsuarios.spRol.Visibility = Visibility.Hidden;
            }
            else
            {
                ajustarDialogo("Usuarios", "../../Recursos/Iconos/usuarios.png");
                ucUsuarios.spRol.Visibility = Visibility.Visible;
            }
            

            
            Grid.SetRowSpan(ucUsuarios, 3);
            Grid.SetColumnSpan(ucUsuarios, 3);
            
            gridCentral.Children.Add(ucUsuarios);
            informes = false;
        }

        /// <summary>
        /// Metodo que oculta los botones de inicio y muestra los de configuracion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            

            txtUbicacion.Text = "Configuracion";
            img.Source = new BitmapImage(new Uri("../../Recursos/Iconos/ajuste.png", UriKind.RelativeOrAbsolute));

            botonInicio.Visibility = Visibility.Visible;
            btnImport.Visibility = Visibility.Visible;
            btnExportar.Visibility = Visibility.Visible;
            btnRoles.Visibility = Visibility.Visible;
            btnPermisos.Visibility = Visibility.Visible;

            btnIncidencias.Visibility = Visibility.Hidden;
            btnUsuarios.Visibility = Visibility.Hidden;
            btnConfiguracion.Visibility = Visibility.Hidden;
            btnMotivos.Visibility = Visibility.Hidden;
            btnGraficos.Visibility = Visibility.Hidden;
            btnEstudiantes.Visibility = Visibility.Hidden;
            btnInformes.Visibility = Visibility.Hidden;

        }

        /// <summary>
        /// Metodo que ajusta el dialogo actual añadiendo un hamburguermenu en un lateral e insertando el control de usuario de graficos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGraficos_Click(object sender, RoutedEventArgs e)
        {

            if (mvPersona.persona.IdRolNavigation.NombreRol.Equals("Directivo") | mvPersona.persona.IdRolNavigation.NombreRol.Equals("Administrador"))
            {
                

                txtUbicacion.Text = "Gráficos";
                img.Source = new BitmapImage(new Uri("../../Recursos/Iconos/estadisticas.png", UriKind.RelativeOrAbsolute));
                btnEstudiantesGraficos.Visibility = Visibility.Visible;
                btnGrupos.Visibility = Visibility.Visible;
                botonInicio.Visibility = Visibility.Visible;
                Grid.SetColumn(btnInicio, 1);
                Grid.SetRow(btnInicio, 1);

                btnIncidencias.Visibility = Visibility.Hidden;
                btnUsuarios.Visibility = Visibility.Hidden;
                btnConfiguracion.Visibility = Visibility.Hidden;
                btnMotivos.Visibility = Visibility.Hidden;
                btnGraficos.Visibility = Visibility.Hidden;
                btnEstudiantes.Visibility = Visibility.Hidden;
                btnInformes.Visibility = Visibility.Hidden;

                inicio.Height = 300;
            }
            else
            {
                ajustarDialogo("Gráficos", "../../Recursos/Iconos/estadisticas.png");
                btnEstudiantesGraficos_Click(sender, e);
            }

        }

        /// <summary>
        /// Metodo que ajusta el dialogo actual añadiendo un hamburguermenu en un lateral e insertando el control de usuario de estudiantes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEstudiantes_Click(object sender, RoutedEventArgs e)
        {
            ajustarDialogo("Estudiantes", "../../Recursos/Iconos/alumnos.png");
            UCAlumnos ucAlumnos = new UCAlumnos(contexto, mvPersona.persona);
            Grid.SetRowSpan(ucAlumnos, 3);
            Grid.SetColumnSpan(ucAlumnos, 3);
            gridCentral.Children.Add(ucAlumnos);

        }

        /// <summary>
        /// Metodo que ajusta el dialogo actual añadiendo un hamburguermenu en un lateral e insertando el control de usuario de informes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInformes_Click(object sender, RoutedEventArgs e)
        {

            txtUbicacion.Text = "Informes";
            img.Source = new BitmapImage(new Uri("../../Recursos/Iconos/notas.png", UriKind.RelativeOrAbsolute));

            if (mvPersona.persona.IdRolNavigation.NombreRol.Equals("Directivo") | mvPersona.persona.IdRolNavigation.NombreRol.Equals("Administrador"))
            {

                if (mvPersona.persona.IdRolNavigation.NombreRol.Equals("Directivo"))
                {
                    btnEstudiantes.Visibility = Visibility.Visible;
                    Grid.SetRow(btnEstudiantes, 2);
                    btnUsuarios.Visibility = Visibility.Visible;
                    
                }


                btnEstudiantes.Visibility = Visibility.Visible;
                btnIncidencias.Visibility = Visibility.Visible;
                btnUsuario.Visibility = Visibility.Visible;
                btnConfiguracion.Visibility = Visibility.Hidden;
                btnMotivos.Visibility = Visibility.Hidden;
                btnGraficos.Visibility = Visibility.Hidden;
                btnInformes.Visibility = Visibility.Hidden;
                botonInicio.Visibility = Visibility.Visible;
                btnGruposInforme.Visibility = Visibility.Visible;
                btnResumenAnual.Visibility = Visibility.Visible;

                Grid.SetColumn(botonInicio, 1);
                Grid.SetRow(botonInicio, 1);

                Grid.SetColumn(btnUsuarios, 2);

                Grid.SetColumn(btnEstudiantes, 0);

                txtUsuarios.Text = "Profesores";
            }
            else
            {
                btnEstudiantes.Visibility = Visibility.Visible;
                btnGruposInforme.Visibility = Visibility.Visible;
                botonInicio.Visibility = Visibility.Visible;

                btnIncidencias.Visibility = Visibility.Hidden;
                btnUsuarios.Visibility = Visibility.Hidden;
                btnConfiguracion.Visibility = Visibility.Hidden;
                btnMotivos.Visibility= Visibility.Hidden;
                btnGraficos.Visibility= Visibility.Hidden;
                btnInformes.Visibility = Visibility.Hidden;

                Grid.SetColumn(btnEstudiantes, 0);
                Grid.SetRow(btnEstudiantes, 1);

                Grid.SetRow(btnGruposInforme, 1);
                inicio.Height = 300;
            }

            informes = true;
            
        }

        /// <summary>
        ///  Metodo que ajusta el dialogo actual añadiendo un hamburguermenu en un lateral e insertando el control de usuario de motivos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMotivos_Click(object sender, RoutedEventArgs e)
        {
            ajustarDialogo("Motivos", "../../Recursos/Iconos/reporte.png");

            UCMotivos ucMotivos = new UCMotivos(contexto);
            Grid.SetRowSpan(ucMotivos, 3);
            Grid.SetColumnSpan(ucMotivos, 3);
            gridCentral.Children.Add(ucMotivos);

        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btnMostrar.IsChecked = false;
        }

        /// <summary>
        /// Metodo que devuelve al dialogo de inicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            DialogoInicio dgInicio = new DialogoInicio(contexto, mvPersona.persona);
            dgInicio.Show();
            this.Close();
        }

        /// <summary>
        /// Metodo que ajusta el dialogo y comprueba que la zona donde queremos insertar el control de usuario esta vacia y en caso de no estarlo la vacia
        /// </summary>
        /// <param name="ubicacion"></param>
        private void ajustarDialogo(string ubicacion, string rutaImg)
        {
            menu.Visibility = Visibility.Visible;
            gridCentral.Margin = new Thickness(0, 0, 0, 0);
            inicio.WindowState = WindowState.Maximized;
            txtUbicacion.Text = ubicacion;
            img.Source = new BitmapImage(new Uri(rutaImg, UriKind.RelativeOrAbsolute));

            if (gridCentral.Children != null)
            {
                
                gridCentral.Children.Clear();
            }

        }

       private void btnImport_Click(object sender, RoutedEventArgs e)
       {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Archivos XML (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == true)
            {
                // Obtener la ruta del archivo seleccionado
                string filePath = openFileDialog.FileName;

                try
                {
                    XmlDocument xmlDoc = new XmlDocument();

                    xmlDoc.Load(filePath);

                    XmlNodeList nodeAmonestaciones = xmlDoc.SelectNodes("/incidenciaspartesrrc/amonestacion/amonestacion");
            
                    foreach(XmlNode amonestacionNode in nodeAmonestaciones)
                    {
                        Amonestacione amonestacione = new Amonestacione
                        {
                            Descripcion = amonestacionNode.SelectSingleNode("descripcion").InnerText,
                            Sancion = amonestacionNode.SelectSingleNode("sancion").InnerText,
                            Tipo = amonestacionNode.SelectSingleNode("tipo").InnerText,
                            IdMotivo = int.Parse(amonestacionNode.SelectSingleNode("idMotivo").InnerText),
                            FechaHoraHecho = DateTime.Parse(amonestacionNode.SelectSingleNode("fechaHoraHecho").InnerText),
                            FechaHoraRegistro = DateTime.Parse(amonestacionNode.SelectSingleNode("fechaHoraRegistro").InnerText),
                            IdAlumno = int.Parse(amonestacionNode.SelectSingleNode("idAlumno").InnerText),
                            IdProfesorRegistra = int.Parse(amonestacionNode.SelectSingleNode("idProfesorRegistra").InnerText),
                            IdProfesorHecho = int.Parse(amonestacionNode.SelectSingleNode("idProfesorHecho").InnerText)
                        };

                    }
            

                }
                catch (Exception ex)
                {
                    // Manejar errores de lectura
                    MessageBox.Show("Error al leer el archivo: " + ex.Message);
                }
            }
       }

       

        private void btnExportar_Click(object sender, RoutedEventArgs e)  
        {
            List<Amonestacione> amonestacionesExp = amonestacionServ.GetAll;
            List<Alumno> alumnos = ObtenerDatosAlumnos(amonestacionesExp);
            List<Profesor> profesores = ObtenerDatosProfesores(amonestacionesExp);
            List<Persona> personas = ObtenerDatosPersonas(profesores);
            List<Grupo> grupos = ObtenerDatosGrupos(alumnos);

            //Generamos el XML
            XElement xmlData = GenerarXML(amonestacionesExp, alumnos, profesores, personas, grupos);

            // Abrir el cuadro de diálogo de selección de archivo
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "datosExportados";
            saveFileDialog.Filter = "Archivo XML (*.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                // Guardamos el XML en el archivo seleccionado por el usuario
                string xmlFilePath = saveFileDialog.FileName;
                xmlData.Save(xmlFilePath);
            }
        }

        /// <summary>
        /// Metodo que se encarga de recorrer la lista de amonestaciones y la lista de alumnos para obtener aquellos que esten implicados
        /// </summary>
        /// <param name="amonestacionesExp">Lista de amonestaciones e incidencias</param>
        /// <returns>Una lista con los datos de todos los alumnos implicados en amonestaciones o incidencias</returns>
        private List<Alumno> ObtenerDatosAlumnos(List<Amonestacione> amonestacionesExp)
        {
            List<Alumno> estudiantes = new List<Alumno>();

            amonestacionesExp.ForEach(amone =>
            {
                alumnosServ.GetAll.ForEach(a =>
                {
                    if (amone.IdAlumno == a.IdAlumno)
                    {
                        estudiantes.Add(a);
                    }
                });
               
            });

            return estudiantes;

        }

        /// <summary>
        /// Metodo que se encarga de recorrer la lista de amonestaciones y la lista de profesores para obtener aquellos que esten implicados
        /// </summary>
        /// <param name="amonestacionesExp"></param>
        /// <returns>Una lista con los datos de todos los profesores implicados en amonestaciones o incidencias</returns>
        private List<Profesor> ObtenerDatosProfesores(List<Amonestacione> amonestacionesExp)
        {
            List<Profesor> profesors = new List<Profesor>();
            amonestacionesExp.ForEach((a) =>
            {
                profesorServ.GetAll.ForEach(p =>
                {
                    if (a.IdProfesorHecho == p.IdProfesor)
                    {
                        profesors.Add(p);
                    }
                });
                
            });

            return profesors;
        }

        /// <summary>
        /// Metodo que se encarga de recorrer la lista de profesores y la lista de personas para obtener aquellos que sean profesores
        /// </summary>
        /// <param name="profesores">Lista de profesores implicados en incidencias y amonestaciones</param>
        /// <returns>Lista de personas que son profesores</returns>
        private List<Persona> ObtenerDatosPersonas(List<Profesor> profesores)
        {
            List<Persona> personas = new List<Persona>();
            profesores.ForEach((profe) => 
            {
                personaServ.GetAll.ForEach(persona => 
                {
                    if (profe.IdPersona == persona.IdPersona)
                    {
                        personas.Add(persona);
                    }
                });
                
            });

            return personas;
        }

        /// <summary>
        /// Metodo que recorre una lista de grupos y alumnos 
        /// </summary>
        /// <param name="alumnos">Lista de alumnos implicados en amonestaciones e incidencias</param>
        /// <returns>Lista de grupos doned haya un alumno implicado en amonestaciones e incidencias</returns>
        private List<Grupo> ObtenerDatosGrupos(List<Alumno> alumnos)
        {
            List<Grupo> grupo = new List<Grupo>();

            alumnos.ForEach((a) =>
            {
                grupoServ.GetAll.ForEach((g) =>
                {
                    if (a.IdGrupo == g.IdGrupo)
                    {
                        grupo.Add(g);
                    }
                });
            });

            return grupo;
        }

        /// <summary>
        /// Metoodo que genera un XML con los datos introducidos
        /// </summary>
        /// <param name="amonestacionesExp">Lista de amonestaciones</param>
        /// <param name="alumnos">Lista de alumnos involucrados en amonestaciones</param>
        /// <param name="profesores">Lista de profesores involucrados en amonestaciones</param>
        /// <param name="personas">Lista de personas involucradas en amonestaciones</param>
        /// <param name="grupos">Lista de grupos que tienen alumnos involucrados en amonestaciones</param>
        /// <returns>Elemento XML</returns>
        private XElement GenerarXML(List<Amonestacione> amonestacionesExp, List<Alumno> alumnos, List<Profesor> profesores, List<Persona> personas, List<Grupo> grupos)
        {
            //Generar elementos XML para cada tabla

            XElement amonestacion = new XElement("amonestacion");
            foreach(Amonestacione amo in amonestacionesExp)
            {
                amonestacion.Add(new XElement("amonestacion",
                    new XElement("idAmonestacion", amo.IdAmonestacion),
                    new XElement("descripcion", amo.Descripcion),
                    new XElement("fechaHoraHecho", amo.FechaHoraHecho),
                    new XElement("fechaHoraRegistro", amo.FechaHoraRegistro),
                    new XElement("sancion", amo.Sancion),
                    new XElement("tipo", amo.Tipo),
                    new XElement("idMotivo", amo.IdMotivo),
                    new XElement("idAlumno", amo.IdAlumno),
                    new XElement("idProfesorRegistra", amo.IdProfesorRegistra),
                    new XElement("idProfesorHecho", amo.IdProfesorHecho)));
            }

            XElement alumno = new XElement("alumno");
            foreach(Alumno al in alumnos)
            {
                alumno.Add(new XElement("alumno",
                    new XElement("idAlumno", al.IdAlumno),
                    new XElement("nia", al.Nia),
                    new XElement("nombre", al.Nombre),
                    new XElement("apellido1", al.Apellido1),
                    new XElement("apellido2", al.Apellido2),
                    new XElement("telefono", al.Telefono),
                    new XElement("idGrupo", al.IdGrupo),
                    new XElement("foto", al.Foto),
                    new XElement("idPadre", al.IdPadre)));
            }

            XElement profe = new XElement("profesor");
            foreach(Profesor p in profesores)
            {
                profe.Add(new XElement("profesor",
                    new XElement("idProfesor", p.IdProfesor),
                    new XElement("idPersona", p.IdProfesor)));
            }

            XElement persona = new XElement("persona");
            foreach(Persona p in personas) 
            {
                persona.Add(new XElement("persona",
                    new XElement("idPersona", p.IdPersona),
                    new XElement("idRol", p.IdRol),
                    new XElement("dni", p.Dni),
                    new XElement("nombre", p.Nombre),
                    new XElement("apellido1", p.Apellido1),
                    new XElement("apellido2", p.Apellido2),
                    new XElement("contrasenya", p.Contrasenya),
                    new XElement("foto", p.Foto)));
            }

            XElement grupo = new XElement("grupo");
            foreach (Grupo g in grupos)
            {
                grupo.Add(new XElement("grupo",
                    new XElement("idGrupo", g.IdGrupo),
                    new XElement("nombreGrupo", g.NombreGrupo)));
            }

            XElement fichero = new XElement("incidenciaspartesrrc", amonestacion, alumno, profe, persona);

            return fichero;
        }

        /// <summary>
        ///  Metodo que ajusta el dialogo actual añadiendo un hamburguermenu en un lateral e insertando el control de usuario de roles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRoles_Click(object sender, RoutedEventArgs e)
        {
            ajustarDialogo("Configuración - Roles", "../../Recursos/Iconos/roles.png");

            UCRol ucRol = new UCRol(contexto);
            Grid.SetRowSpan(ucRol, 3);
            Grid.SetColumnSpan(ucRol, 3);
            gridCentral.Children.Add(ucRol);
        }

        /// <summary>
        ///  Metodo que ajusta el dialogo actual añadiendo un hamburguermenu en un lateral e insertando el control de usuario de permisos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPermisos_Click(object sender, RoutedEventArgs e)
        {
            ajustarDialogo("Configuración - Permisos", "../../Recursos/Iconos/permiso.png");

            UCPermisos ucPermiso = new UCPermisos(contexto);
            Grid.SetRowSpan(ucPermiso, 3);
            Grid.SetColumnSpan(ucPermiso, 3);
            gridCentral.Children.Add(ucPermiso);
        }

        /// <summary>
        /// Metodo que abre el inicio mostrando los botones de configuracion y cierra el dialogo actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfiguracionMenu_Click(object sender, RoutedEventArgs e)
        {
            DialogoInicio dg = new DialogoInicio(contexto, mvPersona.persona);
            dg.btnConfiguracion_Click(sender, e);
            dg.Show();
            this.Close();
        }

        /// <summary>
        /// Metodo que abre el inicio mostrando los botones de graficos y cierra el dialogo actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGraficosMenu_Click(object sender, RoutedEventArgs e)
        {
            DialogoInicio dg = new DialogoInicio(contexto, mvPersona.persona);
            dg.btnGraficos_Click(sender, e);
            dg.Show();
            this.Close();
        }

        /// <summary>
        /// Metodo que abre el inicio mostrando los botones de informes y cierra el dialogo actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInformesMenu_Click(object sender, RoutedEventArgs e)
        {
            DialogoInicio dg = new DialogoInicio(contexto, mvPersona.persona);
            dg.btnInformes_Click(sender, e);
            dg.Show();
            this.Close();
        }

        /// <summary>
        /// Metodo que abre el grafico de los grupos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGrupos_Click(object sender, RoutedEventArgs e)
        {
            ajustarDialogo("Gráficos Grupos", "../../Recursos/Iconos/grupo.png");
            UCGraficoGrupos uc = new UCGraficoGrupos(contexto, mvPersona.persona);
            Grid.SetRowSpan(uc, 3);
            Grid.SetColumnSpan(uc, 3);
            gridCentral.Children.Add(uc);
        }

        /// <summary>
        /// Metodo que abre el grafico de los estudiantes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEstudiantesGraficos_Click(object sender, RoutedEventArgs e)
        {
            ajustarDialogo("Gráficos Alumnos", "../../Recursos/Iconos/estadisticas.png");
            UCGraficoAlumnos uc = new UCGraficoAlumnos(contexto, mvPersona.persona);
            Grid.SetRowSpan(uc, 3);
            Grid.SetColumnSpan(uc, 3);
            gridCentral.Children.Add(uc);
        }

        /// <summary>
        /// Metodo que abre el dialogo para elegir una fecha para realizar el resumen anual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResumenAnual_Click(object sender, RoutedEventArgs e)
        {
            DialogoFecha dgFecha = new DialogoFecha(contexto);
            dgFecha.ShowDialog();
        }

        /// <summary>
        /// Metodo que abre el dialogo para realizar informes de grupos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGruposInforme_Click(object sender, RoutedEventArgs e)
        {
            ajustarDialogo("Informe Grupos", "../../Recursos/Iconos/reporte.png");
            UCGrupos uc = new UCGrupos(contexto, mvPersona.persona);
            Grid.SetRowSpan(uc, 3);
            Grid.SetColumnSpan(uc, 3);
            gridCentral.Children.Add(uc);
        }
    }
}
