using System;
using System.Collections.Generic;
using Gruas.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gruas.API.Data;

public partial class GruasContext : DbContext
{
    public GruasContext()
    {
    }

    public GruasContext(DbContextOptions<GruasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetSystem> AspNetSystems { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Configuracion> Configuracions { get; set; }

    public virtual DbSet<Cuentum> Cuenta { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<EstatusServicio> EstatusServicios { get; set; }

    public virtual DbSet<Grua> Gruas { get; set; }

    public virtual DbSet<GruaImagen> GruaImagens { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Municipio> Municipios { get; set; }

    public virtual DbSet<Propuestum> Propuesta { get; set; }

    public virtual DbSet<Proveedor> Proveedors { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<TipoGrua> TipoGruas { get; set; }

    public virtual DbSet<TipoServicio> TipoServicios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-540B3V8;Initial Catalog=Gruas;Persist Security Info=True;User ID=sa;Password=sql2;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.Property(e => e.RoleId).HasMaxLength(450);

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetSystem>(entity =>
        {
            entity.ToTable("AspNetSystem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Clave).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(500);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Configuracion>(entity =>
        {
            entity.ToTable("Configuracion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Codigo).HasMaxLength(500);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Modulo).HasMaxLength(50);
            entity.Property(e => e.ValorDate).HasColumnType("datetime");
            entity.Property(e => e.ValorDecimal).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.ValorString).HasMaxLength(500);
        });

        modelBuilder.Entity<Cuentum>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Apellidos).HasMaxLength(500);
            entity.Property(e => e.CorreoElectronico).HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(500);
            entity.Property(e => e.NombreUsuario).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(50);

            entity.HasOne(d => d.Proveedor).WithMany(p => p.CuentaNavigation)
                .HasForeignKey(d => d.ProveedorId)
                .HasConstraintName("FK_Cuenta_Proveedor");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.ToTable("Estado");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Banderazo).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostoMinimo).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CuotaKm).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Maniobras).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.NombreCorto).HasMaxLength(50);
        });

        modelBuilder.Entity<EstatusServicio>(entity =>
        {
            entity.ToTable("EstatusServicio");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<Grua>(entity =>
        {
            entity.ToTable("Grua");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Marca).HasMaxLength(50);
            entity.Property(e => e.Modelo).HasMaxLength(50);
            entity.Property(e => e.Placas).HasMaxLength(50);

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Gruas)
                .HasForeignKey(d => d.ProveedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grua_Proveedor");

            entity.HasOne(d => d.TipoGrua).WithMany(p => p.Gruas)
                .HasForeignKey(d => d.TipoGruaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grua_TipoGrua");
        });

        modelBuilder.Entity<GruaImagen>(entity =>
        {
            entity.ToTable("GruaImagen");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(500);

            entity.HasOne(d => d.Grua).WithMany(p => p.GruaImagens)
                .HasForeignKey(d => d.GruaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GruaImagen_Grua");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.ToTable("Marca");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<Municipio>(entity =>
        {
            entity.ToTable("Municipio");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(500);
            entity.Property(e => e.NombreCorto).HasMaxLength(50);

            entity.HasOne(d => d.Estado).WithMany(p => p.Municipios)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Municipio_Estado");
        });

        modelBuilder.Entity<Propuestum>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.MontoPropuesto).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.TiempoPropuesto).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.Grua).WithMany(p => p.Propuesta)
                .HasForeignKey(d => d.GruaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Propuesta_Grua");

            entity.HasOne(d => d.Servicio).WithMany(p => p.Propuesta)
                .HasForeignKey(d => d.ServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Propuesta_Servicio");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.ToTable("Proveedor");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Banco).HasMaxLength(50);
            entity.Property(e => e.Cuenta).HasMaxLength(50);
            entity.Property(e => e.Direccion).HasMaxLength(500);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.RazonSocial).HasMaxLength(500);
            entity.Property(e => e.Rfc).HasMaxLength(50);
            entity.Property(e => e.Telefono1)
                .HasMaxLength(50)
                .HasColumnName("Telefono_1");
            entity.Property(e => e.Telefono2)
                .HasMaxLength(50)
                .HasColumnName("Telefono_2");

            entity.HasOne(d => d.Estado).WithMany(p => p.Proveedors)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Proveedor_Estado");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.ToTable("Servicio");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Banderazo).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CarreteraCarril).HasColumnName("Carretera_Carril");
            entity.Property(e => e.CarreteraDestino)
                .HasMaxLength(500)
                .HasColumnName("Carretera_Destino");
            entity.Property(e => e.CarreteraKm).HasColumnName("Carretera_Km");
            entity.Property(e => e.CuotaKm).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DestinoDireccion)
                .HasMaxLength(500)
                .HasColumnName("Destino_Direccion");
            entity.Property(e => e.DestinoLat)
                .HasMaxLength(500)
                .HasColumnName("Destino_Lat");
            entity.Property(e => e.DestinoLon)
                .HasMaxLength(500)
                .HasColumnName("Destino_Lon");
            entity.Property(e => e.DestinoMunicipio)
                .HasMaxLength(500)
                .HasColumnName("Destino_Municipio");
            entity.Property(e => e.DestinoReferencia)
                .HasMaxLength(500)
                .HasColumnName("Destino_Referencia");
            entity.Property(e => e.DireccionCongestion)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Direccion_Congestion");
            entity.Property(e => e.DireccionKmTotales)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Direccion_KmTotales");
            entity.Property(e => e.DireccionMinsNormal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Direccion_MinsNormal");
            entity.Property(e => e.DireccionMinsTrafico)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("Direccion_MinsTrafico");
            entity.Property(e => e.Distancia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EstacionamientoPiso)
                .HasMaxLength(50)
                .HasColumnName("Estacionamiento_Piso");
            entity.Property(e => e.EstacionamientoTipo)
                .HasMaxLength(500)
                .HasColumnName("Estacionamiento_Tipo");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.LugarUbicuidad).HasMaxLength(500);
            entity.Property(e => e.ManiobrasCosto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrigenDireccion)
                .HasMaxLength(500)
                .HasColumnName("Origen_Direccion");
            entity.Property(e => e.OrigenLat)
                .HasMaxLength(500)
                .HasColumnName("Origen_Lat");
            entity.Property(e => e.OrigenLon)
                .HasMaxLength(500)
                .HasColumnName("Origen_Lon");
            entity.Property(e => e.OrigenMunicipio)
                .HasMaxLength(500)
                .HasColumnName("Origen_Municipio");
            entity.Property(e => e.OrigenReferencia)
                .HasMaxLength(500)
                .HasColumnName("Origen_Referencia");
            entity.Property(e => e.TiempoLlegada).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TipoVehiculo).HasMaxLength(50);
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalSugerido).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VehiculoAnio).HasColumnName("Vehiculo_Anio");
            entity.Property(e => e.VehiculoColor)
                .HasMaxLength(500)
                .HasColumnName("Vehiculo_Color");
            entity.Property(e => e.VehiculoCuentaModificaciones).HasColumnName("Vehiculo_CuentaModificaciones");
            entity.Property(e => e.VehiculoCuentaPlacas)
                .HasMaxLength(50)
                .HasColumnName("Vehiculo_CuentaPlacas");
            entity.Property(e => e.VehiculoDescripcionModificaciones)
                .HasMaxLength(500)
                .HasColumnName("Vehiculo_DescripcionModificaciones");
            entity.Property(e => e.VehiculoMarca)
                .HasMaxLength(500)
                .HasColumnName("Vehiculo_Marca");
            entity.Property(e => e.VehiculoModelo)
                .HasMaxLength(500)
                .HasColumnName("Vehiculo_Modelo");
            entity.Property(e => e.VehiculoPlacas)
                .HasMaxLength(50)
                .HasColumnName("Vehiculo_Placas");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_Cuenta");

            entity.HasOne(d => d.EstatusServicio).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.EstatusServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_EstatusServicio");

            entity.HasOne(d => d.Grua).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.GruaId)
                .HasConstraintName("FK_Servicio_Grua");

            entity.HasOne(d => d.Municipio).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.MunicipioId)
                .HasConstraintName("FK_Servicio_Municipio");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.ProveedorId)
                .HasConstraintName("FK_Servicio_Proveedor");

            entity.HasOne(d => d.TipoServicio).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.TipoServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_TipoServicio");
        });

        modelBuilder.Entity<TipoGrua>(entity =>
        {
            entity.ToTable("TipoGrua");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<TipoServicio>(entity =>
        {
            entity.ToTable("TipoServicio");

            entity.Property(e => e.TipoServicioId).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
