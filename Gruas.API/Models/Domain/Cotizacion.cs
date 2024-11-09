using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class Cotizacion
{
    public Guid GruaId { get; set; }

    public Guid ServicioId { get; set; }

    public decimal TiempoArrivo { get; set; }

    public decimal Cotizacion1 { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public virtual Grua Grua { get; set; } = null!;

    public virtual Servicio Servicio { get; set; } = null!;
}
