using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class Proveedor
{
    public Guid Id { get; set; }

    public int NoProveedor { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string Telefono1 { get; set; } = null!;

    public string Telefono2 { get; set; } = null!;

    public string Rfc { get; set; } = null!;

    public string Cuenta { get; set; } = null!;

    public string Banco { get; set; } = null!;

    public Guid EstadoId { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreadionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacionId { get; set; }

    public virtual ICollection<Cuentum> CuentaNavigation { get; set; } = new List<Cuentum>();

    public virtual Estado Estado { get; set; } = null!;

    public virtual ICollection<Grua> Gruas { get; set; } = new List<Grua>();

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
