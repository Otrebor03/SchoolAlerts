using Microsoft.EntityFrameworkCore;

namespace ProyectoRRC.Backend.Modelo;

public partial class IncidenciaspartesrrcContext : DbContext
{
    public IncidenciaspartesrrcContext()
    {
    }

    public IncidenciaspartesrrcContext(DbContextOptions<IncidenciaspartesrrcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumnos { get; set; }

    public virtual DbSet<Amonestacione> Amonestaciones { get; set; }

    public virtual DbSet<Grupo> Grupos { get; set; }

    public virtual DbSet<Motivo> Motivos { get; set; }

    public virtual DbSet<Padre> Padres { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Persona> Personas { get; set; }

    public virtual DbSet<Profesor> Profesors { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseLazyLoadingProxies().UseMySql("server=127.0.0.1;port=3306;database=incidenciaspartesrrc;user=root;password=mysql", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.IdAlumno).HasName("PRIMARY");

            entity.ToTable("alumno");

            entity.HasIndex(e => e.IdPadre, "fk_alumno_padre1_idx");

            entity.HasIndex(e => e.IdGrupo, "idGrupo");

            entity.Property(e => e.IdAlumno).HasColumnName("idAlumno");
            entity.Property(e => e.Apellido1)
                .HasMaxLength(45)
                .HasColumnName("apellido1");
            entity.Property(e => e.Apellido2)
                .HasMaxLength(45)
                .HasColumnName("apellido2");
            entity.Property(e => e.Foto)
                .HasColumnType("blob")
                .HasColumnName("foto");
            entity.Property(e => e.IdGrupo).HasColumnName("idGrupo");
            entity.Property(e => e.IdPadre).HasColumnName("idPadre");
            entity.Property(e => e.Nia)
                .HasMaxLength(9)
                .HasColumnName("nia");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.Alumnos)
                .HasForeignKey(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("alumno_ibfk_1");

            entity.HasOne(d => d.IdPadreNavigation).WithMany(p => p.Alumnos)
                .HasForeignKey(d => d.IdPadre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_alumno_padre1");
        });

        modelBuilder.Entity<Amonestacione>(entity =>
        {
            entity.HasKey(e => e.IdAmonestacion).HasName("PRIMARY");

            entity.ToTable("amonestaciones");

            entity.HasIndex(e => e.IdProfesorRegistra, "fk_amonestaciones_profesor1_idx");

            entity.HasIndex(e => e.IdProfesorHecho, "fk_amonestaciones_profesor2_idx");

            entity.HasIndex(e => e.IdAlumno, "idAlumno");

            entity.HasIndex(e => e.IdMotivo, "idMotivo");

            entity.Property(e => e.IdAmonestacion).HasColumnName("idAmonestacion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaHoraHecho)
                .HasColumnType("datetime")
                .HasColumnName("fechaHoraHecho");
            entity.Property(e => e.FechaHoraRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fechaHoraRegistro");
            entity.Property(e => e.IdAlumno).HasColumnName("idAlumno");
            entity.Property(e => e.IdMotivo).HasColumnName("idMotivo");
            entity.Property(e => e.IdProfesorHecho).HasColumnName("idProfesorHecho");
            entity.Property(e => e.IdProfesorRegistra).HasColumnName("idProfesorRegistra");
            entity.Property(e => e.Sancion)
                .HasMaxLength(45)
                .HasColumnName("sancion");
            entity.Property(e => e.Tipo)
                .HasMaxLength(45)
                .HasColumnName("tipo");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.Amonestaciones)
                .HasForeignKey(d => d.IdAlumno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("amonestacionesincidencias_ibfk_2");

            entity.HasOne(d => d.IdMotivoNavigation).WithMany(p => p.Amonestaciones)
                .HasForeignKey(d => d.IdMotivo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("amonestacionesincidencias_ibfk_1");

            entity.HasOne(d => d.IdProfesorHechoNavigation).WithMany(p => p.AmonestacioneIdProfesorHechoNavigations)
                .HasForeignKey(d => d.IdProfesorHecho)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_amonestaciones_profesor2");

            entity.HasOne(d => d.IdProfesorRegistraNavigation).WithMany(p => p.AmonestacioneIdProfesorRegistraNavigations)
                .HasForeignKey(d => d.IdProfesorRegistra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_amonestaciones_profesor1");
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.IdGrupo).HasName("PRIMARY");

            entity.ToTable("grupo");

            entity.Property(e => e.IdGrupo).HasColumnName("idGrupo");
            entity.Property(e => e.NombreGrupo)
                .HasMaxLength(45)
                .HasColumnName("nombreGrupo");

            entity.HasMany(d => d.IdProfesors).WithMany(p => p.IdGrupos)
                .UsingEntity<Dictionary<string, object>>(
                    "Grupoprofesor",
                    r => r.HasOne<Profesor>().WithMany()
                        .HasForeignKey("IdProfesor")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("grupo_has_profesor_ibfk_2"),
                    l => l.HasOne<Grupo>().WithMany()
                        .HasForeignKey("IdGrupo")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("grupo_has_profesor_ibfk_1"),
                    j =>
                    {
                        j.HasKey("IdGrupo", "IdProfesor")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("grupoprofesor");
                        j.HasIndex(new[] { "IdProfesor" }, "idProfesor");
                        j.IndexerProperty<int>("IdGrupo").HasColumnName("idGrupo");
                        j.IndexerProperty<int>("IdProfesor").HasColumnName("idProfesor");
                    });
        });

        modelBuilder.Entity<Motivo>(entity =>
        {
            entity.HasKey(e => e.IdMotivo).HasName("PRIMARY");

            entity.ToTable("motivos");

            entity.Property(e => e.IdMotivo).HasColumnName("idMotivo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .HasColumnName("descripcion");
            entity.Property(e => e.Motivo1)
                .HasMaxLength(45)
                .HasColumnName("motivo");
        });

        modelBuilder.Entity<Padre>(entity =>
        {
            entity.HasKey(e => e.IdPadre).HasName("PRIMARY");

            entity.ToTable("padre");

            entity.HasIndex(e => e.IdPersona, "fk_padre_persona1_idx");

            entity.Property(e => e.IdPadre).HasColumnName("idPadre");
            entity.Property(e => e.IdPersona).HasColumnName("idPersona");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Padres)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_padre_persona1");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PRIMARY");

            entity.ToTable("permisos");

            entity.Property(e => e.IdPermiso).HasColumnName("idPermiso");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .HasColumnName("descripcion");

            entity.HasMany(d => d.IdRols).WithMany(p => p.IdPermisos)
                .UsingEntity<Dictionary<string, object>>(
                    "Permisosrole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("permisos_has_roles_ibfk_2"),
                    l => l.HasOne<Permiso>().WithMany()
                        .HasForeignKey("IdPermiso")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("permisos_has_roles_ibfk_1"),
                    j =>
                    {
                        j.HasKey("IdPermiso", "IdRol")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("permisosroles");
                        j.HasIndex(new[] { "IdRol" }, "idRol");
                        j.IndexerProperty<int>("IdPermiso").HasColumnName("idPermiso");
                        j.IndexerProperty<int>("IdRol").HasColumnName("idRol");
                    });
        });

        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("PRIMARY");

            entity.ToTable("persona");

            entity.HasIndex(e => e.IdRol, "fk_persona_roles1_idx");

            entity.HasIndex(e => e.IdPersona, "idPersona_UNIQUE").IsUnique();

            entity.Property(e => e.IdPersona).HasColumnName("idPersona");
            entity.Property(e => e.Apellido1)
                .HasMaxLength(45)
                .HasColumnName("apellido1");
            entity.Property(e => e.Apellido2)
                .HasMaxLength(45)
                .HasColumnName("apellido2");
            entity.Property(e => e.Contrasenya)
                .HasMaxLength(15)
                .HasColumnName("contrasenya");
            entity.Property(e => e.Dni)
                .HasMaxLength(9)
                .HasColumnName("dni");
            entity.Property(e => e.Foto)
                .HasColumnType("blob")
                .HasColumnName("foto");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Personas)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_persona_roles1");
        });

        modelBuilder.Entity<Profesor>(entity =>
        {
            entity.HasKey(e => e.IdProfesor).HasName("PRIMARY");

            entity.ToTable("profesor");

            entity.HasIndex(e => e.IdPersona, "fk_profesor_persona1_idx");

            entity.Property(e => e.IdProfesor).HasColumnName("idProfesor");
            entity.Property(e => e.IdPersona).HasColumnName("idPersona");

            entity.HasOne(d => d.IdPersonaNavigation).WithMany(p => p.Profesors)
                .HasForeignKey(d => d.IdPersona)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_profesor_persona1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(45)
                .HasColumnName("nombreRol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
