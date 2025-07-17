using Microsoft.EntityFrameworkCore;
using Trivo.Dominio.Enum;
using Trivo.Dominio.Modelos;

namespace Trivo.Infraestructura.Persistencia.Contexto;

public class TrivoContexto : DbContext
{
    public TrivoContexto(DbContextOptions<TrivoContexto> options) : base(options) {}

    #region Modelos
    
        public DbSet<Usuario> Usuario { get; set; }
             
        public DbSet<Administrador> Administrador { get; set; }
        
        public DbSet<Emparejamiento> Emparejamiento { get; set; }
        
        public DbSet<Reporte> Reporte { get; set; }
        
        public DbSet<Reclutador> Reclutador { get; set; }
        
        public DbSet<Experto> Experto { get; set; }
        
        public DbSet<Habilidad> Habilidad { get; set; }
        
        public DbSet<UsuarioHabilidad> UsuarioHabilidad { get; set; }
        
        public DbSet<CategoriaInteres> CategoriaInteres { get; set; }
        
        public DbSet<Interes> Interes { get; set; }
        
        public DbSet<UsuarioInteres> UsuarioInteres { get; set; }
        
        public DbSet<Codigo> Codigo { get; set; }
        
        public DbSet<Chat> Chat { get; set; }
        
        public DbSet<ChatUsuario> ChatUsuario { get; set; }
        
        public DbSet<Mensaje> Mensaje { get; set; }
        
        public DbSet<Notificacion> Notificacion { get; set; }
        
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
        
        #region Tablas

            modelBuilder.Entity<Usuario>()
                .ToTable("Usuario");
            
            modelBuilder.Entity<Administrador>()
                .ToTable("Administrador");

            modelBuilder.Entity<Emparejamiento>()
                .ToTable("Emparejamiento");

            modelBuilder.Entity<Reporte>()
                .ToTable("Reporte");

            modelBuilder.Entity<Reclutador>()
                .ToTable("Reclutador");

            modelBuilder.Entity<Experto>()
                .ToTable("Experto");

            modelBuilder.Entity<Habilidad>()
                .ToTable("Habilidad");

            modelBuilder.Entity<UsuarioHabilidad>()
                .ToTable("UsuarioHabilidad");

            modelBuilder.Entity<CategoriaInteres>()
                .ToTable("CategoriaInteres");

            modelBuilder.Entity<Interes>()
                .ToTable("Interes");

            modelBuilder.Entity<UsuarioInteres>()
                .ToTable("UsuarioInteres");

            modelBuilder.Entity<Codigo>()
                .ToTable("Codigo");

            modelBuilder.Entity<Chat>()
                .ToTable("Chat");

            modelBuilder.Entity<ChatUsuario>()
                .ToTable("ChatUsuario");

            modelBuilder.Entity<Mensaje>()
                .ToTable("Mensaje");

            modelBuilder.Entity<Notificacion>()
                .ToTable("Notificacion");
            
        #endregion

        #region PrimaryKey

            modelBuilder.Entity<Usuario>()
                .HasKey(e => e.Id)
                .HasName("PKUsuarioId");
                    
            modelBuilder.Entity<Administrador>()
                .HasKey(e => e.Id)
                .HasName("PKAdministradorId");
            
            modelBuilder.Entity<Emparejamiento>()
                .HasKey(e => e.Id)
                .HasName("PKEmparejamientoId");
            
            modelBuilder.Entity<Reporte>()
                .HasKey(e => e.ReporteId)
                .HasName("PKReporteId");
            
            modelBuilder.Entity<Reclutador>()
                .HasKey(e => e.Id)
                .HasName("PKReclutadorId");
            
            modelBuilder.Entity<Experto>()
                .HasKey(e => e.Id)
                .HasName("PKExpertoId");
            
            
            modelBuilder.Entity<Habilidad>()
                .HasKey(h => h.HabilidadId)
                .HasName("PKHabilidadId");
            
            modelBuilder.Entity<UsuarioHabilidad>()
                .HasKey(uh => new { uh.UsuarioId, uh.HabilidadId });
            
            modelBuilder.Entity<CategoriaInteres>()
                .HasKey(c => c.CategoriaId)
                .HasName("PKCategoriaInteresId");
            
