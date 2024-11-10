using System;
using System.Collections.Generic;

namespace Gruas.API.Models.Domain;

public partial class EstatusPago
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
