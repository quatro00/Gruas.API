using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class Municipio
{
    public Guid Id { get; set; }

    public Guid EstadoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string NombreCorto { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreadionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacionId { get; set; }

    public virtual Estado Estado { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