            modelBuilder.Entity<Interes>()
                .HasKey(e => e.Id)
                .HasName("PKInteresId");

            modelBuilder.Entity<UsuarioInteres>()
                .HasKey(ui => new { ui.InteresId, ui.UsuarioId });

            modelBuilder.Entity<Codigo>()
                .HasKey(c => c.CodigoId)
                .HasName("PKCodigoId");

            modelBuilder.Entity<Chat>()
                .HasKey(c => c.Id)
                .HasName("PkChatId");

            modelBuilder.Entity<ChatUsuario>()
                .HasKey(cu => new { cu.ChatId, cu.UsuarioId });
            
            modelBuilder.Entity<Mensaje>()
                .HasKey(m => m.MensajeId)
                .HasName("PKMensajeId");
            
            modelBuilder.Entity<Notificacion>()
                .HasKey(m => m.NotificacionId)
                .HasName("PKNotificacionId");
            
        #endregion

        #region ForeignKeys

            modelBuilder.Entity<Codigo>()
                .Property(c => c.UsuarioId)
                .HasColumnName("FkUsuariosId");

            modelBuilder.Entity<Notificacion>()
                .Property(c => c.UsuarioId)
                .HasColumnName("FkUsuariosId");

            modelBuilder.Entity<Interes>()
                .Property(c => c.CreadoPor)
                .HasColumnName("FkCreadoPorId");

            modelBuilder.Entity<Mensaje>()
                .Property(c => c.EmisorId)
                .HasColumnName("FkEmisorId");

            modelBuilder.Entity<Reporte>()
                .Property(c => c.ReportadoPor)
                .HasColumnName("FkReportadoPorId");

            modelBuilder.Entity<Experto>()
                .Property(c => c.UsuarioId)
                .HasColumnName("FkUsuariosId");

            modelBuilder.Entity<Reclutador>()
                .Property(c => c.UsuarioId)
                .HasColumnName("FkUsuariosId");

            modelBuilder.Entity<Emparejamiento>()
                .Property(c => c.ExpertoId)
                .HasColumnName("FkExpertoId");

            modelBuilder.Entity<Emparejamiento>()
                .Property(c => c.ReclutadorId)
                .HasColumnName("FkReclutadorId");

            modelBuilder.Entity<Reporte>()
                .Property(c => c.MensajeId)
                .HasColumnName("FkMensajeId");

            modelBuilder.Entity<Mensaje>()
                .Property(c => c.ChatId)
                .HasColumnName("FkChatId");

            modelBuilder.Entity<ChatUsuario>()
                .Property(c => c.ChatId)
                .HasColumnName("FkChatId");

            modelBuilder.Entity<ChatUsuario>()
                .Property(c => c.UsuarioId)
                .HasColumnName("FkUsuariosId");

            modelBuilder.Entity<UsuarioHabilidad>()
                .Property(c => c.UsuarioId)
                .HasColumnName("FkUsuariosId");

            modelBuilder.Entity<UsuarioHabilidad>()
                .Property(c => c.HabilidadId)
                .HasColumnName("FkHabilidadId");

            modelBuilder.Entity<UsuarioInteres>()
                .Property(c => c.UsuarioId)
                .HasColumnName("FkUsuariosId");

            modelBuilder.Entity<UsuarioInteres>()
                .Property(c => c.InteresId)
                .HasColumnName("FkInteresId");

            modelBuilder.Entity<Interes>()
                .Property(c => c.CategoriaId)
                .HasColumnName("FkCategoriaId");
            
        #endregion
        
        #region Relaciones

             modelBuilder.Entity<Usuario>()
                 .HasMany(u => u.Codigos)
                 .WithOne(c => c.Usuarios)
                 .HasForeignKey(c => c.UsuarioId)
                 .IsRequired()
                 .HasConstraintName("FKUsuariosId");
        
             modelBuilder.Entity<Usuario>()
                 .HasMany(u => u.Notificaciones)
                 .WithOne(c => c.Usuario)
                 .HasForeignKey(c => c.UsuarioId)
                 .IsRequired()
                 .HasConstraintName("FKUsuariosId");
             
