using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class TipoServicio
{
    public int TipoServicioId { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
