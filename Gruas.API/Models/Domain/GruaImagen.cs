using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class GruaImagen
{
    public Guid Id { get; set; }

    public Guid GruaId { get; set; }

    public int Orden { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreadionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacionId { get; set; }

    public virtual Grua Grua { get; set; } = null!;
}