             modelBuilder.Entity<Usuario>()
                 .HasMany(u => u.Interes)
                 .WithOne(c => c.Usuario)
                 .HasForeignKey(c => c.CreadoPor)
                 .IsRequired()
                 .HasConstraintName("FKCreadoPorId");
             
             modelBuilder.Entity<Usuario>()
                 .HasMany(u => u.Mensajes)
                 .WithOne(c => c.Usuario)
                 .HasForeignKey(c => c.EmisorId)
                 .IsRequired()
                 .HasConstraintName("FKEmisorId");
             
             modelBuilder.Entity<Usuario>()
                 .HasMany(u => u.Reportes)
                 .WithOne(c => c.Usuario)
                 .HasForeignKey(c => c.ReportadoPor)
                 .IsRequired()
                 .HasConstraintName("FKReportadoPorId");
             
             modelBuilder.Entity<Usuario>()
                 .HasMany(u => u.Expertos)
                 .WithOne(c => c.Usuario)
                 .HasForeignKey(c => c.UsuarioId)
                 .IsRequired()
                 .HasConstraintName("FKUsuariosId");
             
             modelBuilder.Entity<Usuario>()
                 .HasMany(u => u.Reclutadores)
                 .WithOne(c => c.Usuario)
                 .HasForeignKey(c => c.UsuarioId)
                 .IsRequired()
                 .HasConstraintName("FKUsuariosId");
             
             modelBuilder.Entity<Experto>()
                 .HasMany(u => u.Emparejamientos)
                 .WithOne(c => c.Experto)
                 .HasForeignKey(c => c.ExpertoId)
                 .IsRequired()
                 .HasConstraintName("FKExpertoId");
             
             modelBuilder.Entity<Reclutador>()
                 .HasMany(u => u.Emparejamientos)
                 .WithOne(c => c.Reclutador)
                 .HasForeignKey(c => c.ReclutadorId)
                 .IsRequired()
                 .HasConstraintName("FKReclutadorId");

             modelBuilder.Entity<Mensaje>()
                 .HasMany(m => m.Reportes)
                 .WithOne(c => c.Mensaje)
                 .HasForeignKey(c => c.MensajeId)
                 .IsRequired()
                 .HasConstraintName("FKMensajeId");
             
             modelBuilder.Entity<Mensaje>()
                 .HasOne(m => m.Chat)
                 .WithMany(c => c.Mensajes)
                 .HasForeignKey(m => m.ChatId)
                 .HasConstraintName("FK_Mensaje_Chat")
                 .OnDelete(DeleteBehavior.Cascade);

             modelBuilder.Entity<ChatUsuario>()
                 .HasOne(m => m.Chat)
                 .WithMany(c => c.ChatUsuarios)
                 .HasForeignKey(c => c.ChatId)
                 .IsRequired()
                 .HasConstraintName("FKChatId");

             modelBuilder.Entity<ChatUsuario>()
                 .HasOne(x => x.Usuario)
                 .WithMany(u => u.ChatUsuarios)
                 .HasForeignKey(x => x.UsuarioId)
                 .IsRequired()
                 .HasConstraintName("FKUsuariosId");
             
             modelBuilder.Entity<UsuarioHabilidad>()
                 .HasOne(h => h.Usuario)
                 .WithMany(u => u.UsuarioHabilidades)
                 .HasForeignKey(c => c.UsuarioId)
                 .IsRequired()
                 .HasConstraintName("FKUsuariosId");
             
             modelBuilder.Entity<UsuarioHabilidad>()
                 .HasOne(h => h.Habilidad)
                 .WithMany(u => u.UsuarioHabilidades)
                 .HasForeignKey(c => c.HabilidadId)
                 .IsRequired()
                 .HasConstraintName("FKHabilidadId");

             modelBuilder.Entity<UsuarioInteres>()
                 .HasOne(i => i.Usuario)
                 .WithMany(u => u.UsuarioInteres)
                 .HasForeignKey(c => c.UsuarioId)
                 .IsRequired()
                 .HasConstraintName("FKUsuariosId");
             
