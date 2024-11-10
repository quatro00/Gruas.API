using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class Pago
{
    public Guid Id { get; set; }

    public int Folio { get; set; }

    public decimal Monto { get; set; }

    public Guid ProveedorId { get; set; }

    public string Concepto { get; set; } = null!;

    public int EstatusPagoId { get; set; }

    public string? Referencia { get; set; }

    public DateTime? FechaPago { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public virtual EstatusPago EstatusPago { get; set; } = null!;

    public virtual ICollection<PagoDetalle> PagoDetalles { get; set; } = new List<PagoDetalle>();

    public virtual Proveedor Proveedor { get; set; } = null!;
}
