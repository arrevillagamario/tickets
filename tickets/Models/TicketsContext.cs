using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace tickets.Models;

public partial class TicketsContext : DbContext
{
    public TicketsContext()
    {
    }

    public TicketsContext(DbContextOptions<TicketsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Archivo> Archivos { get; set; }

    public virtual DbSet<Comentario> Comentarios { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<RegistroActualizacione> RegistroActualizaciones { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=DefaultConecction");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Archivo>(entity =>
        {
            entity.HasKey(e => e.IdArchivo);

            entity.Property(e => e.NombreArchivo).HasMaxLength(50);
            entity.Property(e => e.TipoArchivo).HasMaxLength(10);

            entity.HasOne(d => d.IdTicketNavigation).WithMany(p => p.Archivos)
                .HasForeignKey(d => d.IdTicket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Archivos_Ticket");
        });

        modelBuilder.Entity<Comentario>(entity =>
        {
            entity.HasKey(e => e.IdComentario);

            entity.ToTable("Comentario");

            entity.Property(e => e.Comentario1)
                .IsUnicode(false)
                .HasColumnName("Comentario");

            entity.HasOne(d => d.IdUsuarioRNavigation).WithMany(p => p.Comentarios)
                .HasForeignKey(d => d.IdUsuarioR)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comentario_Usuario");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.IdEstado);

            entity.ToTable("Estado");

            entity.Property(e => e.Estado1)
                .HasMaxLength(50)
                .HasColumnName("Estado");
        });

        modelBuilder.Entity<RegistroActualizacione>(entity =>
        {
            entity.HasKey(e => e.IdRegistro);

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.RegistroActualizaciones)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RegistroActualizaciones_Estado");

            entity.HasOne(d => d.IdTicketNavigation).WithMany(p => p.RegistroActualizaciones)
                .HasForeignKey(d => d.IdTicket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RegistroActualizaciones_Ticket");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.ToTable("Rol");

            entity.Property(e => e.NombreRol).HasMaxLength(50);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.IdTicket);

            entity.ToTable("Ticket");

            entity.Property(e => e.IdTicket).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Prioridad).HasMaxLength(50);
            entity.Property(e => e.Servicio).HasMaxLength(50);

            entity.HasOne(d => d.EstadoNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Estado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Estado");

            entity.HasOne(d => d.IdUsuarioAsignadoNavigation).WithMany(p => p.TicketIdUsuarioAsignadoNavigations)
                .HasForeignKey(d => d.IdUsuarioAsignado)
                .HasConstraintName("FK_Ticket_UsuarioAsignado");

            entity.HasOne(d => d.IdUsuarioSolicitanteNavigation).WithMany(p => p.TicketIdUsuarioSolicitanteNavigations)
                .HasForeignKey(d => d.IdUsuarioSolicitante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_UsuarioSolicitante");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Empresa).HasMaxLength(50);
            entity.Property(e => e.NombreUsuario).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