             modelBuilder.Entity<UsuarioInteres>()
                 .HasOne(i => i.Interes)
                 .WithMany(u => u.UsuarioInteres)
                 .HasForeignKey(c => c.InteresId)
                 .IsRequired()
                 .HasConstraintName("FKInteresId");
            
             modelBuilder.Entity<CategoriaInteres>()
                 .HasMany(i => i.Interes)
                 .WithOne(i => i.Categoria)
                 .HasForeignKey(c => c.CategoriaId)
                 .IsRequired()
                 .HasConstraintName("FKCategoriaId");
             
             #endregion

        #region Usuario

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(a => a.Id)
                    .HasColumnName("PkUsuarioId")
                    .IsRequired();
                
                entity.Property(c => c.Nombre)
                    .IsRequired()
                    .HasMaxLength(30);
                
                entity.Property(c => c.Apellido)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(c => c.Biografia)
                    .IsRequired()
                    .HasColumnType("text");
                
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(u => u.Email)
                    .IsUnique()
                    .HasDatabaseName("UQUsuariosEmail");
            
                entity.Property(u => u.ContrasenaHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(u => u.NombreUsuario)
                    .IsRequired()              
                    .HasMaxLength(50);

                entity.HasIndex(u => u.NombreUsuario)
                    .IsUnique()
                    .HasDatabaseName("UQUsuariosNombreUsuario");

                entity.Property(u => u.FotoPerfil)
                    .HasMaxLength(255)
                    .IsRequired(false);       

                entity.Property(u => u.Linkedin)
                    .HasMaxLength(255)
                    .IsRequired(false);        

                entity.Property(u => u.Ubicacion)
                    .HasMaxLength(50)
                    .IsRequired(false);        

                entity.Property(u => u.CuentaConfirmada)
                    .IsRequired();

                entity.Property(u => u.FechaRegistro)
                    .IsRequired();

                entity.Property(u => u.FechaActualizacion)
                    .IsRequired(false);

                entity.Property(u => u.EstadoUsuario)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });
            

        #endregion
        
        #region Administrador
        
            modelBuilder.Entity<Administrador>(entity =>
            {
                
                entity.Property(a => a.Id)
                    .HasColumnName("PkAdministradorId")
                    .IsRequired();
                
                entity.Property(a => a.Nombre)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(a => a.Apellido)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(a => a.Biografia)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(a => a.Email)
                    .HasMaxLength(100)
                    .IsRequired(false);

                entity.HasIndex(a => a.Email)
                    .IsUnique()
                    .HasDatabaseName("UQAdministradorEmail");

                entity.Property(a => a.ContrasenaHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(a => a.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(a => a.NombreUsuario)
                    .IsUnique()
                    .HasDatabaseName("UQAdministradorNombreUsuario");

                entity.Property(a => a.FotoPerfil)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(a => a.Linkedin)
                    .HasMaxLength(255)
                    .IsRequired(false);

                entity.Property(a => a.FechaRegistro)
                    .IsRequired();

                entity.Property(e => e.FechaActualizacion)
                    .IsRequired(false);

                entity.Property(a => a.Activo)
                    .IsRequired();
                
            });
        #endregion

        #region Emparejamiento

            modelBuilder.Entity<Emparejamiento>(entity =>
            {
                entity.Property(a => a.Id)
                    .HasColumnName("PkEmparejamientoId")
                    .IsRequired();
                
                entity.Property(a => a.FechaRegistro)
                    .IsRequired();
                
                entity.Property(e => e.FechaActualizacion)
                    .IsRequired(false);
                
                entity.Property(u => u.EmparejamientoEstado)
                    .IsRequired()
                    .IsRequired()
                    .HasColumnType("varchar(50)");
                
                entity.Property(u => u.ExpertoEstado)
                    .IsRequired()
                    .IsRequired()
                    .HasColumnType("varchar(50)");
                
                entity.Property(u => u.ReclutadorEstado)
                    .IsRequired()
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

        #endregion
        
        #region Reporte

            modelBuilder.Entity<Reporte>(entity =>
            {
                entity.Property(re => re.ReporteId)
                    .HasColumnName("PkReporteId")
                    .IsRequired();
                
                entity.Property(re => re.Nota)
                    .HasColumnType("text")
                    .IsRequired();
                
                entity.Property(u => u.EstadoReporte)
                    .IsRequired()
                    .IsRequired()
                    .HasColumnType("varchar(50)");
                
            });

        #endregion

        #region Reclutador
        
            modelBuilder.Entity<Reclutador>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("PkReclutadorId")
                    .IsRequired();
                
                entity.Property(e => e.NombreEmpresa)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FechaRegistro)
                    .IsRequired();
                
                entity.Property(e => e.FechaActualizacion)
                    .IsRequired(false);
            });

        #endregion
        
        #region Experto

            modelBuilder.Entity<Experto>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("PkExpertoId")
                    .IsRequired();
                
                entity.Property(e => e.DisponibleParaProyectos)
                    .IsRequired();

                entity.Property(e => e.Contratado)
                    .IsRequired();

                entity.Property(e => e.FechaRegistro)
                    .IsRequired();

                entity.Property(e => e.FechaActualizacion)
                    .IsRequired(false);
            });
            
        #endregion
        
        #region Habilidad
        
            modelBuilder.Entity<Habilidad>(entity =>
            {
                entity.Property(e => e.HabilidadId)
                    .HasColumnName("PkHabilidadId")
                    .IsRequired();
                
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.FechaRegistro)
                    .IsRequired();
            });
        
        #endregion

        #region Categoria Intereses

            modelBuilder.Entity<CategoriaInteres>(entity =>
            {
                entity.Property(e => e.CategoriaId)
                    .HasColumnName("PkCategoriaId")
                    .IsRequired();
                
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });
            
        #endregion

        #region Interes
        
            modelBuilder.Entity<Interes>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("PkInteresId")
                    .IsRequired();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FechaRegistro)
                    .IsRequired();

                entity.Property(e => e.FechaActualizacion)
                    .IsRequired(false);
            });
        
        #endregion
        
        #region Codigo
        
            modelBuilder.Entity<Codigo>(entity =>
            {
                entity.Property(e => e.CodigoId)
                    .HasColumnName("PkCodigoId")
                    .IsRequired();
                
                entity.Property(e => e.Valor)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
                
                entity.Property(e => e.Usado)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(e => e.Expiracion)
                    .IsRequired();

                entity.Property(e => e.Creado)
                    .IsRequired();

                entity.Property(e => e.Revocado)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(e => e.RefrescarCodigo)
                    .IsRequired(false)
                    .HasDefaultValue(false);
            });
            
        #endregion
        
        #region Chat
        
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("PkChatId")
                    .IsRequired();
                
                entity.Property(e => e.TipoChat)
                    .IsRequired()
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Activo)
                    .IsRequired();

                entity.Property(e => e.FechaRegistro)
                    .IsRequired();

                entity.Property(e => e.FechaActualizacion)
                    .IsRequired(false);
            });
            
        #endregion
        
        #region ChatUsuario
        
            modelBuilder.Entity<ChatUsuario>(entity =>
            {
                entity.Property(e => e.FechaIngreso)
                    .IsRequired();

                entity.Property(e => e.FechaSalida)
                    .IsRequired(false); 
                
                entity.Property(e => e.NombreChat)
                    .HasMaxLength(100)
                    .IsRequired();
            });

            #endregion
        
        #region Mensaje 
        
            modelBuilder.Entity<Mensaje>(entity =>
            {
                entity.Property(e => e.ChatId)
                    .HasColumnName("PkChatId")
                    .IsRequired();
                
                entity.Property(e => e.Contenido)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.FechaEnvio)
                    .IsRequired();

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.FechaRegistro)
                    .IsRequired();
            });
            
        #endregion

        #region Notificacion
        
            modelBuilder.Entity<Notificacion>(entity =>
            {
                
                entity.Property(e => e.NotificacionId)
                    .HasColumnName("PkNotificacionId")
                    .IsRequired();
                
                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Contenido)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FechaCreacion)
                    .IsRequired();

                entity.Property(e => e.Leida)
                    .IsRequired();

                entity.Property(e => e.FechaLeida)
                    .IsRequired();
            });
            
        #endregion
        
    }
    
}