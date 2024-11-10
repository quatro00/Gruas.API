using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class PagoDetalle
{
    public Guid PagoId { get; set; }

    public Guid ServicioId { get; set; }

    public int Detalle { get; set; }

    public decimal SubTotal { get; set; }

    public decimal Comision { get; set; }

    public decimal Total { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public virtual Pago Pago { get; set; } = null!;

    public virtual Servicio Servicio { get; set; } = null!;
}
