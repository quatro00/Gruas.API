﻿using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class Propuestum
{
    public Guid Id { get; set; }

    public Guid GruaId { get; set; }

    public Guid ServicioId { get; set; }

    public decimal MontoPropuesto { get; set; }

    public decimal TiempoPropuesto { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreadionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacionId { get; set; }

    public virtual Grua Grua { get; set; } = null!;

    public virtual Servicio Servicio { get; set; } = null!;
}
