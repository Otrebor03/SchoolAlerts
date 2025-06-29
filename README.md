# SchoolAlerts

Documentación en pdf: [SchoolAlerts_Documentacion.pdf](https://github.com/user-attachments/files/20959602/SchoolAlerts_Documentacion.pdf)

## Índice
1. [Descripción del problema](#descripcion-del-problema)  
2. [Solución proporcionada](#solucion-proporcionada)  
   2.1 [Introducción](#introduccion)  
   2.2 [Herramientas utilizadas](#herramientas-utilizadas)  
   2.3 [Base de datos](#base-de-datos)  
       2.3.1 [Tablas](#tablas)  
       2.3.2 [Relaciones entre tablas](#relaciones-entre-tablas)  
   2.4 [Estructura del proyecto](#estructura-del-proyecto)  
       2.4.1 [Organización de la aplicación](#organizacion-de-la-aplicacion)  
       2.4.2 [Explicación de las clases](#explicacion-de-las-clases)  
       2.4.3 [Interfaz gráfica](#interfaz-grafica)  
   2.5 [Utilización del programa](#utilizacion-del-programa)

## 1. Descripción del problema

En los centros educativos, cuando se quiere registrar un parte de amonestación o una incidencia, estos datos suelen almacenarse en sistemas distintos y sin conexión entre ellos. Esta dispersión complica el acceso centralizado a la información por parte del equipo directivo del centro, dificultando la toma de decisiones y la aplicación de medidas organizativas o pedagógicas de forma ágil y eficaz.

---

## 2. Solución proporcionada

### 2.1 Introducción

La solución propuesta consiste en una aplicación unificada que permite registrar tanto incidencias como amonestaciones dentro de un mismo sistema centralizado. Dependiendo del rol asignado al usuario, se le otorgará acceso a determinadas funciones del sistema, manteniendo así un control de permisos adecuado.

El sistema contempla 5 tipos de roles, cada uno con sus funciones específicas:

- **Administrador**: acceso total. Puede registrar incidencias, importar y exportar datos, gestionar informes, modificar usuarios, roles y permisos.
- **Directivo**: puede añadir y editar incidencias, generar informes y visualizar gráficos para análisis.
- **Tutor**: puede añadir y editar incidencias de su grupo o en las que esté implicado, generar informes y visualizar gráficos únicamente de sus grupos asignados.
- **Profesor**: puede registrar incidencias y editarlas si está directamente implicado.
- **Padre/madre**: puede visualizar gráficos de las incidencias que afectan a sus hijos.

Esta jerarquía de roles permite una gestión ordenada, respetando tanto la privacidad de los datos como la funcionalidad requerida por cada perfil del entorno educativo.

---

### 2.2 Herramientas utilizadas

A lo largo del desarrollo del proyecto se han utilizado diversas herramientas para cubrir tanto el diseño como el desarrollo técnico:

- 🛠️ **MySQL Workbench**: diseño y creación de la base de datos.
- 🖼️ **Flaticon**: obtención de imágenes e iconos para la interfaz.
- 🧩 **Justinmind**: prototipado y esquematización previa de la interfaz gráfica.
- 🎨 **Coolors**: generación de paletas de colores coherentes.
- 💻 **Visual Studio**: entorno de desarrollo para la codificación del sistema en .NET.
- 🤖 **ChatGPT**: generación de datos de prueba para la base de datos y consulta de dudas puntuales.
- 📺 **YouTube**: apoyo con tutoriales sobre generación de informes, gráficos y encriptación de claves.
- 🗂️ **Creately**: creación de esquemas de la base de datos y estructura del proyecto.

---

## 2.3 Base de datos

El sistema cuenta con una base de datos relacional que centraliza la información relacionada con usuarios, roles, grupos, incidencias y permisos.

### 🖼️ Modelo de la base de datos
![bd](https://github.com/user-attachments/assets/3518b2c1-c580-4810-a81b-083e0e1e2151)

### 2.3.1 Tablas

#### 📄 Amonestaciones  
Tabla encargada de almacenar las amonestaciones e incidencias.  
**Campos:**
- `idAmonestacion`
- `descripcion`
- `fechaHoraHecho`
- `sancion`
- `tipo`
- `fechaHoraRegistro`
- `idMotivo`
- `idAlumno`
- `idProfesorRegistra`
- `idProfesorHecho`

---

#### 🎯 Motivos  
Contiene los diferentes motivos definidos por el usuario.  
**Campos:**
- `idMotivo`
- `motivo`
- `descripcion`

---

#### 👨‍🏫 Profesor  
Registra los profesores que pueden participar en amonestaciones o tutorías.  
**Campos:**
- `idProfesor`
- `idPersona`

---

#### 🧩 GrupoProfesor  
Relaciona a los profesores con los grupos.  
**Campos:**
- `idGrupo`
- `idProfesor`

---

#### 👥 Grupo  
Contiene los grupos académicos existentes.  
**Campos:**
- `idGrupo`
- `nombreGrupo`

---

#### 🎓 Alumno  
Almacena los datos de los alumnos que pueden recibir amonestaciones.  
**Campos:**
- `idAlumno`
- `nia`
- `nombre`, `apellido1`, `apellido2`
- `telefono`
- `foto`
- `idGrupo`
- `idPadre`

---

#### 👨‍👧 Padre  
Representa a los tutores legales de los alumnos.  
**Campos:**
- `idPadre`
- `idPersona`

---

#### 🧑 Persona  
Contiene los datos de todos los usuarios del sistema.  
**Campos:**
- `idPersona`
- `idRol`
- `dni`
- `nombre`, `apellido1`, `apellido2`
- `contrasenya`
- `foto`

---

#### 🛡️ Roles  
Define los distintos roles existentes en la aplicación.  
**Campos:**
- `idRol`
- `nombreRol`

---

#### 🔗 PermisosRoles  
Relaciona los roles con los permisos que tienen asignados.  
**Campos:**
- `idPermiso`
- `idRol`

---

#### ⚙️ Permisos  
Define las acciones permitidas en el sistema.  
**Campos:**
- `idPermiso`
- `descripcion`

---

### 2.3.2 Relaciones entre tablas

- **Amonestaciones ↔ Motivos**: relación 1:N → cada amonestación tiene un motivo; un motivo puede estar en varias amonestaciones.
- **Amonestaciones ↔ Alumno**: relación 1:N → un alumno puede tener varias amonestaciones.
- **Amonestaciones ↔ Profesor**: relación doble 1:N → un profesor puede registrar y también estar presente en varias amonestaciones.
- **Profesor ↔ Persona**: 1:N → cada profesor es una persona.
- **Profesor ↔ Grupo**: M:N → un profesor puede pertenecer a varios grupos y viceversa.
- **Alumno ↔ Grupo**: 1:N → un alumno pertenece a un grupo, y un grupo puede tener varios alumnos.
- **Alumno ↔ Padre**: 1:N → un padre puede tener varios hijos registrados.
- **Padre ↔ Persona**: 1:N → cada padre es una persona.
- **Persona ↔ Roles**: 1:N → una persona tiene un rol asignado, y un rol puede estar asociado a varias personas.
- **Roles ↔ Permisos**: M:N → un rol puede tener varios permisos, y un permiso puede ser compartido entre varios roles.

### 2.4. Estructura del proyecto

![estructuraCarpetas](https://github.com/user-attachments/assets/960d960d-0be3-4d80-bc0a-b8f0f2b0cfe1)

El programa está organizado en tres carpetas principales:

- `backend`
- `frontend`
- `recursos`

#### 📁 Backend

Contiene los componentes esenciales para la comunicación con la base de datos y la lógica de negocio.

- **`Modelos/`**: representa las estructuras de datos que reflejan las tablas de la base de datos.
- **`Servicios/`**: incluye la lógica de negocio e intermediarios entre la interfaz y los modelos.

#### 📁 Frontend

Aquí se encuentra todo lo relacionado con la interfaz gráfica y la capa de presentación (MVVM).

- **`Dialogos/`**: contiene todos los cuadros de diálogo de la aplicación.
  - **`InsertarActualizar/`**: diálogos específicos para inserción y edición de datos.
  
- **`MVVM/`**: almacena los modelos vista encargados de actuar entre la vista y los modelos.
  
- **`UserControl/`**: contiene los distintos controles personalizados de usuario.
  - **`Graficos/`**: subcarpeta con controles que implementan gráficos.

#### 📁 Recursos

Contiene los recursos gráficos del proyecto.

- **`Iconos/`**: carpeta con todas las imágenes e iconos utilizados.

---

### 📦 Paquetes NuGet utilizados

- `Extended.Wpf.Toolkit`: controles adicionales para WPF.
- `iTextSharp`: creación y manipulación de documentos PDF.
- `LiveCharts.Wpf`: gráficos interactivos en WPF.
- `MaterialDesignThemes.MahApps`: estilos y temas para interfaz.
- `Microsoft.EntityFrameworkCore`: ORM para acceder a bases de datos.
- `Microsoft.EntityFrameworkCore.Design`: herramientas de diseño EF Core.
- `Microsoft.EntityFrameworkCore.Proxies`: soporte para lazy loading.
- `NLog`: sistema de logging.
- `Pomelo.EntityFrameworkCore.MySql`: acceso a bases de datos MySQL.

---

### 2.4.2 Explicación de las clases

#### 🧱 Modelos (carpeta `Modelos/`)

- `Alumno`: representa a los alumnos.
- `Amonestacion`: representa las incidencias y amonestaciones.
- `Grupo`: grupos académicos.
- `Motivo`: motivos de las incidencias.
- `Padre`: padres o tutores legales.
- `Permiso`: acciones autorizadas del sistema.
- `Persona`: usuarios que acceden al sistema.
- `Profesor`: docentes y tutores.
- `Role`: roles del sistema.
- `IncidenciaspartesrrcContext`: contexto principal de la base de datos y acceso CRUD.

#### 🔧 Servicios (carpeta `Servicios/`)

- `AlumnoServicio`: obtención de NIAs y estadísticas por alumno y año.
- `GrupoServicio`: estadísticas de grupo por año.
- `IncidenciasServicio`: obtención de fechas de la última incidencia.
- `MotivoServicio`, `PadreServicio`, `PermisoServicio`, `ProfesorServicio`: lógica de negocio asociada.
- `PersonaServicio`: verificación de credenciales y consulta de DNI.
- `RolServicio`: obtención de roles disponibles.
- `ServicioGenerico`: implementación base para acceso a datos.
- `IServicioGenerico`: interfaz de operaciones básicas CRUD.

---

### 2.4.3 Interfaz gráfica

#### 🪟 Diálogos

- `DialogoBienvenida`: conecta con la base de datos y abre la siguiente ventana.
- `DialogoLogin`: validación de credenciales.
- `DialogoInicio`: carga la interfaz según el rol y permite exportar los datos.
- `DialogoUsuario`: muestra datos del usuario y permite cambiar la contraseña.
- `DialogoFecha`: genera informes por fecha seleccionada.
- `DialogoAnyadirAlumno`: añade o edita alumnos.
- `DialogoAnyadirIncidencia`: añade incidencias o amonestaciones.
- `DialogoMotivoIncidencia`: añade nuevos motivos.
- `DialogoAnyadirPermiso`: crea nuevos permisos.
- `DialogoAnyadirUsuario`: añade usuarios según su rol.
- `DialogoContrasenya`: cambia la contraseña de un usuario.
- `DialogoCrearGrupo`: añade nuevos grupos.
- `DialogoCrearRol`: permite la creación de roles.
- `DialogoModificarContrasenya`: requiere contraseña antigua para cambiarla.

#### 🧩 Controles de usuario (`UserControl/`)

- `UCAlumnos`: gestión, filtrado, amonestación y generación de informes de alumnos.
- `UCGrupos`: visualización e informes de grupos.
- `UCIncidencias`: gestión, filtrado y generación de informes de incidencias.
- `UCMotivos`: gestión de motivos.
- `UCPermisos`: gestión de permisos.
- `UCUsuarios`: gestión de usuarios y generación de informes para docentes.
- `UCGraficoAlumnos`: generación de gráficos por alumno y año.
- `UCRol`: gestión de roles.
- `UCGraficoGrupos`: generación de gráficos por grupo y año.

---

### 🧠 Modelos Vista (`MVVM/`)

- `MVAlumno`: gestiona y filtra la información de alumnos, inicializa servicios y gráficos.
- `MVGrupo`: maneja la información y gráficos relacionados con los grupos.
- `MVIncidencia`: gestiona acceso, servicios y filtrado de incidencias.
- `MVMotivo`: gestiona la información de motivos.
- `MVPermiso`: maneja la lógica de permisos.
- `MVPersona`: acceso y filtrado de usuarios.
- `MVProfesor`: gestiona la información de profesores.
- `MVRol`: maneja la lógica de roles y permisos asignados.
- `MVBase`: lógica de validación para formularios.
- `MVBaseCRUD`: métodos para operaciones CRUD reutilizables.
- `PropertyChangedDataError`: implementación de validación de propiedades y errores.

### 2.4.3 Interfaz gráfica

La interfaz del programa ha sido desarrollada utilizando **WPF (Windows Presentation Foundation)**, junto con las librerías **MahApps.Metro** y **Material Design**, lo que proporciona una estética moderna, limpia y funcional.

#### 🎨 Características generales
- **Diseño moderno** con barra de título personalizada, incluyendo:
  - Icono del programa.
  - Nombre de la aplicación.
  - Botón de cerrar sesión en la mayoría de las ventanas.
- **Ventanas modales**, centradas en pantalla para mantener la atención del usuario.
- **Botones con efecto de sombra**, para destacarlos visualmente.
- **Diseño adaptable según función**, con estructuras basadas en:
  - `Grid`: para ventanas de inserción y formularios.
  - `DockPanel` y `StackPanel`: para organización flexible en ventanas de navegación y visualización.
- **Elementos de navegación**:
  - Menú hamburguesa (desplegable/plegable).
  - Indicador de ubicación del usuario.
  - Información del usuario y su rol.

#### 🧩 Estructura de los User Controls
- Basados principalmente en `DockPanel`, ofreciendo:
  - Diseño intuitivo y bien distribuido.
  - Visualización de datos mediante:
    - `TreeView` para selección de elementos.
    - `LiveCharts.Wpf` para mostrar gráficos dinámicos de alumnos y grupos.
    - `IntegerUpDown` y `DateTimePicker` para seleccionar fechas y horas.
    - `DataGrid` para visualización de tablas con:
      - Encabezados claros.
      - Menús contextuales para acciones rápidas.
      - Filtros para búsqueda eficiente.

#### 🧭 Navegación y acciones
- Navegación a través de **botones** con distintas funcionalidades:
  - Volver atrás sin guardar.
  - Guardar datos.
  - Aplicar/borrar filtros.
  - Abrir ventanas para insertar o visualizar información adicional.

#### 🛡️ Validación de datos
- Implementada mediante `Binding` y validaciones en tiempo real:
  - `ValidatesOnDataErrors`: asegura que los datos cumplan los requisitos definidos.
  - `NotifyOnValidationError`: notifica si hay errores de validación.
  - `UpdateSourceTrigger`: actualiza automáticamente las propiedades del modelo vista.
  - Validaciones adicionales en C#, con **ventanas emergentes** ante excepciones.

---

### 🎨 Paleta de colores

| Elemento                    | Color             | Justificación |
|-----------------------------|-------------------|---------------|
| Fondo de diálogos           | `Gunmetal`        | Bajo brillo, reduce fatiga visual, aspecto profesional. |
| Menú hamburguesa            | `Eerie Black`     | Uniformidad con el fondo, resalta iconos y textos. |
| Fuente                      | `Champagne`       | Buen contraste, tono cálido, equilibrio general. |
| Iconos                      | `Dodger Blue`, `Mauveine`, `Spring Green` | Colores vibrantes que facilitan identificación y accesibilidad visual. |

![paletaColores](https://github.com/user-attachments/assets/cc5db713-5e60-4657-a4cc-2c1fa25890d7)

---

### 2.5 Utilización del programa

Al iniciar el programa, se muestra una **splash screen** con el logo del sistema. Esta pantalla se cierra automáticamente tras unos segundos, dando paso a la ventana de **inicio de sesión**.

![splashscreen](https://github.com/user-attachments/assets/b9cc64ea-95e6-4c62-9842-d4ea7daca7fd)

#### 🔐 Inicio de sesión

El usuario debe introducir su **nombre de usuario (DNI)** y **contraseña**. Para facilitar las pruebas, existen usuarios genéricos según rol, por ejemplo:
- Usuario: `admin`
- Contraseña: `admin`

![inicioSesion](https://github.com/user-attachments/assets/c2e1bfa3-3dfa-4629-950e-cef65404aca3)

---

### 👤 Interfaz tras iniciar sesión

- **Barra superior izquierda**: indicador de la sección actual del programa.
- **Barra superior derecha**: botón de cerrar sesión.
- **Nombre del usuario y su rol**: visible debajo del botón.
- Al hacer clic sobre el nombre, se accede a la **información del usuario**.

![menuInicio](https://github.com/user-attachments/assets/3194db67-1476-408b-b84e-56079738d4ea)

En este diálogo se puede:
- Ver los datos personales.
- Modificar la contraseña.

![infoUser](https://github.com/user-attachments/assets/eec60d0c-2c16-4f9e-832e-1c2f464f56d8)

Para cambiar la contraseña, se abrirá un diálogo donde se introduce la contraseña antigua y la nueva.

![cambioContra](https://github.com/user-attachments/assets/8ad36efc-7ca2-401a-b8ba-402e6f3a8c28)

---

### ⚠️ Funcionalidad de Incidencias

#### 📋 Visualización de incidencias
- **Administrador/Directivo**: puede ver todas las incidencias.

![indicidencias](https://github.com/user-attachments/assets/cfa22ec6-649b-4927-9278-4a64c83ef2c2)

- **Tutor**: ve incidencias de alumnos de su grupo y en las que ha participado.

![incidenciasTutor](https://github.com/user-attachments/assets/237a6d1e-4dea-4c82-8e43-bb5123407305)

- **Profesor**: solo puede ver las que ha registrado o en las que ha estado presente.

![incidenciasProfe](https://github.com/user-attachments/assets/a5777bbd-e4ac-44d6-a56a-31bcb2001d50)

#### ➕ Añadir incidencias
- Desde el botón "Añadir incidencia".
- Desde el contexto del alumno (clic derecho → Amonestar).
- Profesor y tutor aparecen autocompletados si son el usuario actual.

#### 📄 Formulario de incidencia
Campos:
- Alumno, profesores involucrados, tipo, fecha, sanción, descripción, motivo.
- Posibilidad de **añadir un nuevo motivo** desde el mismo formulario.

![añadirIncidencia](https://github.com/user-attachments/assets/ef112e27-881c-42a9-9f35-5b1b99f16610)


![formularioIncidencia](https://github.com/user-attachments/assets/59657c28-5f7b-4b54-bcc6-9ce457eadfc1)
_

#### ❌ Eliminar incidencias
- Seleccionando mediante checkboxes y pulsando el botón eliminar.

![eliminarIncidencia](https://github.com/user-attachments/assets/c55a80c0-e171-4285-8eef-597f0591a7b8)

#### 🔎 Filtros disponibles
- NIA, fechas, tipo (incidencia/amonestación), motivo.

![filtros](https://github.com/user-attachments/assets/fce61ffc-7419-4422-8e83-16a931ea208a)

#### 🧹 Limpiar filtros
- Botón para borrar filtros y ver todos los registros nuevamente.
- Acceso rápido para **añadir motivo**.

#### 📁 Menú contextual (clic derecho)

![menuContextual](https://github.com/user-attachments/assets/7e85505e-955b-45ec-bf63-5249f6380957)

- Crear informe
- Editar
- Borrar

![crearInforme](https://github.com/user-attachments/assets/cbacbc2a-c001-47d6-aad6-26d2b0589e1a)


#### 📤 Informes de incidencias
- Se selecciona ubicación y nombre del archivo (por defecto: fecha actual).
- Formato PDF.

![crearInforme](https://github.com/user-attachments/assets/3e0ca581-1f83-4c17-a600-3dc7b9ac55e6)

![guardarInforme](https://github.com/user-attachments/assets/d3b4d29a-561f-4075-91d7-365cf67f68dc)

![informe](https://github.com/user-attachments/assets/96ab0d2b-9a7f-4b75-a45e-d71127f56232)

---

### 👥 Gestión de usuarios (Admin/Directivo)

- **Filtros por rol, DNI, nombre**.
- Clic derecho para:
  - Borrar usuario
  - Crear informe (solo profesores/tutores)
  - Modificar contraseña

![usuarios](https://github.com/user-attachments/assets/9e3e6907-56d6-4506-b2b2-bebd4700237d)

#### ➕ Añadir usuario
- Formulario de inserción.
- Asignación de grupo si es tutor.
- Posibilidad de crear grupo directamente.

![añadirUsuario](https://github.com/user-attachments/assets/72c5c07f-b23e-4390-b978-81d7cb300772)

---

### 🎓 Gestión de alumnos

- **Administrador/Directivo**: ve todos los alumnos.
- **Tutor**: ve los de su grupo.

![alumnos](https://github.com/user-attachments/assets/3dd98467-8936-4288-a15b-5c8562c9158d)

#### 🧹 Eliminar alumnos
- Por selección múltiple (checkbox).
- Desde menú contextual.

![eliminarUsuario](https://github.com/user-attachments/assets/eb9abeb9-368a-4ee5-8139-0ba0d71c28ab)

#### 🔎 Filtros disponibles
- Nombre, grupo, NIA, teléfono.

#### ➕ Añadir alumno
- Botón que abre el formulario de inserción.

![añadirAlumno](https://github.com/user-attachments/assets/cc408768-655b-4468-81fe-76e8d3ccf974)

#### 📋 Menú contextual del alumno
- Eliminar
- Crear informe
- Añadir amonestación/incidencia

![menuContextualAlumno](https://github.com/user-attachments/assets/3842f3ee-5977-40e1-92c9-d6455957502d)

---

### 📊 Gráficos (Alumnos y Grupos)

#### 📌 Selección inicial
- Opción para elegir entre gráficos de grupos o alumnos.
- En rol tutor, solo se muestran datos del grupo asignado.

![opciones](https://github.com/user-attachments/assets/fff14c26-3da4-4455-906e-cb15a129699c)

#### 📈 Gráfico de grupos
- Lista de grupos a la izquierda.
- Gráfico mensual al centro.
- Selector de año en parte superior.

![graficosGrupos](https://github.com/user-attachments/assets/31b16530-2478-4040-8214-aea063b8540e)

#### 📈 Gráfico de alumnos
- Lista de alumnos a la izquierda.
- Gráfico al centro.
- Selector de año arriba a la derecha.

![graficoAlumnos](https://github.com/user-attachments/assets/1ff3c49a-4e99-465d-9162-f5c37c2414cc)

---

### 📚 Gestión de motivos

- Visualización, edición y eliminación mediante clic derecho.
- Añadir motivo: botón que abre formulario vacío.

![gestionMotivos](https://github.com/user-attachments/assets/120fab2e-4aed-4f96-bdec-f44c19199c72)

![añadirMotivos](https://github.com/user-attachments/assets/a31c1a4b-eec5-49a8-a473-bab9f8ffb2f1)

---

### 📄 Generación de informes

Desde la sección de informes se puede generar informes de:

- **Incidencias/Amonestaciones**

![informeAmonestacion](https://github.com/user-attachments/assets/c80d7594-a3ec-4511-98db-3ba9567d2b57)

- **Alumnos**

![informeEstudiante](https://github.com/user-attachments/assets/ad9c6dcf-c0ce-4ea1-97fd-bff288d6a241)

- **Profesores/Tutores**

![informeProfesor](https://github.com/user-attachments/assets/e6f9984e-398a-4ce9-b948-08caea79abf9)

- **Grupos**

![informe de grupo](https://github.com/user-attachments/assets/9d579c50-e912-4e24-acc3-aae1fbcdc04a)

- **Resumen anual**

![informeAnual](https://github.com/user-attachments/assets/46427c6c-7b33-46a6-be20-302ab2cd2f67)

---

### ⚙️ Configuración del sistema

#### 🔐 Administración de roles
- Lista de roles a la izquierda.
- Permisos asignados en el centro.
- Añadir/Quitar permisos.
- Crear/Borrar roles.

![configuracionSist](https://github.com/user-attachments/assets/103b74c1-552d-4b50-9b0b-c73246787e84)

#### 🔐 Administración de permisos
- Lista de permisos a la izquierda.
- Roles que lo usan al centro.
- Añadir roles, crear nuevos permisos, borrar permisos.

![administrarPermisos](https://github.com/user-attachments/assets/de1e2b3d-99a3-40f9-ad41-4c75a581c653)

![administrarPermisos2](https://github.com/user-attachments/assets/71beb406-7fd3-42ec-aa65-1f411bb2c326)

#### 📤 Exportar datos
- Botón para exportar.
- Se genera un archivo XML con los datos del sistema.

